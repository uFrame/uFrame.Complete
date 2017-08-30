#define FAST_EVENTS
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using uFrame.Attributes;
using uFrame.IOC;
using UniRx;
using UnityEngine.SceneManagement;

namespace uFrame.Kernel
{
    public class uFrameKernel : MonoBehaviour
    {

        private static UFrameContainer _container;
        private static IEventAggregator _eventAggregator;

        private static bool _isKernelLoaded;
        private List<ISystemService> _services;
        private List<ISystemLoader> _systemLoaders;

        public static bool IsKernelLoaded
        {
            get { return _isKernelLoaded; }
            private set { _isKernelLoaded = value; }
        }

        public static uFrameKernel Instance { get; private set; }

        public static IUFrameContainer Container
        {
            get
            {
                if (_container == null)
                {
                    _container = new UFrameContainer();
                    _container.RegisterInstance<IUFrameContainer>(_container);
                    _container.RegisterInstance<IEventAggregator>(EventAggregator);

                }
                return _container;
            }
        }

        public static IEventAggregator EventAggregator
        {
            get { return _eventAggregator ?? (_eventAggregator = new EcsEventAggregator()); }
            set { _eventAggregator = value; }
        }

        public List<ISystemLoader> SystemLoaders
        {
            get { return _systemLoaders ?? (_systemLoaders = new List<ISystemLoader>()); }
        }

        public List<ISystemService> Services
        {
            get { return _services ?? (_services = new List<ISystemService>()); }
        }

        private void Awake()
        {
            if (Instance != null)
            {
                throw new Exception("Loading Kernel twice is not a good practice!");
            }
            else
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
                StartCoroutine(Startup());
            }
        }

        private IEnumerator Startup()
        {
            var attachedSystemLoaders =
                gameObject.GetComponentsInChildren(typeof (ISystemLoader)).OfType<ISystemLoader>();

            foreach (var systemLoader in attachedSystemLoaders)
            {
                this.Publish(new SystemLoaderEvent() {State = SystemState.Loading, Loader = systemLoader});
                systemLoader.Container = Container;
                systemLoader.EventAggregator = EventAggregator;
                systemLoader.Load();
                yield return StartCoroutine(systemLoader.LoadAsync());
                SystemLoaders.Add(systemLoader);
                this.Publish(new SystemLoaderEvent() {State = SystemState.Loaded, Loader = systemLoader});
            }

            var attachedServices = gameObject.GetComponentsInChildren(typeof (SystemServiceMonoBehavior))
                .OfType<SystemServiceMonoBehavior>()
                .Where(_ => _.enabled)
                .ToArray();

            foreach (var service in attachedServices)
            {
                Container.RegisterService(service);

            }

            Container.InjectAll();
            var allServices = Container.ResolveAll<ISystemService>().ToArray();
            foreach (var item in allServices)
                Services.Add(item);

            for (int index = 0; index < allServices.Length; index++)
            {
                var service = allServices[index];
                this.Publish(new ServiceLoaderEvent() { State = ServiceState.Loading, Service = service, GlobalProgress = (index+1)/(float)allServices.Length });
                yield return StartCoroutine(service.SetupAsync());
                this.Publish(new ServiceLoaderEvent() { State = ServiceState.Loaded, Service = service });

            }
            foreach (var service in allServices)
            {
                service.Setup();
            }
            foreach (var service in allServices)
            {
                service.Loaded();
            }

            this.Publish(new SystemsLoadedEvent()
            {
                Kernel = this
            });

            _isKernelLoaded = true;

            this.Publish(new KernelLoadedEvent()
            {
                Kernel = this
            });
            yield return new WaitForEndOfFrame(); //Ensure that everything is bound
            this.Publish(new GameReadyEvent());
        }

        public void OnDestroy()
        {
            _container = null;
            IsKernelLoaded = false;
            Services.Clear();
            SystemLoaders.Clear();
            EventAggregator = null;
            Instance = null;
        }

        public void ResetKernel()
        {
            DestroyImmediate(Instance.gameObject);
            _container = null;
            IsKernelLoaded = false;
            Services.Clear();
            SystemLoaders.Clear();
            EventAggregator = null;
            Instance = null;
        }

        public static void DestroyKernel(string levelToLoad = null)
        {
            Instance.ResetKernel();
            if (levelToLoad != null)
                SceneManager.LoadScene(levelToLoad);
        }
    }

    public struct SystemsLoadedEvent
    {
        public uFrameKernel Kernel;
    }

    /// <summary>
    /// This is invoked directly after all scenes of
    /// </summary>
    [uFrameEvent("Kernel Loaded")]
    public struct KernelLoadedEvent
    {
        public uFrameKernel Kernel;
    }

    /// <summary>
    /// The game ready event is invoked after the kernel has loaded and two addditional frames have occured.
    /// </summary>
    [uFrameEvent("Game Ready")]
    public struct GameReadyEvent
    {

    }

    public struct LoadSceneCommand
    {

        public string SceneName { get; set; }
        public ISceneSettings Settings { get; set; }
        public bool RestrictToSingleScene { get; set; }
    }

    public struct UnloadSceneCommand
    {
        public string SceneName { get; set; }
    }

    public struct SystemLoaderEvent
    {
        public SystemState State { get; set; }
        public ISystemLoader Loader { get; set; }
    }

    public struct ServiceLoaderEvent
    {
        public ServiceState State { get; set; }
        public ISystemService Service { get; set; }
        public float GlobalProgress { get; set; }
    }

    public struct SceneLoaderEvent
    {
        public SceneState State { get; set; }
        public IScene SceneRoot { get; set; }
        public float Progress { get; set; }
        public string ProgressMessage { get; set; }
        public string Name { get; set; }
    }

    public enum SceneState
    {
        Loading,
        Update,
        Loaded,
        Unloading,
        Unloaded,
        Instantiating,
        Instantiated,
        Destructed
    }

    public enum ServiceState
    {
        Loading,
        Loaded,
        Unloaded,
    }

    public enum SystemState
    {
        Loading,
        Loaded,
        Unloaded,
    }

    public static class uFrameKernelExtensions
    {
        public static void RegisterService(this IUFrameContainer container, ISystemService service)
        {
            container.RegisterInstance<ISystemService>(service, service.GetType().Name);
            //container.RegisterInstance(typeof(TService), service, false);
            container.RegisterInstance(service.GetType(), service);
        }

        public static void RegisterService<TService>(this IUFrameContainer container, ISystemService service)
        {
            container.RegisterInstance<ISystemService>(service, service.GetType().Name);
            container.RegisterInstance(typeof (TService), service);
        }

        public static void RegisterSceneLoader(this IUFrameContainer container, ISceneLoader sceneLoader)
        {
            container.RegisterInstance<ISceneLoader>(sceneLoader, sceneLoader.GetType().Name, false);
            container.RegisterInstance(sceneLoader.GetType(), sceneLoader, false);
        }

        public static void Publish<TEvent>(this uFrameKernel kernel, TEvent evt)
        {
            uFrameKernel.EventAggregator.Publish(evt);
        }

        public static IObservable<TEvent> OnEvent<TEvent>(this uFrameKernel kernel)
        {
            return uFrameKernel.EventAggregator.GetEvent<TEvent>();
        }
    }
}