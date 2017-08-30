using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace uFrame.Kernel
{
    public class SceneManagementService : SystemServiceMonoBehavior
    {
        private Queue<SceneQueueItem> _scenesQueue;

        public Queue<SceneQueueItem> ScenesQueue
        {
            get { return _scenesQueue ?? (_scenesQueue = new Queue<SceneQueueItem>()); }
        }

        public List<IScene> LoadedScenes
        {
            get { return _loadedScenes ?? (_loadedScenes = new List<IScene>()); }
        }

        private List<IScene> _loadedScenes;

        public override void Setup()
        {
            base.Setup();


            this.OnEvent<LoadSceneCommand>().Subscribe(_ =>
            {

                this.LoadScene(_.SceneName, _.Settings, _.RestrictToSingleScene);
            });

            this.OnEvent<UnloadSceneCommand>().Subscribe(_ =>
            {
                this.UnloadScene(_.SceneName);
            });

            var attachedSceneLoaders =
                uFrameKernel.Instance.GetComponentsInChildren(typeof (ISceneLoader)).OfType<ISceneLoader>();
            foreach (var sceneLoader in attachedSceneLoaders)
            {
                uFrameKernel.Container.RegisterSceneLoader(sceneLoader);
                uFrameKernel.Container.Inject(sceneLoader);
                SceneLoaders.Add(sceneLoader);
            }
            _defaultSceneLoader = gameObject.GetComponent<DefaultSceneLoader>() ??
                                  gameObject.AddComponent<DefaultSceneLoader>();

            this.OnEvent<SceneAwakeEvent>().Subscribe(_ => StartCoroutine(SetupScene(_.Scene)));
        }

        private List<ISceneLoader> _sceneLoaders;

        public List<ISceneLoader> SceneLoaders
        {
            get { return _sceneLoaders ?? (_sceneLoaders = new List<ISceneLoader>()); }
        }


        private DefaultSceneLoader _defaultSceneLoader;


        public IEnumerator LoadSceneInternal(string sceneName)
        {
            yield return StartCoroutine(InstantiateSceneAsyncAdditively(sceneName));
        }

        public void QueueSceneLoad(string sceneName, ISceneSettings settings)
        {
            ScenesQueue.Enqueue(new SceneQueueItem()
            {
                Loader = LoadSceneInternal(sceneName),
                Name = sceneName,
                Settings = settings
            });
        }

        public void QueueScenesLoad(params SceneQueueItem[] items)
        {
            foreach (var item in items)
            {
                if (item.RestrictToSingleScene &&
                    (LoadedScenes.Any(p => p.Name == name)
                     || ScenesQueue.Any(p => p.Name == name)
                     || SceneManager.GetSceneByName(name).isLoaded)) continue;
                    // Application.loadedLevelName == name)) continue;
                if (item.Loader == null)
                {
                    item.Loader = LoadSceneInternal(item.Name);
                }
                ScenesQueue.Enqueue(item);
            }
        }

        protected IEnumerator ExecuteLoadAsync()
        {
            foreach (var sceneQueeItem in ScenesQueue.ToArray())
            {
                yield return StartCoroutine(sceneQueeItem.Loader);
            }
        }

        public void ExecuteLoad()
        {
            StartCoroutine(ExecuteLoadAsync());
        }

        public IEnumerator SetupScene(IScene sceneRoot)
        {

            this.Publish(new SceneLoaderEvent()
            {
                State = SceneState.Instantiating,
                SceneRoot = sceneRoot
            });

            //If the scene was loaded via the api (it was queued having some name and settings)
            if (ScenesQueue.Count > 0)
            {
                var sceneQueueItem = ScenesQueue.Dequeue();
                sceneRoot.Name = sceneQueueItem.Name;
                sceneRoot._SettingsObject = sceneQueueItem.Settings;
            }
            //Else, means scene was the start scene (loaded before kernel)
            else
            {
                //sceneRoot.Name = Application.loadedLevelName;
                sceneRoot.Name = SceneManager.GetActiveScene().name;
            }


            this.Publish(new SceneLoaderEvent()
            {
                State = SceneState.Instantiated,
                SceneRoot = sceneRoot
            });
            var sceneRootClosure = sceneRoot;
            Action<float, string> updateDelegate = (v, m) =>
            {
                this.Publish(new SceneLoaderEvent()
                {
                    SceneRoot = sceneRootClosure,
                    Name = sceneRootClosure.Name,
                    State = SceneState.Update,
                    Progress = v,
                    ProgressMessage = m
                });
            };

            var sceneLoader = SceneLoaders.FirstOrDefault(loader => loader.SceneType == sceneRoot.GetType()) ??
                              _defaultSceneLoader;

            yield return StartCoroutine(sceneLoader.Load(sceneRoot, updateDelegate));

            LoadedScenes.Add(sceneRoot);

            this.Publish(new SceneLoaderEvent()
            {
                State = SceneState.Loaded,
                SceneRoot = sceneRoot
            });


        }

        protected IEnumerator UnloadSceneAsync(string name)
        {
            var sceneRoot = LoadedScenes.FirstOrDefault(s => s.Name == name);
            if (sceneRoot != null) yield return StartCoroutine(this.UnloadSceneAsync(sceneRoot));
            else yield break;
        }

        protected IEnumerator UnloadSceneAsync(IScene sceneRoot)
        {

            var sceneLoader = SceneLoaders.FirstOrDefault(loader => loader.SceneType == sceneRoot.GetType()) ??
                              _defaultSceneLoader;

            Action<float, string> updateDelegate = (v, m) =>
            {
                this.Publish(new SceneLoaderEvent()
                {
                    State = SceneState.Unloading,
                    Progress = v,
                    ProgressMessage = m
                });
            };

            yield return StartCoroutine(sceneLoader.Unload(sceneRoot, updateDelegate));

            LoadedScenes.Remove(sceneRoot);
            this.Publish(new SceneLoaderEvent() {State = SceneState.Unloaded, SceneRoot = sceneRoot});

            AsyncOperation unloadSceneAsync = SceneManager.UnloadSceneAsync(((MonoBehaviour) sceneRoot).gameObject.scene);
            while (!unloadSceneAsync.isDone) {
                yield return null;
            }

            this.Publish(new SceneLoaderEvent() {State = SceneState.Destructed, SceneRoot = sceneRoot});
        }

        public void UnloadScene(string name)
        {
            StartCoroutine(UnloadSceneAsync(name));
        }

        public void UnloadScene(IScene sceneRoot)
        {
            StartCoroutine(UnloadSceneAsync(sceneRoot));
        }

        public void UnloadScenes(string[] names)
        {
            foreach (var name in names)
            {
                StartCoroutine(UnloadSceneAsync(name));
            }
        }

        public void UnloadScenes(IScene[] sceneRoots)
        {
            foreach (var sceneRoot in sceneRoots)
            {
                StartCoroutine(UnloadSceneAsync(sceneRoot));
            }
        }

        public void LoadScene(string name, ISceneSettings settings, bool restrictToSingleScene)
        {
            if (restrictToSingleScene &&
                (LoadedScenes.Any(p => p.Name == name)
              || ScenesQueue.Any(p => p.Name == name)
              || SceneManager.GetSceneByName(name).isLoaded)) return;
                 //Application.loadedLevelName == name)) return;
            this.QueueSceneLoad(name, settings);
            this.ExecuteLoad();
        }

        public void LoadScenes(params SceneQueueItem[] items)
        {

            this.QueueScenesLoad(items);
            this.ExecuteLoad();
        }

        public void QueueSceneLoadIfNotAlready(string sceneName, ISceneSettings settings)
        {

            if (LoadedScenes.Any(p => p.Name == sceneName)
             || ScenesQueue.Any(p => p.Name == sceneName)
             || SceneManager.GetSceneByName(sceneName).isLoaded)
                //Application.loadedLevelName == sceneName)
            {
                return;
            }
            ScenesQueue.Enqueue(new SceneQueueItem()
            {
                Loader = LoadSceneInternal(sceneName),
                Name = sceneName,
                Settings = settings
            });
        }

        public static IEnumerator InstantiateSceneAsyncAdditively(string sceneName)
        {
            var asyncOperation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
            float lastProgress = -1;
            while (!asyncOperation.isDone)
            {
                if (lastProgress != asyncOperation.progress)
                {
                    uFrameKernel.EventAggregator.Publish(new SceneLoaderEvent()
                    {
                        State = SceneState.Instantiating,
                        Name = sceneName,
                        Progress = asyncOperation.progress
                    });
                    lastProgress = asyncOperation.progress;
                }
                yield return new WaitForSeconds(0.1f);
            }
        }
    }

    public class SceneQueueItem
    {
        public string Name { get; set; }
        public IEnumerator Loader { get; set; }
        public ISceneSettings Settings { get; set; }
        public bool RestrictToSingleScene { get; set; }
    }
}