namespace Clovers.Tools
{
    public class LazyService<T> where T : class
    {
        public T Value
        {
            get
            {
                _value ??= StaticServiceProvider.Get<T>();
                return _value;
            }
        }
        
        private T _value;
    }
}