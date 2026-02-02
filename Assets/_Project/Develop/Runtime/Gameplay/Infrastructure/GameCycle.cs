using System;
using System.Collections;
using _Project.Develop.Runtime.Gameplay.Configs;
using _Project.Develop.Runtime.Gameplay.Features;
using _Project.Develop.Runtime.Gameplay.Inputs;
using _Project.Develop.Runtime.Meta.Features;
using _Project.Develop.Runtime.Utilities.CoroutinesManagement;
using _Project.Develop.Runtime.Utilities.DataManagement;
using _Project.Develop.Runtime.Utilities.PlayerInput;
using _Project.Develop.Runtime.Utilities.SceneManagement;
using UnityEngine;

namespace _Project.Develop.Runtime.Gameplay.Infrastructure
{
    public class GameCycle : IDisposable
    {
        private readonly PlayerProgressTracker _playerProgressTracker;
        private readonly LevelConfig _levelConfig;
        private readonly WalletService _walletService;
        private readonly SymbolsSequenceGenerator _symbolsSequenceGenerator;
        private readonly InputStringReader _inputStringReader;
        private readonly SceneSwitcherService _sceneSwitcherService;
        private readonly GameplayPlayerInputs _gameplayPlayerInputs;
        private readonly ICoroutinesPerformer _coroutinesPerformer;
        private readonly SaveLoadDataProvidersService _saveLoadDataProvidersService;
        private readonly GameplayInputArgs _inputArgs;


        public GameCycle(
            PlayerProgressTracker playerProgressTracker,
            LevelConfig levelConfig,
            WalletService walletService,
            SymbolsSequenceGenerator symbolsSequenceGenerator,
            InputStringReader inputStringReader,
            SceneSwitcherService sceneSwitcherService,
            GameplayPlayerInputs gameplayPlayerInputs, 
            ICoroutinesPerformer coroutinesPerformer,
            SaveLoadDataProvidersService saveLoadDataProvidersService,
            GameplayInputArgs inputArgs)
        {
            _playerProgressTracker = playerProgressTracker;
            _levelConfig = levelConfig;
            _walletService = walletService;
            _symbolsSequenceGenerator = symbolsSequenceGenerator;
            _inputStringReader = inputStringReader;
            _sceneSwitcherService = sceneSwitcherService;
            _gameplayPlayerInputs = gameplayPlayerInputs;
            _coroutinesPerformer = coroutinesPerformer;
            _saveLoadDataProvidersService = saveLoadDataProvidersService;
            _inputArgs = inputArgs;
        }

        public IEnumerator Start()
        {
            string generated = _symbolsSequenceGenerator.Generate(_inputArgs.Symbols, _inputArgs.SequenceLenght);

            Debug.Log($"Retry symbols sequence - { generated }");

            yield return _coroutinesPerformer.StartPerform(_inputStringReader.StartProcess(_inputArgs.SequenceLenght));

            if (string.Equals(_inputStringReader.CurrentInput, generated, StringComparison.OrdinalIgnoreCase))
                ProcessWin();
            else
                ProcessDefeat();

            _saveLoadDataProvidersService.SaveAll();
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
            yield return _sceneSwitcherService.ProcessSwitchTo(Scenes.MainMenu);
        }

        private void UnsubscribeAll()
        {
            _gameplayPlayerInputs.EndGameKeyDown -= OnMainMenuReturn;
            _gameplayPlayerInputs.EndGameKeyDown -= OnRestartGame;
        }

        public void Dispose() => UnsubscribeAll();
    }
}
