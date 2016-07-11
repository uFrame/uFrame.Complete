using uFrame.Json;

namespace uFrame.Editor.Undo
{
    public class RedoItem : TransactionItem
    {
        private string _undoData;
        [JsonProperty]
        public string UndoData
        {
            get { return _undoData; }
            set
            {
                _undoData = value;
                Changed = true;
            }
        }
    }
}