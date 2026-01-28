using _Project.Develop.Runtime.Gameplay.Configs;
using _Project.Develop.Runtime.Gameplay.Features;
using _Project.Develop.Runtime.Gameplay.Infrastructure;
using _Project.Develop.Runtime.Gameplay.Inputs;
using _Project.Develop.Runtime.Infrastructure.DI;
using _Project.Develop.Runtime.Meta.Features;
using _Project.Develop.Runtime.Utilities.ConfigsManagement;
using _Project.Develop.Runtime.Utilities.CoroutinesManagement;
using _Project.Develop.Runtime.Utilities.SceneManagement;

namespace _Project.Develop.Runtime.Utilities.Factories
{
    public class GameCycleFactory
    {
        private readonly DIContainer _container;

        public GameCycleFactory(DIContainer container)
        {
            _container = container;
        }

        public GameCycle CreateGameCycle(GameplayInputArgs gameplayInputArgs)
        {
            PlayerProgressTracker playerProgressTracker = _container.Resolve<PlayerProgressTracker>();
            LevelConfig levelConfig = _container.Resolve<ConfigsProviderService>().GetConfig<LevelConfig>();
            WalletService walletService = _container.Resolve<WalletService>();
            SymbolsSequenceGenerator symbolsSequenceGenerator = _container.Resolve<SymbolsSequenceGenerator>();
            InputStringReader inputStringReader = _container.Resolve<InputStringReader>();
            SceneSwitcherService sceneSwitcherService = _container.Resolve<SceneSwitcherService>();
            GameplayPlayerInputs gameplayPlayerInputs = _container.Resolve<GameplayPlayerInputs>();
            ICoroutinesPerformer coroutinesPerformer = _container.Resolve<ICoroutinesPerformer>();

            return new GameCycle(
                playerProgressTracker,
                levelConfig,
                walletService,
                symbolsSequenceGenerator,
                inputStringReader,
                sceneSwitcherService,
                gameplayPlayerInputs,
                coroutinesPerformer,
                gameplayInputArgs);
        }
    }
}