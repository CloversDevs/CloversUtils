using System;
using System.Collections.Generic;
using UnityEngine;

namespace Clovers.Tools
{
    public class StaticServiceProvider
    {
        private static StaticServiceProvider _instance;
        private readonly Dictionary<Type, object> _context = new();
        
        public static T Get<T>() where T : class
        {
            return _instance._context[typeof(T)] as T;
        }
        
        public StaticServiceProvider()
        {
            if (_instance != null)
            {
                Debug.LogError("Overwriting service provider!");
                _instance.Dispose();
            }
            _instance = this;
        }

        public static void Set<T>(T instance) where T : class
        {
            var type = typeof(T);
            if (_instance._context.ContainsKey(type))
            {
                Debug.LogWarning($"Replacing existing service: {type}");
            }
            _instance._context[typeof(T)] = instance;
        }
        
        public void Dispose()
        {
            _context.Clear();
            _instance = null;
        }
    }
}