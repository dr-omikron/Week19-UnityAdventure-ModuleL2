using System.Collections;
using _Project.Develop.Runtime.Utilities.CoroutinesManagement;
using _Project.Develop.Runtime.Utilities.DataManagement.DataProviders;

namespace _Project.Develop.Runtime.Utilities.DataManagement
{
    public class SaveLoadDataProvidersService
    {
        private readonly PlayerCurrencyProvider _playerCurrencyProvider;
        private readonly PlayerStatisticProvider _playerStatisticProvider;
        private readonly ICoroutinesPerformer _coroutinesPerformer;

        public SaveLoadDataProvidersService(
            PlayerCurrencyProvider playerCurrencyProvider, 
            PlayerStatisticProvider playerStatisticProvider, 
            ICoroutinesPerformer coroutinesPerformer)
        {
            _playerCurrencyProvider = playerCurrencyProvider;
            _playerStatisticProvider = playerStatisticProvider;
            _coroutinesPerformer = coroutinesPerformer;
        }

        public IEnumerator CheckExistsSaving()
        {
            yield return CheckPlayerCurrencySaveFile();
            yield return CheckPlayerStatisticSaveFile();
        }

        public void SaveAll() => _coroutinesPerformer.StartPerform(SaveProcess());

        private IEnumerator SaveProcess()
        {
            yield return _playerCurrencyProvider.Save();
            yield return _playerStatisticProvider.Save();
        }

        private IEnumerator CheckPlayerCurrencySaveFile()
        {
            bool isPlayerDataSaveExist = false;

            yield return _playerCurrencyProvider.Exists(result => isPlayerDataSaveExist = result);

            if (isPlayerDataSaveExist)
                yield return _playerCurrencyProvider.Load();
            else
                _playerCurrencyProvider.Reset();
        }

        private IEnumerator CheckPlayerStatisticSaveFile()
        {
            bool isPlayerDataSaveExist = false;

            yield return _playerStatisticProvider.Exists(result => isPlayerDataSaveExist = result);

            if (isPlayerDataSaveExist)
                yield return _playerStatisticProvider.Load();
            else
                _playerStatisticProvider.Reset();
        }
    }
}
