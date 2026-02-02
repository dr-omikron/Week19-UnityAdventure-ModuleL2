using _Project.Develop.Runtime.Infrastructure.DI;
using _Project.Develop.Runtime.Meta.Features;
using _Project.Develop.Runtime.Meta.Infrastructure;
using _Project.Develop.Runtime.Utilities.ConfigsManagement;
using _Project.Develop.Runtime.Utilities.CoroutinesManagement;
using _Project.Develop.Runtime.Utilities.SceneManagement;

namespace _Project.Develop.Runtime.Utilities.Factories
{
    public class MetaCycleFactory
    {
        private readonly DIContainer _container;

        public MetaCycleFactory(DIContainer container)
        {
            _container = container;
        }

        public MetaCycle Create()
        {
            SelectGameModeArgsService selectGameModeArgsService = _container.Resolve<SelectGameModeArgsService>();
            PlayerProgressPrinter playerProgressPrinter = _container.Resolve<PlayerProgressPrinter>();
            PlayerProgressRemover playerProgressRemover = _container.Resolve<PlayerProgressRemover>();
            ConfigsProviderService configsProviderService = _container.Resolve<ConfigsProviderService>();
            SceneSwitcherService sceneSwitcherService = _container.Resolve<SceneSwitcherService>();
            ICoroutinesPerformer  coroutinesPerformer = _container.Resolve<ICoroutinesPerformer>();

            return new MetaCycle(
                selectGameModeArgsService,
                playerProgressPrinter,
                playerProgressRemover,
                configsProviderService,
                sceneSwitcherService,
                coroutinesPerformer);
        }
    }
}