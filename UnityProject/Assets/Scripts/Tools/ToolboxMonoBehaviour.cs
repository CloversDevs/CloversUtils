using System;
using System.Collections.Generic;
using UnityEngine;

namespace Clovers.Tools
{
    public abstract class ToolboxMonoBehaviour : MonoBehaviour
    {
        private bool _hasStarted;
        protected event Action OnCleanup;
        
        protected virtual void OnDisable()
        {
            OnCleanup?.Invoke();
            OnCleanup = null;
        }
        
        protected abstract void OnReady();
        
        protected virtual void Start()
        {
            _hasStarted = true;
            OnReady();
        }

        protected virtual void OnEnable()
        {
            if(!_hasStarted) return;
            OnReady();
        }
    }
}