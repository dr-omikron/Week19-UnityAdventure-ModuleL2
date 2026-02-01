using System;
using System.Collections;
using _Project.Develop.Runtime.Utilities.DataManagement.DataRepository;
using _Project.Develop.Runtime.Utilities.DataManagement.KeyStorage;
using _Project.Develop.Runtime.Utilities.DataManagement.Serializers;

namespace _Project.Develop.Runtime.Utilities.DataManagement
{
    public class SaveLoadService : ISaveLoadService
    {
        private readonly IDataSerializer _dataSerializer;
        private readonly IDataKeyStorage _dataKeyStorage;
        private readonly IDataRepository _dataRepository;

        public SaveLoadService(IDataSerializer dataSerializer, IDataKeyStorage dataKeyStorage, IDataRepository dataRepository)
        {
            _dataSerializer = dataSerializer;
            _dataKeyStorage = dataKeyStorage;
            _dataRepository = dataRepository;
        }

        public IEnumerator Load<TData>(Action<TData> onLoad) where TData : ISaveData
        {
            string key = _dataKeyStorage.GetKeyFor<TData>();
            string serializedData = "";

            yield return _dataRepository.Read(key, result => serializedData = result);

            TData data = _dataSerializer.Deserialize<TData>(serializedData);
            onLoad?.Invoke(data);
        }

        public IEnumerator Save<TData>(TData data) where TData : ISaveData
        {
            string serializedData = _dataSerializer.Serialize(data);
            string key = _dataKeyStorage.GetKeyFor<TData>();

            yield return _dataRepository.Write(key, serializedData);
        }

        public IEnumerator Exists<TData>(Action<bool> onExistsResult) where TData : ISaveData
        {
            string key = _dataKeyStorage.GetKeyFor<TData>();

            yield return _dataRepository.Exists(key, result => onExistsResult?.Invoke(result));
        }

        public IEnumerator Remove<TData>() where TData : ISaveData
        {
            string key = _dataKeyStorage.GetKeyFor<TData>();

            yield return _dataRepository.Remove(key);
        }
    }
}
