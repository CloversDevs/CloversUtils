using System;

namespace Clovers.Tools
{
    public interface IReadOnlyObservable<out T>
    {
        event Action<T> OnChange;
        T Value { get; }
        Action Track(Action<T> listener);
    }
}