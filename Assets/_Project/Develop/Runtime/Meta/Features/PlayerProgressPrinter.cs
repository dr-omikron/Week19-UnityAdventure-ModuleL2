using System;
using _Project.Develop.Runtime.Gameplay.Services;
using _Project.Develop.Runtime.Meta.Infrastructure;
using UnityEngine;

namespace _Project.Develop.Runtime.Meta.Features
{
    public class PlayerProgressPrinter : IDisposable
    {
        private readonly PlayerProgressTracker _playerProgressTracker;
        private readonly WalletService _walletService;
        private readonly MainMenuPlayerInputs _mainMenuPlayerInputs;

        public PlayerProgressPrinter(
            PlayerProgressTracker playerProgressTracker, 
            WalletService walletService,
            MainMenuPlayerInputs mainMenuPlayerInputs)
        {
            _playerProgressTracker = playerProgressTracker;
            _walletService = walletService;
            _mainMenuPlayerInputs = mainMenuPlayerInputs;

            _mainMenuPlayerInputs.ShowInfoKeyDown += Print;
        }

        public void Print()
        {
            Debug.Log($"Победы: { _playerProgressTracker.Wins }, Поражения: { _playerProgressTracker.Losses }, Золото: { _walletService.Gold.Value }");
        }

        public void Dispose() => _mainMenuPlayerInputs.ShowInfoKeyDown -= Print;
    }
}
