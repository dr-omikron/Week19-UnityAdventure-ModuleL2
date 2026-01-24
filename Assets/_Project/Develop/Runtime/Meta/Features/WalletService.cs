using System;
using _Archero.Develop.Runtime.Utilities.Reactive;

namespace _Project.Develop.Runtime.Meta.Features
{
    public class WalletService
    {
        private readonly ReactiveVariable<int> _gold;

        public WalletService(ReactiveVariable<int> gold)
        {
            _gold = gold;
        }

        public IReadOnlyVariable<int> Gold => _gold;

        public bool Enough(int amount)
        {
            if(amount < 0)
                throw new ArgumentOutOfRangeException(nameof(amount));

            return amount >= _gold.Value;
        }

        public void Add(int amount)
        {
            if(amount < 0)
                throw new ArgumentOutOfRangeException(nameof(amount));

            _gold.Value += amount;
        }

        public void Spend(int amount)
        {
            if(Enough(amount) == false)
                throw new InvalidOperationException("Not enough gold");

            if(amount < 0)
                throw new ArgumentOutOfRangeException(nameof(amount));

            _gold.Value -= amount;
        }
    }
}
