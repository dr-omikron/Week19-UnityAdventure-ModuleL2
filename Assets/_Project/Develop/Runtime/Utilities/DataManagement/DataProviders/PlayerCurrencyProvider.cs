using _Project.Develop.Runtime.Meta.Configs;
using _Project.Develop.Runtime.Utilities.ConfigsManagement;

namespace _Project.Develop.Runtime.Utilities.DataManagement.DataProviders
{
    public class PlayerCurrencyProvider : DataProvider<PlayerCurrency>
    {
        private readonly ConfigsProviderService _configsProviderService;

        public PlayerCurrencyProvider(ISaveLoadService saveLoadService, ConfigsProviderService configsProviderService) : base(saveLoadService)
        {
            _configsProviderService = configsProviderService;
        }

        protected override PlayerCurrency GetOriginData()
        {
            StartPlayerDataConfig startPlayerDataConfig = _configsProviderService.GetConfig<StartPlayerDataConfig>();

            return new PlayerCurrency
            {
                Gold = startPlayerDataConfig.DefaultGoldAmount
            };
        }
    }
}
