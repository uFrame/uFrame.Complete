using uFrame.ECS.Components;
using uFrame.Kernel;
using UnityEngine;

namespace uFrame.ECS.UnityUtilities
{
    public class EntityPrefab : MonoBehaviour
    {

        [SerializeField]
        private string _name;

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        [SerializeField]
        private Entity _prefab;

        public Entity Prefab
        {
            get { return _prefab; }
            set { _prefab = value; }
        }
    }
}