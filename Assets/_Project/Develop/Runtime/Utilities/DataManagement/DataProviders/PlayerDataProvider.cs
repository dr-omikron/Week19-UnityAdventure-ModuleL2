namespace _Project.Develop.Runtime.Utilities.DataManagement.DataProviders
{
    public class PlayerDataProvider : DataProvider<PlayerData>
    {
        private readonly int _defaultGoldAmount;

        public PlayerDataProvider(ISaveLoadService saveLoadService, int defaultGoldAmount) : base(saveLoadService)
        {
            _defaultGoldAmount = defaultGoldAmount;
        }

        protected override PlayerData GetOriginData()
        {
            return new PlayerData()
            {
                Gold = _defaultGoldAmount,
                Wins = 0,
                Losses = 0
            };
        }
    }
}
