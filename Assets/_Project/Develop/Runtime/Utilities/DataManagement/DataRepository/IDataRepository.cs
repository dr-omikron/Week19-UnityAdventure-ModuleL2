using System;
using System.Collections;

namespace _Project.Develop.Runtime.Utilities.DataManagement.DataRepository
{
    public interface IDataRepository
    {
        IEnumerator Read(string key, Action<string> onRead);
        IEnumerator Write(string key, string serializedData);
        IEnumerator Exists(string key, Action<bool> onExistsResult);
        IEnumerator Remove(string key);
    }
}
