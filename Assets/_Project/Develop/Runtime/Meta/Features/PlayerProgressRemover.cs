using System;
using _Project.Develop.Runtime.Gameplay.Configs;
using _Project.Develop.Runtime.Gameplay.Services;
using _Project.Develop.Runtime.Meta.Infrastructure;
using _Project.Develop.Runtime.Utilities.ConfigsManagement;
using UnityEngine;

namespace _Project.Develop.Runtime.Meta.Features
{
    public class PlayerProgressRemover : IDisposable
    {
        private readonly PlayerProgressTracker _playerProgressTracker;
        private readonly WalletService _walletService;
        private readonly MainMenuPlayerInputs _mainMenuPlayerInputs;
        private readonly LevelConfig _levelConfig;

        public PlayerProgressRemover(
            PlayerProgressTracker playerProgressTracker, 
            WalletService walletService, 
            MainMenuPlayerInputs mainMenuPlayerInputs,
            ConfigsProviderService configsProviderService)
        {
            _playerProgressTracker = playerProgressTracker;
            _walletService = walletService;
            _mainMenuPlayerInputs = mainMenuPlayerInputs;
            _levelConfig = configsProviderService.GetConfig<LevelConfig>();

            _mainMenuPlayerInputs.ResetProgressKeyDown += Remove;
        }

        public void Remove()
        {
            if(_playerProgressTracker.IsNotZeroProgress() == false)
            {
                Debug.Log("Прогресс уже сброшен");
                return;
            }

            if (_walletService.Enough(_levelConfig.ResetPrice))
            {
                _walletService.Spend(_levelConfig.ResetPrice);
                _playerProgressTracker.ResetProgress();
                Debug.Log($"Прогресс успешно сброшен, с кошелька списано { _levelConfig.ResetPrice } золота. Баланс - { _walletService.Gold.Value }");
            }
            else
            {
                Debug.Log($"{_walletService.Gold.Value} золота не достаточно для сброса прогресса.");
            }
        }

        public void Dispose() => _mainMenuPlayerInputs.ResetProgressKeyDown -= Remove;
    }
}
