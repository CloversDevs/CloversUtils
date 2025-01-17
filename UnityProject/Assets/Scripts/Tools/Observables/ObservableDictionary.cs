using System;
using System.Collections.Generic;

namespace Clovers.Tools
{
    public interface IReadOnlyObservableDictionary<T, TY>
    {
        event Action<IReadOnlyDictionary<T,TY>> OnChange;
        IReadOnlyDictionary<T, TY> Value { get; set; }
        Action Track(Action<IReadOnlyDictionary<T, TY>> listener);
        TY this[T index] { get; set; }
    }
    
    // TODO: Think of the implications of TY having Observable values.
    // What rules to put in place to make sure that?
    public class ObservableDictionary<T, TY> : IReadOnlyObservableDictionary<T, TY>
    {
        public event Action<IReadOnlyDictionary<T,TY>> OnChange;
        public event Action<T,TY> OnAddElement;
        public event Action<T> OnRemoveElement;
        public event Action<T, TY> OnReplacedElement;
        
        public IReadOnlyDictionary<T, TY> Value
        {
            get => _value;
            set
            {
                var addedKeys = new List<T>();
                var replacedKeys = new List<T>();
                var removedKeys = new List<T>();
                foreach (var kvp in value)
                {
                    if (_value.ContainsKey(kvp.Key))
                    {
                        replacedKeys.Add(kvp.Key);
                        continue;
                    }
                    addedKeys.Add(kvp.Key);
                }
                
                foreach (var kvp in _value)
                {
                    if (!value.ContainsKey(kvp.Key))
                    {
                        removedKeys.Add(kvp.Key);
                    }
                }
                
                _value.Clear();
                foreach (var kvp in value)
                {
                    _value[kvp.Key] = kvp.Value;
                }

                foreach (var added in addedKeys)
                {
                    OnAddElement?.Invoke(added, _value[added]);
                }
                foreach (var removed in removedKeys)
                {
                    OnRemoveElement?.Invoke(removed);
                }
                foreach (var replaced in replacedKeys)
                {
                    OnReplacedElement?.Invoke(replaced, _value[replaced]);
                }
                OnChange?.Invoke(_value);
            }
        }

        private Dictionary<T, TY> _value;

        public Action Track(Action<IReadOnlyDictionary<T, TY>> listener)
        {
            OnChange += listener;
            listener?.Invoke(_value);
            return () => OnChange -= listener;
        }
        
        public virtual TY this[T index]
        {
            get => _value[index];
            set
            {
                var added = !_value.ContainsKey(index);
                _value[index] = value;
                if (added)
                {
                    OnAddElement?.Invoke(index, value);
                }
                else
                {
                    OnReplacedElement?.Invoke(index, value);
                }
                OnChange?.Invoke(_value);
            }
        }
    }
}