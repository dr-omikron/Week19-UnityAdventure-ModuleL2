namespace _Project.Develop.Runtime.Utilities.DataManagement.DataProviders
{
    public class PlayerStatisticProvider : DataProvider<PlayerStatistic>
    {
        public PlayerStatisticProvider(ISaveLoadService saveLoadService) : base(saveLoadService)
        {
        }

        protected override PlayerStatistic GetOriginData()
        {
            return new PlayerStatistic
            {
                Wins = 0,
                Losses = 0,
            };
        }

        public bool IsNotZeroProgress() => Data.Wins != 0 || Data.Losses != 0;
    }
}
