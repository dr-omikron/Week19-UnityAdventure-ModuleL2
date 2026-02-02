using System;
using System.Collections;
using System.Collections.Generic;

namespace _Project.Develop.Runtime.Utilities.DataManagement.DataProviders
{
    public abstract class DataProvider<TData> where TData : ISaveData
    {
        private readonly ISaveLoadService _saveLoadService;

        private readonly List<IDataWriter<TData>> _dataWriters =  new List<IDataWriter<TData>>();
        private readonly List<IDataReader<TData>> _dataReaders =  new List<IDataReader<TData>>();

        private TData _data;

        protected DataProvider(ISaveLoadService saveLoadService)
        {
            _saveLoadService = saveLoadService;
        }

        protected TData Data => _data;

        public void RegisterWriter(IDataWriter<TData> writer)
        {
            if(_dataWriters.Contains(writer))
                throw new ArgumentException(nameof(writer));

            _dataWriters.Add(writer);
        }

        public void RegisterReader(IDataReader<TData> reader)
        {
            if(_dataReaders.Contains(reader))
                throw new ArgumentException(nameof(reader));

            _dataReaders.Add(reader);
        }

        public IEnumerator Load()
        {
            yield return _saveLoadService.Load<TData>(loadedData => _data = loadedData);

            SendDataToReaders();
        }

        public IEnumerator Save()
        {
            UpdateDataFromWriters();

            yield return _saveLoadService.Save(_data);
        }

        public IEnumerator Exists(Action<bool> onExistsResult)
        {
            yield return _saveLoadService.Exists<TData>(existsResult => onExistsResult?.Invoke(existsResult));
        }

        public IEnumerator Remove()
        {
            yield return _saveLoadService.Remove<TData>();
        }

        public void Reset()
        {
            _data = GetOriginData();
            SendDataToReaders();
        }

        protected abstract TData GetOriginData();

        private void SendDataToReaders()
        {
            foreach (IDataReader<TData> dataReader in _dataReaders)
                dataReader.ReadFrom(_data);
        }

        private void UpdateDataFromWriters()
        {
            foreach (IDataWriter<TData> dataWriter in _dataWriters)
                dataWriter.WriteTo(_data);
        }
    }
}
