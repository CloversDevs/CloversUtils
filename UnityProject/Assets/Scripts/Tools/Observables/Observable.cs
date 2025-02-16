using System;
using UnityEngine;

namespace Clovers.Tools
{
    [Serializable]
    public class Observable<T> : IReadOnlyObservable<T>
    {
        public event Action<T> OnChange;

        public T Value
        {
            get => _value;
            set
            {
                _value = value;
                OnChange?.Invoke(_value);
            }
        }
        
        [SerializeField]
        private T _value;
        
        public Observable(T value = default)
        {
            _value = value;
        }

        public Action Track(Action<T> listener)
        {
            OnChange += listener;
            listener.Invoke(_value);
            
            return () => OnChange -= listener;
        }
        
#if UNITY_EDITOR
        /// <summary>
        /// Invoked when the value is modified on the inspector at runtime.
        /// </summary>
        public void ForceNotifyChange()
        {
            OnChange?.Invoke(_value);
        }
#endif
    }
}
