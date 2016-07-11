using uFrame.Json;

namespace uFrame.Editor.Graphs.Data
{
    public interface IJsonObject
    {
        void Serialize(JSONClass cls);
        void Deserialize(JSONClass cls);
    }
}