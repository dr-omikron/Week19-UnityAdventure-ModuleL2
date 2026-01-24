using System;

namespace _Project.Develop.Runtime.Gameplay.Services
{
    public class PlayerProgressTracker
    {
        public int Wins { get; private set; }
        public int Losses { get; private set; }

        public void AddWin() => Wins++;
        public void AddLoss() => Losses++;

        public void ResetProgress()
        {
            Wins = 0;
            Losses = 0;
        }

        public bool IsNotZeroProgress() => Losses != 0 || Wins != 0;

        public void SetProgress(int wins, int losses)
        {
            if(wins < 0)
                throw new ArgumentOutOfRangeException(nameof(wins), "The progress cannot be negative");

            if(losses < 0)
                throw new ArgumentOutOfRangeException(nameof(losses), "The progress cannot be negative");

            Wins = wins;
            Losses = losses;
        }
    }
}
