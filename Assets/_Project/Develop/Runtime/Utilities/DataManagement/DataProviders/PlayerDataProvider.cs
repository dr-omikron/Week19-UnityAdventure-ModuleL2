using _Project.Develop.Runtime.Meta.Configs;

namespace _Project.Develop.Runtime.Utilities.DataManagement.DataProviders
{
    public class PlayerDataProvider : DataProvider<PlayerData>
    {
        private readonly StartPlayerDataConfig _startPlayerData;

        public PlayerDataProvider(ISaveLoadService saveLoadService, StartPlayerDataConfig startPlayerData) : base(saveLoadService)
        {
            _startPlayerData = startPlayerData;
        }

        protected override PlayerData GetOriginData()
        {
            return new PlayerData()
            {
                Gold = _startPlayerData.DefaultGoldAmount,
                Wins = 0,
                Losses = 0
            };
        }
    }
}
