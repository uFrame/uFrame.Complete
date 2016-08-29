using uFrame.Json;
using UnityEngine;

namespace uFrame.ECS.UnityUtilities
{
    public class JsonComponentRepository : ComponentRepositoryBehaviour
    {
        public virtual JSONNode GetDataByEntity(int entityId, int componentId)
        {
            return JSON.Parse(PlayerPrefs.GetString(entityId + "_" + componentId, null));
        }

        public virtual void SetDataByEntity(int entityId, int componentId, JSONNode data)
        {
            PlayerPrefs.SetString(entityId + "_" + componentId, data.ToString());
        }

        public override void Initialize(PlayerDataGroup @group)
        {
            
        }

        public override void LoadComponent(IPlayerDataComponent ecsComponent)
        {

            var jsonData = GetDataByEntity(ecsComponent.EntityId, ecsComponent.ComponentId);
            if (jsonData != null)
                JsonExtensions.DeserializeExistingObject(ecsComponent, jsonData.AsObject);
            ecsComponent.IsDirty = false;
        }

        public override void SaveComponent(IPlayerDataComponent ecsComponent)
        {
            var json = JsonExtensions.SerializeObject(ecsComponent);
            SetDataByEntity(ecsComponent.EntityId, ecsComponent.ComponentId, json);
            ecsComponent.IsDirty = false;
        }
    }
}