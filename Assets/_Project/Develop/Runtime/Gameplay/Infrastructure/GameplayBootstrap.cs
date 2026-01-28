using System;
using System.Collections;
using _Project.Develop.Runtime.Gameplay.Inputs;
using _Project.Develop.Runtime.Infrastructure;
using _Project.Develop.Runtime.Infrastructure.DI;
using _Project.Develop.Runtime.Utilities.CoroutinesManagement;
using _Project.Develop.Runtime.Utilities.Factories;
using _Project.Develop.Runtime.Utilities.ObjectsLifetimeManagement;
using _Project.Develop.Runtime.Utilities.SceneManagement;
using UnityEngine;

namespace _Project.Develop.Runtime.Gameplay.Infrastructure
{
    public class GameplayBootstrap : SceneBootstrap
    {
        private DIContainer _container;
        private GameplayInputArgs _inputArgs;
        private ObjectsUpdater _objectsUpdater;
        private ICoroutinesPerformer _coroutinesPerformer;
        private GameCycle _gameCycle;
        private bool _isRun;

        public override void ProcessRegistration(DIContainer container, IInputSceneArgs sceneArgs = null)
        {
            _container = container;

            if (sceneArgs is not GameplayInputArgs gameplayInputArgs)
                throw new ArgumentException($"{nameof(sceneArgs)} is not match with {typeof(GameplayInputArgs)}");

            _inputArgs = gameplayInputArgs;

            GameplayContextRegistrations.Process(_container, _inputArgs);
        }

        public override IEnumerator Initialize()
        {
            _coroutinesPerformer = _container.Resolve<ICoroutinesPerformer>();
            _objectsUpdater = _container.Resolve<ObjectsUpdater>();

            GameplayPlayerInputs gameplayPlayerInputs = _container.Resolve<GameplayPlayerInputs>();
            _objectsUpdater.Add(gameplayPlayerInputs);
            
            GameCycleFactory gameCycleFactory = _container.Resolve<GameCycleFactory>();
            _gameCycle = gameCycleFactory.CreateGameCycle(_inputArgs);

            yield return null;
        }

        private void Update()
        {
            if (_isRun == false)
                return;

            _objectsUpdater.Update(Time.deltaTime);
        }

        private void OnDestroy()
        {
            _gameCycle.Dispose();
        }

        public override void Run()
        {
            _isRun = true;
            _coroutinesPerformer.StartPerform(_gameCycle.Start());
        }
    }
}
