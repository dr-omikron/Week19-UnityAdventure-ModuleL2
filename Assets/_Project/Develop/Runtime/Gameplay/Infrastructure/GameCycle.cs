using System;
using System.Collections;
using _Project.Develop.Runtime.Gameplay.Configs;
using _Project.Develop.Runtime.Gameplay.Inputs;
using _Project.Develop.Runtime.Gameplay.Services;
using _Project.Develop.Runtime.Meta.Features;
using _Project.Develop.Runtime.Utilities.CoroutinesManagement;
using _Project.Develop.Runtime.Utilities.Factories;
using _Project.Develop.Runtime.Utilities.PlayerInput;
using _Project.Develop.Runtime.Utilities.SceneManagement;
using UnityEngine;

namespace _Project.Develop.Runtime.Gameplay.Infrastructure
{
    public class GameCycle : IDisposable
    {
        private readonly GameplayServicesFactory _gameplayServicesFactory;
        private readonly ProjectServicesFactory _projectServicesFactory;
        private readonly GameplayPlayerInputs _gameplayPlayerInputs;
        private readonly ICoroutinesPerformer _coroutinesPerformer;
        private readonly GameplayInputArgs _inputArgs;

        private PlayerProgressTracker _playerProgressTracker;
        private WalletService _walletService;
        private LevelConfig _levelConfig;

        public GameCycle(
            GameplayServicesFactory gameplayServicesFactory, 
            ProjectServicesFactory projectServicesFactory, 
            GameplayPlayerInputs gameplayPlayerInputs, 
            ICoroutinesPerformer coroutinesPerformer,
            GameplayInputArgs inputArgs)
        {
            _gameplayServicesFactory = gameplayServicesFactory;
            _projectServicesFactory = projectServicesFactory;
            _gameplayPlayerInputs = gameplayPlayerInputs;
            _coroutinesPerformer = coroutinesPerformer;
            _inputArgs = inputArgs;
        }

        public IEnumerator Start()
        {
            _playerProgressTracker = _projectServicesFactory.GetPlayerProgressTracker();
            _levelConfig = _projectServicesFactory.GetConfigsProviderService().GetConfig<LevelConfig>();
            _walletService = _projectServicesFactory.GetWalletService();

            SymbolsSequenceGenerator symbolsSequenceGenerator = _gameplayServicesFactory.GetSymbolsSequenceGenerator();
            string generated = symbolsSequenceGenerator.Generate(_inputArgs.Symbols, _inputArgs.SequenceLenght);

            Debug.Log($"Retry symbols sequence - { generated }");

            InputStringReader inputStringReader = _gameplayServicesFactory.GetInputStringReader();
            yield return _coroutinesPerformer.StartPerform(inputStringReader.StartProcess(_inputArgs.SequenceLenght));

            if (string.Equals(inputStringReader.CurrentInput, generated, StringComparison.OrdinalIgnoreCase))
                ProcessWin();
            else
                ProcessDefeat();
        }

        private void ProcessWin()
        {
            Debug.Log("Win");
            Debug.Log($"Press { KeyboardInputKeys.EndGameKey } to Return in Main Menu");
            _gameplayPlayerInputs.EndGameKeyDown += OnMainMenuReturn;

            _playerProgressTracker.AddWin();
            _walletService.Add(_levelConfig.WinGoldAmount);
        }

        private void ProcessDefeat()
        {
            Debug.Log("Defeat");
            Debug.Log($"Press { KeyboardInputKeys.EndGameKey } to Restart Game");
            _gameplayPlayerInputs.EndGameKeyDown += OnRestartGame;

            _playerProgressTracker.AddLoss();

            if(_walletService.Enough(_levelConfig.DefeatGoldAmount))
                _walletService.Spend(_levelConfig.DefeatGoldAmount);
            else
                _walletService.Reset();
        }

        private void OnMainMenuReturn()
        {
            UnsubscribeAll();
            _coroutinesPerformer.StartPerform(ReturnToMainMenu());
        }

        private void OnRestartGame()
        {
            UnsubscribeAll();
            _coroutinesPerformer.StartPerform(Start());
        }

        private IEnumerator ReturnToMainMenu()
        {
            SceneSwitcherService sceneSwitcherService = _projectServicesFactory.GetSceneSwitcherService();
            yield return sceneSwitcherService.ProcessSwitchTo(Scenes.MainMenu);
        }

        private void UnsubscribeAll()
        {
            _gameplayPlayerInputs.EndGameKeyDown -= OnMainMenuReturn;
            _gameplayPlayerInputs.EndGameKeyDown -= OnRestartGame;
        }

        public void Dispose() => UnsubscribeAll();
    }
}
