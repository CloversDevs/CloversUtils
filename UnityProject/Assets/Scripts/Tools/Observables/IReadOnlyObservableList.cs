using System;
using System.Collections.Generic;

namespace Clovers.Tools
{
    public interface IReadOnlyObservableList<out T>
    {
        event Action<IReadOnlyList<T>> OnChange;
        IReadOnlyList<T> Value { get; }
        Action Track(Action<IReadOnlyList<T>> listener);
    }
}