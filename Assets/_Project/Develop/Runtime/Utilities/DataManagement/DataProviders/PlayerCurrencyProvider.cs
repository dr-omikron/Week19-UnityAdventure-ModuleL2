using _Project.Develop.Runtime.Meta.Configs;

namespace _Project.Develop.Runtime.Utilities.DataManagement.DataProviders
{
    public class PlayerCurrencyProvider : DataProvider<PlayerCurrency>
    {
        private readonly StartPlayerDataConfig _startPlayerData;

        public PlayerCurrencyProvider(ISaveLoadService saveLoadService, StartPlayerDataConfig startPlayerData) : base(saveLoadService)
        {
            _startPlayerData = startPlayerData;
        }

        protected override PlayerCurrency GetOriginData()
        {
            return new PlayerCurrency
            {
                Gold = _startPlayerData.DefaultGoldAmount,
            };
        }
    }
}
