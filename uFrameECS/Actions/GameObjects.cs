using uFrame.Attributes;
using UnityEngine;

namespace uFrame.ECS.Actions
{
    [ActionLibrary, uFrameCategory("GameObject")]
    public static class GameObjects
    {


        [ActionTitle("Instantiate GameObject")]
        public static GameObject Instantiate(GameObject gameObject, Vector3 position, Vector3 rotation)
        {
            return Object.Instantiate(gameObject, position, Quaternion.Euler(rotation)) as GameObject;
        }

        [ActionTitle("Deactivate GameObject")]
        public static void DeactiateGameObject(GameObject gameObject, MonoBehaviour behaviour)
        {
            if (gameObject != null)
            {
                gameObject.SetActive(false);
                return;
            }
            if (behaviour != null)
            {
                behaviour.gameObject.SetActive(false);
            }
        }
        [ActionTitle("Activate GameObject")]
        public static void ActivateGameObject(GameObject gameObject, MonoBehaviour behaviour)
        {
            if (gameObject != null)
            {
                gameObject.SetActive(true);
                return;
            }
            if (behaviour != null)
            {
                behaviour.gameObject.SetActive(true);
            }
        }
    }
}