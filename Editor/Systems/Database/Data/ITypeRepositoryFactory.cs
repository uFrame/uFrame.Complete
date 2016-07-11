using System;
using System.Collections.Generic;

namespace uFrame.Editor.Database.Data
{
    public interface ITypeRepositoryFactory
    {
        IDataRecordManager CreateRepository(IRepository typeDatabase, Type type);
        IEnumerable<IDataRecordManager> CreateAllManagers(IRepository repository);
    }
}