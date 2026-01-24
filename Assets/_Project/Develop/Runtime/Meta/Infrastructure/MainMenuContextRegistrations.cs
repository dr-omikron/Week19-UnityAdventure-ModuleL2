using _Project.Develop.Runtime.Gameplay.Services;
using _Project.Develop.Runtime.Infrastructure.DI;
using _Project.Develop.Runtime.Meta.Features;
using _Project.Develop.Runtime.Utilities.ConfigsManagement;

namespace _Project.Develop.Runtime.Meta.Infrastructure
{
    public class MainMenuContextRegistrations
    {
        public static void Process(DIContainer container)
        {

            container.RegisterAsSingle(CreateMainMenuPlayerInputs);
            container.RegisterAsSingle(CreateSelectGameModeService);
            container.RegisterAsSingle(CreatePlayerProgressPrinter);
            container.RegisterAsSingle(CreatePlayerProgressRemover);
        }

        private static MainMenuPlayerInputs CreateMainMenuPlayerInputs(DIContainer c) => new MainMenuPlayerInputs();
        private static SelectGameModeArgsService CreateSelectGameModeService(DIContainer c)
        {
            MainMenuPlayerInputs mainMenuPlayerInputs = c.Resolve<MainMenuPlayerInputs>();
            ConfigsProviderService configsProviderService = c.Resolve<ConfigsProviderService>();

            return new SelectGameModeArgsService(mainMenuPlayerInputs, configsProviderService);
        }

        private static PlayerProgressPrinter CreatePlayerProgressPrinter(DIContainer c)
        {
            PlayerProgressTracker playerProgressTracker = c.Resolve<PlayerProgressTracker>();
            WalletService walletService = c.Resolve<WalletService>();
            MainMenuPlayerInputs mainMenuPlayerInputs = c.Resolve<MainMenuPlayerInputs>();

            return new PlayerProgressPrinter(playerProgressTracker, walletService, mainMenuPlayerInputs);
        }

        private static PlayerProgressRemover CreatePlayerProgressRemover(DIContainer c)
        {
            PlayerProgressTracker playerProgressTracker = c.Resolve<PlayerProgressTracker>();
            WalletService walletService = c.Resolve<WalletService>();
            MainMenuPlayerInputs mainMenuPlayerInputs = c.Resolve<MainMenuPlayerInputs>();
            ConfigsProviderService configsProviderService = c.Resolve<ConfigsProviderService>();

            return new PlayerProgressRemover(playerProgressTracker, walletService, mainMenuPlayerInputs, configsProviderService);
        }
    }
}
