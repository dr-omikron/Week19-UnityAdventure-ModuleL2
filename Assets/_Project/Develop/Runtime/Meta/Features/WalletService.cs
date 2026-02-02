using System;
using _Project.Develop.Runtime.Utilities.DataManagement;
using _Project.Develop.Runtime.Utilities.DataManagement.DataProviders;
using _Project.Develop.Runtime.Utilities.Reactive;

namespace _Project.Develop.Runtime.Meta.Features
{
    public class WalletService : IDataReader<PlayerCurrency>, IDataWriter<PlayerCurrency>
    {
        private readonly ReactiveVariable<int> _gold;

        public WalletService(ReactiveVariable<int> gold, PlayerCurrencyProvider playerCurrencyProvider)
        {
            _gold = gold;
            playerCurrencyProvider.RegisterReader(this);
            playerCurrencyProvider.RegisterWriter(this);
        }

        public IReadOnlyVariable<int> Gold => _gold;

        public bool Enough(int amount)
        {
            if(amount < 0)
                throw new ArgumentOutOfRangeException(nameof(amount));

            return amount <= _gold.Value;
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

        public void Reset() => _gold.Value = 0;

        public void ReadFrom(PlayerCurrency currency) => _gold.Value = currency.Gold;

        public void WriteTo(PlayerCurrency currency) => currency.Gold = _gold.Value;
    }
}
