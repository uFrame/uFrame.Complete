using UnityEngine;

namespace uFrame.ECS.UnityUtilities
{
    public class EntityPrefabPool : MonoBehaviour
    {
        //public override int ComponentId
        //{
        //    get { return -1; }
        //}

        //[SerializeField]
        //private string _name;

        //[SerializeField]
        //private EntityPrefab[] _prefabs;

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        //public EntityPrefab[] Prefabs
        //{
        //    get { return _prefabs; }
        //    set { _prefabs = value; }
        //}
    }
}