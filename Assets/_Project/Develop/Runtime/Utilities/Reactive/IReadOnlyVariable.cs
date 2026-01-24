using System;

namespace _Archero.Develop.Runtime.Utilities.Reactive
{
    public interface IReadOnlyVariable<T>
    {
        IDisposable Subscribe(Action<T, T> action);
        public T Value { get; }
    }
}
