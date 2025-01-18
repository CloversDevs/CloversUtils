using System;
using System.Reflection;
using UnityEngine;

namespace Clovers.Tools
{
    public abstract class ObservableReference<T>
    {
        public Observable<T> Value
        {
            get
            {
                _value ??= GetObservable();
                return _value;
            }
        }

        private Observable<T> _value;
        [Tooltip("The component (e.g. SimpleTree, SimpleMine) that holds an Observable<int> field.")]
        public MonoBehaviour sourceComponent;

        [Tooltip("The name of the Observable<int> field in the source component.")]
        public string fieldName;

        /// <summary>
        /// Use reflection to retrieve the Observable<T> field on the sourceComponent by fieldName.
        /// Returns null if not found or if the cast fails.
        /// TODO: Can this grab readonly and / or non-serializable fields?
        /// </summary>
        public Observable<T> GetObservable()
        {
            if (sourceComponent == null || string.IsNullOrEmpty(fieldName))
                return null;

            // Get the type of the component
            Type type = sourceComponent.GetType();

            // Get the field info via reflection (we look for instance fields, both public and non-public)
            FieldInfo field = type.GetField(fieldName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

            if (field == null)
            {
                Debug.LogWarning($"{fieldName} not found on {sourceComponent.name}");
                return null;
            }

            // Attempt to get the value as Observable<T>
            object fieldValue = field.GetValue(sourceComponent);
            if (fieldValue is Observable<T> observable)
            {
                return observable;
            }

            Debug.LogWarning($"{fieldName} on {sourceComponent.name} is not an Observable<{typeof(T)}>.");
            return null;
        }
    }
}