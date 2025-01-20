using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Clovers.Tools
{
    [Serializable]
    public class ObservableList<T> : IReadOnlyObservableList<T>
    {
        public event Action<IReadOnlyList<T>> OnChange;
        public IReadOnlyList<T> Value => _value;

        [SerializeField]
        private List<T> _value = new();

        public ObservableList(List<T> value = default)
        {
            if (value == null)
            {
                return;
            }
            _value.AddRange(value);
        }
        
        public void Set(List<T> value)
        {
            _value.Clear();
            _value.AddRange(value);
                
            OnChange?.Invoke(_value);
        }

        public void Add(T element)
        {
            _value.Add(element);
            OnChange?.Invoke(_value);
        }
        
        public void Remove(T element)
        {
            _value.Remove(element);
            OnChange?.Invoke(_value);
        }

        public void RemoveAt(int index)
        {
            _value.RemoveAt(index);
            OnChange?.Invoke(_value);
        }

        public T FirstOrDefault(Func<T, bool> search)
        {
            return _value.FirstOrDefault(search);
        }
        
        public List<T> FindAll(Predicate<T> search)
        {
            return _value.FindAll(search);
        }

        public Action Track(Action<IReadOnlyList<T>> listener)
        {
            OnChange += listener;
            listener?.Invoke(_value);
            return ()=> OnChange -= listener;
        }
    }
}