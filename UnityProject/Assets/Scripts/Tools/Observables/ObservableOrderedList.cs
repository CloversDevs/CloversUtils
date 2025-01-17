using System;
using System.Collections.Generic;

namespace Clovers.Tools
{
    
    public abstract class ListUpdate<T>
    {
        public readonly T Value;

        public ListUpdate(T value)
        {
            Value = value;
        }
    }
    
    public class ListElementRemove<T> : ListUpdate<T>
    {
        public ListElementRemove(T value) : base(value)
        {
            //
        }
    }
    
    public class ListElementMove<T> : ListUpdate<T>
    {
        public readonly int NewIndex;

        public ListElementMove(T value, int newIndex) : base(value)
        {
            NewIndex = newIndex;
        }
    }
    
    public class ListElementAdd<T> : ListUpdate<T>
    {
        public readonly int Index;

        public ListElementAdd(T value, int index) : base(value)
        {
            Index = index;
        }
    }
    
    public class ObservableOrderedList<T>
    {
        public event Action<IReadOnlyList<ListUpdate<T>>> OnUpdate;

        public IReadOnlyList<T> Value
        {
            get => _value;
            set
            {
                var update = Wipe();
                _value.AddRange(value);
                update.AddRange(GenerateCompleteUpdate());
                OnUpdate?.Invoke(update);
            }
        }

        private readonly List<T> _value = new();

        public ObservableOrderedList(List<T> value = default)
        {
            if (value == null)
            {
                return;
            }
            _value.AddRange(value);
        }

        public void Add(T element)
        {
            _value.Add(element);
            OnUpdate?.Invoke(new List<ListUpdate<T>>()
            {
                new ListElementAdd<T>(element, _value.Count - 1)
            });
        }

        public void Remove(Predicate<T> condition)
        {
            var found = _value.FindAll(condition);
            var update = new List<ListUpdate<T>>();
            foreach (var element in found)
            {
                _value.Remove(element);
                update.Add(new ListElementRemove<T>(element));
            }
            OnUpdate?.Invoke(update);
        }
        
        public void Remove(T element)
        {
            _value.Remove(element);
            OnUpdate?.Invoke(new List<ListUpdate<T>>()
            {
                new ListElementRemove<T>(element)
            });
        }

        public void RemoveAt(int index)
        {
            var element = _value[index];
            _value.RemoveAt(index);
            OnUpdate?.Invoke(new List<ListUpdate<T>>()
            {
                new ListElementRemove<T>(element)
            });
        }
        
        public Action Track(Action<IReadOnlyList<ListUpdate<T>>> listener)
        {
            OnUpdate += listener;
            listener?.Invoke(GenerateCompleteUpdate());
            return ()=> OnUpdate -= listener;
        }

        private List<ListUpdate<T>> GenerateCompleteUpdate()
        {
            var update = new List<ListUpdate<T>>();
            for (var i = 0; i < _value.Count; i++)
            {
                var element = _value[i];
                update.Add(
                    new ListElementAdd<T>(element, i)
                );
            }
            return update;
        }
        
        private List<ListUpdate<T>> Wipe()
        {
            var update = new List<ListUpdate<T>>();
            for (var i = 0; i < _value.Count; i++)
            {
                var element = _value[i];
                update.Add(
                    new ListElementRemove<T>(element)
                );
            }
            _value.Clear();
            return update;
        }
    }
}