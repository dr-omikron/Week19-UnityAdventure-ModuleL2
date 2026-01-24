using System;
using System.Collections;
using System.Collections.Generic;
using _Project.Develop.Runtime.Gameplay.Configs;
using _Project.Develop.Runtime.Gameplay.Infrastructure;
using _Project.Develop.Runtime.Gameplay.Services;
using _Project.Develop.Runtime.Infrastructure;
using _Project.Develop.Runtime.Infrastructure.DI;
using _Project.Develop.Runtime.Meta.Features;
using _Project.Develop.Runtime.Utilities.CoroutinesManagement;
using _Project.Develop.Runtime.Utilities.Factories;
using _Project.Develop.Runtime.Utilities.ObjectsLifetimeManagement;
using _Project.Develop.Runtime.Utilities.PlayerInput;
using _Project.Develop.Runtime.Utilities.SceneManagement;
using UnityEngine;

namespace _Project.Develop.Runtime.Meta.Infrastructure
{
    public class MainMenuBootstrap : SceneBootstrap
    {
        private DIContainer _container;
        private ProjectServicesFactory _projectServicesFactory;
        private ObjectsUpdater _objectsUpdater;
        private SelectGameModeArgsService _selectGameModeArgsService;
        private PlayerProgressPrinter _playerProgressPrinter;
        private PlayerProgressRemover _playerProgressRemover;
        private MainMenuPlayerInputs _mainMenuPlayerInputs;
        private LevelConfig _levelConfig;
        private bool _isRun;

        private List<IDisposable> _disposables;

        public override void ProcessRegistration(DIContainer container, IInputSceneArgs sceneArgs = null)
        {
            _container = container;
            MainMenuContextRegistrations.Process(_container);
        }

        public override IEnumerator Initialize()
        {
            _projectServicesFactory = new ProjectServicesFactory(_container);
            MainMenuServicesFactory mainMenuServicesFactory = new MainMenuServicesFactory(_container);

            _levelConfig = _projectServicesFactory.GetConfigsProviderService().GetConfig<LevelConfig>();

            _objectsUpdater = _projectServicesFactory.GetObjectsUpdater();
            _mainMenuPlayerInputs = mainMenuServicesFactory.GetMainMenuPlayerInputs();
            _selectGameModeArgsService = mainMenuServicesFactory.GetSelectGameModeArgsService();
            _playerProgressPrinter = mainMenuServicesFactory.GetPlayerProgressPrinter();
            _playerProgressRemover = mainMenuServicesFactory.GetPlayerProgressRemover();

            _selectGameModeArgsService.GameModeSelected += OnSelectedGameModeArgs;

            _objectsUpdater.Add(_mainMenuPlayerInputs);

            _disposables = new List<IDisposable>
            {
                _selectGameModeArgsService,
                _playerProgressPrinter,
                _playerProgressRemover
            };

            yield return null;
        }

        public override void Run()
        {
            _isRun = true;

            Debug.Log($"Выбрать режим игры: {KeyboardInputKeys.LoadNumbersModeKey} - сгенерировать цифры, " +
                      $"{KeyboardInputKeys.LoadCharactersModeKey} - сгенерировать буквы, " +
                      $"{KeyboardInputKeys.ShowInfoKey} - показать прогресс.");

            Debug.Log($"{KeyboardInputKeys.ResetProgressKey} - Сбросить прогресс за {_levelConfig.ResetPrice} золота.");
        }

        private void Update()
        {
            if (_isRun == false)
                return;

            _objectsUpdater.Update(Time.deltaTime);
        }

        private void OnDestroy()
        {
            _selectGameModeArgsService.GameModeSelected -= OnSelectedGameModeArgs;

            foreach (var disposable in _disposables)
                disposable.Dispose();
        }

        private void OnSelectedGameModeArgs(GameplayInputArgs args)
        {
            SceneSwitcherService sceneSwitcherService = _projectServicesFactory.GetSceneSwitcherService();
            ICoroutinesPerformer coroutinesPerformer = _projectServicesFactory.GetCoroutinesPerformer();
            coroutinesPerformer.StartPerform(sceneSwitcherService.ProcessSwitchTo(Scenes.Gameplay, args));
        }
    }
}
