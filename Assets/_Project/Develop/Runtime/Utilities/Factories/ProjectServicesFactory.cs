using _Project.Develop.Runtime.Gameplay.Services;
using _Project.Develop.Runtime.Infrastructure.DI;
using _Project.Develop.Runtime.Meta.Features;
using _Project.Develop.Runtime.Utilities.AssetsManagement;
using _Project.Develop.Runtime.Utilities.ConfigsManagement;
using _Project.Develop.Runtime.Utilities.CoroutinesManagement;
using _Project.Develop.Runtime.Utilities.LoadScreen;
using _Project.Develop.Runtime.Utilities.ObjectsLifetimeManagement;
using _Project.Develop.Runtime.Utilities.SceneManagement;

namespace _Project.Develop.Runtime.Utilities.Factories
{
    public class ProjectServicesFactory
    {
        private readonly DIContainer _container;

        public ProjectServicesFactory(DIContainer container)
        {
            _container = container;
        }

        public ICoroutinesPerformer GetCoroutinesPerformer() => _container.Resolve<ICoroutinesPerformer>();
        public ConfigsProviderService GetConfigsProviderService() => _container.Resolve<ConfigsProviderService>();
        public ResourcesAssetsLoader GetResourcesAssetsLoader() => _container.Resolve<ResourcesAssetsLoader>();
        public SceneLoaderService GetSceneLoaderService() => _container.Resolve<SceneLoaderService>();
        public SceneSwitcherService GetSceneSwitcherService() => _container.Resolve<SceneSwitcherService>();
        public ILoadingScreen GetStandardLoadingScreen() => _container.Resolve<ILoadingScreen>();
        public ObjectsUpdater GetObjectsUpdater() => _container.Resolve<ObjectsUpdater>();
        public PlayerProgressTracker GetPlayerProgressTracker() => _container.Resolve<PlayerProgressTracker>();
        public WalletService GetWalletService() => _container.Resolve<WalletService>();

    }
}
