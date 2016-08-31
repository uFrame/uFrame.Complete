namespace uFrame.Editor.Database.Data
{
    public interface IDataRecordPropertyChanged
    {
        void RecordPropertyChanged(IDataRecord record, string name,object previousValue, object nextValue);
    }
}