using System;
using System.Collections.Generic;
using UnityEngine;
using Clovers.Tools;

namespace Clovers.Tools
{
    public abstract class OrderedListInstantiator<T, TY> : ToolboxMonoBehaviour where TY : MonoBehaviour
    {
        [SerializeField]
        private TY _prefab;
        
        private readonly Dictionary<T, TY> _instances = new();
        protected override void OnReady()
        {
            OnCleanup += GetOrderedList().Track(OnNamesChangedOrdered);
            OnCleanup += ReturnAllInstances;
        }

        private void ReturnAllInstances()
        {
            foreach (var instance in _instances.Values)
            {
                ReturnInstance(instance);
            }
            _instances.Clear();
        }

        private void OnNamesChangedOrdered(IReadOnlyList<ListUpdate<T>> listUpdate)
        {
            var newElements = new List<ListElementAdd<T>>();
            var movedElements = new List<ListElementMove<T>>();
            
            foreach (var update in listUpdate)
            {
                switch (update)
                {
                    case ListElementAdd<T> addUpdate:
                        var instance = GetInstance();
                        newElements.Add(addUpdate);
                        _instances[update.Value] = instance;
                        OnInstantiate(instance, update.Value);
                        break;
                    case ListElementMove<T> moveUpdate:
                        movedElements.Add(moveUpdate);
                        break;
                    case ListElementRemove<T>:
                        ReturnInstance(_instances[update.Value]);
                        _instances.Remove(update.Value);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            
            // Only move elements after all elements have been added / removed.
            foreach (var update in newElements)
            {
                _instances[update.Value].transform.SetSiblingIndex(update.Index);
            }
            foreach (var update in movedElements)
            {
                _instances[update.Value].transform.SetSiblingIndex(update.NewIndex);
            }
        }

        protected abstract ObservableOrderedList<T> GetOrderedList();

        protected virtual void OnInstantiate(TY instance, T value)
        {
            //
        }
        
        protected virtual TY GetInstance()
        {
            // TODO: Pooling.
            return Instantiate(_prefab, transform);
        }

        protected virtual void ReturnInstance(TY instance)
        {
            // TODO: Pooling.
            Destroy(instance.gameObject);
        }
    }
}