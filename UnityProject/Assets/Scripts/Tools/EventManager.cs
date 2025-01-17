using System;
using UnityEngine;

namespace Clovers.Tools
{
    public class EventManager
    {
        public Action<EventIdentifier, object> OnEvent;
        
        public Action AddListener<T>(EventIdentifier identifier, Action<T> listener)
        {
            void FilteredListener(EventIdentifier incomingIdentifier, object message)
            {
                if (identifier != incomingIdentifier)
                {
                    return;
                }
                listener?.Invoke((T)message);
            }
            
            OnEvent += FilteredListener;
            return () => OnEvent -= FilteredListener;
        }
        
        public void SendEvent(EventIdentifier identifier, object parameters)
        {
            Debug.Log($"[EventManager] Send {identifier.Id}");
            OnEvent?.Invoke(identifier, parameters);
        }
    }
}