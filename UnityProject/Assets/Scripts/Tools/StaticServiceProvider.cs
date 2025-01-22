using System;
using System.Collections.Generic;
using UnityEngine;

namespace Clovers.Tools
{
    public class ServiceProvider
    {
        private readonly Dictionary<Type, object> _context = new();
        public T Get<T>() where T : class
        {
            return _context[typeof(T)] as T;
        }

        public void Set<T>(T instance) where T : class
        {
            var type = typeof(T);
            if (_context.ContainsKey(type))
            {
                Debug.LogWarning($"Replacing existing service: {type}");
            }
            _context[typeof(T)] = instance;
        }
        
        public void Dispose()
        {
            _context.Clear();
        }
    }

    public static class StaticServiceProvider
    {
        private static readonly List<Transform> _contextSorted = new();
        private static readonly Dictionary<Transform, ServiceProvider> _context = new();
        public static T Get<T>(MonoBehaviour context) where T : class
        {
            foreach (var serviceProviderRoot in _contextSorted)
            {
                if(!context.transform.IsChildOf(serviceProviderRoot)) continue;
                return _context[serviceProviderRoot].Get<T>();
            }
            return null;
        }
        
        public static void Set<T>(MonoBehaviour context, T service) where T : class
        {
            var transform = context.transform;
            if (!_context.TryGetValue(transform, out var root))
            {
                _context[transform] = new();
                
                // HACK: Sorted in inverse order of appearance to hopefully catch hierarchies in order.
                // Makes it the user's responsibility to avoid the incorrect ServiceProvider being picked when nested.
                _contextSorted.Insert(0, transform);
            }
            _context[context.transform].Set(service);
        }
    }
}