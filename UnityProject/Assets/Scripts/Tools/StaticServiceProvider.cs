using System;
using System.Collections.Generic;
using UnityEngine;

namespace Clovers.Tools
{
    public static class StaticServiceProvider
    {
        private static readonly Dictionary<Type, object> _context = new();
        
        public static T Get<T>() where T : class
        {
            return _context[typeof(T)] as T;
        }

        public static void Set<T>(T instance) where T : class
        {
            var type = typeof(T);
            if (_context.ContainsKey(type))
            {
                Debug.LogWarning($"Replacing existing service: {type}");
            }
            _context[typeof(T)] = instance;
        }
        
        public static void Dispose()
        {
            _context.Clear();
        }
    }
}