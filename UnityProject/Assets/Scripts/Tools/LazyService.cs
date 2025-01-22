using UnityEngine;

namespace Clovers.Tools
{
    public class LazyService<T> where T : class
    {
        private T _value;
        public object Context { get; private set; }

        public T GetValue(MonoBehaviour context)
        {
            _value ??= StaticServiceProvider.Get<T>(context);
            Context = context;
            return _value;
        }
    }
}