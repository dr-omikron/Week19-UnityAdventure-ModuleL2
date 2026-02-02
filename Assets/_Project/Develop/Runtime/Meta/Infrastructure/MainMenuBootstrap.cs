using System.Collections;
using _Project.Develop.Runtime.Infrastructure;
using _Project.Develop.Runtime.Infrastructure.DI;
using _Project.Develop.Runtime.Utilities.Factories;
using _Project.Develop.Runtime.Utilities.ObjectsLifetimeManagement;
using _Project.Develop.Runtime.Utilities.SceneManagement;
using UnityEngine;

namespace _Project.Develop.Runtime.Meta.Infrastructure
{
    public class MainMenuBootstrap : SceneBootstrap
    {
        private DIContainer _container;
        private ObjectsUpdater _objectsUpdater;
        private MainMenuPlayerInputs _mainMenuPlayerInputs;
        private MetaCycle _metaCycle;
        private bool _isRun;

        public override void ProcessRegistration(DIContainer container, IInputSceneArgs sceneArgs = null)
        {
            _container = container;
            MainMenuContextRegistrations.Process(_container);
        }

        public override IEnumerator Initialize()
        {

            _objectsUpdater = _container.Resolve<ObjectsUpdater>();
            _mainMenuPlayerInputs = _container.Resolve<MainMenuPlayerInputs>();

            _objectsUpdater.Add(_mainMenuPlayerInputs);

            MetaCycleFactory factory = _container.Resolve<MetaCycleFactory>();
            _metaCycle = factory.Create();

            yield return null;
        }

        public override void Run()
        {
            _isRun = true;
            _metaCycle.Start();
        }

        private void Update()
        {
            if (_isRun == false)
                return;

            _objectsUpdater.Update(Time.deltaTime);
        }

        private void OnDestroy()
        {
            _metaCycle.Deinitialize();
        }
    }
}
