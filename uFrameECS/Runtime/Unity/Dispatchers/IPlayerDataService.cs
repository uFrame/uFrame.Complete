namespace uFrame.ECS.UnityUtilities
{
    public interface IPlayerDataService
    {
        void Save(SavePlayerData data);
        void Load(LoadPlayerData data);
    }
}