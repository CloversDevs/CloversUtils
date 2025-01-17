using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace Clovers.Tools.Editor
{
    /// <summary>
    /// Find and display the inner list value and expose it to the Editor with their default inspector.
    /// </summary>
    [CustomPropertyDrawer(typeof(ObservableList<>))]
    public class ObservableListPropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginChangeCheck();

            // Draw the default inspector for the _value
            SerializedProperty valueProperty = property.FindPropertyRelative("_value");
            EditorGUI.PropertyField(position, valueProperty, label, true);

            // If the value has changed on the inspector, find and invoke ForceNotifyChange.
            // This is done for letting listeners know of the change if it happens during runtime.
            if (!EditorGUI.EndChangeCheck()) return;
            
            property.serializedObject.ApplyModifiedProperties();

            // Get the Observable<T> object instance
            var instance = fieldInfo.GetValue(property.serializedObject.targetObject);
            if (instance == null) return;
            
            var forceNotifyMethod = instance
                .GetType()
                .GetMethod("ForceNotifyChange", BindingFlags.Public | BindingFlags.Instance);

            if (forceNotifyMethod == null) return;
            forceNotifyMethod.Invoke(instance, null);
        }
        
        /// <summary>
        /// Make sure the property drawer has the correct size for the list <T> type.
        /// </summary>
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            var valueProperty = property.FindPropertyRelative("_value");

            return valueProperty != null
                ? EditorGUI.GetPropertyHeight(valueProperty, true)
                : base.GetPropertyHeight(property, label);
        }
    }
}