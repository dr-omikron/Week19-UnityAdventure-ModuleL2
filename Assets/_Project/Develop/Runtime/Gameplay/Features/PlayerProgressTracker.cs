using _Project.Develop.Runtime.Utilities.DataManagement;
using _Project.Develop.Runtime.Utilities.DataManagement.DataProviders;

namespace _Project.Develop.Runtime.Gameplay.Features
{
    public class PlayerProgressTracker : IDataReader<PlayerStatistic>, IDataWriter<PlayerStatistic>
    {
        public PlayerProgressTracker(PlayerStatisticProvider playerStatisticProvider)
        {
            playerStatisticProvider.RegisterReader(this);
            playerStatisticProvider.RegisterWriter(this);
        }
        public int Wins { get; private set; }
        public int Losses { get; private set; }

        public void AddWin() => Wins++;
        public void AddLoss() => Losses++;

        public void ReadFrom(PlayerStatistic data)
        {
            Wins = data.Wins;
            Losses = data.Losses;
        }

        public void WriteTo(PlayerStatistic data)
        {
            data.Wins = Wins;
            data.Losses = Losses;
        }
    }
}
