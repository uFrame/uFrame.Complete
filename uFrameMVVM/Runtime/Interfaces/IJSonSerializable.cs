
namespace uFrame.MVVM
{
    public interface IJSonSerializable
    {
        string Serialize();
        void Deserialize(string json);
    }
}