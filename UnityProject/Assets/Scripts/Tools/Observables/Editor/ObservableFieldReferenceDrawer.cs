using UnityEditor;
using UnityEngine;
using System;
using System.Linq;
using System.Reflection;

namespace Clovers.Tools.Editor
{
    public abstract class ObservableFieldReferenceDrawer<T> : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            // We'll show the property label as a foldout
            EditorGUI.BeginProperty(position, label, property);

            // 1. Draw the foldout
            var foldoutRect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
            property.isExpanded = EditorGUI.Foldout(foldoutRect, property.isExpanded, label, true);

            if (property.isExpanded)
            {
                EditorGUI.indentLevel++;
                float lineHeight = EditorGUIUtility.singleLineHeight;
                float spacing = EditorGUIUtility.standardVerticalSpacing;

                // 2. Draw the sourceComponent field
                var sourceCompProp = property.FindPropertyRelative("sourceComponent");
                var fieldNameProp  = property.FindPropertyRelative("fieldName");

                Rect sourceRect = new Rect(position.x, position.y + lineHeight + spacing,
                                           position.width, lineHeight);

                EditorGUI.PropertyField(sourceRect, sourceCompProp, new GUIContent("Source Component"));

                // 3. If source component is assigned, let’s gather all the Observable<T> fields
                MonoBehaviour sourceComp = sourceCompProp.objectReferenceValue as MonoBehaviour;
                if (sourceComp != null)
                {
                    // Reflection: get all fields that are Observable<T>
                    var type = sourceComp.GetType();
                    var fields = type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                        .Where(f => typeof(Observable<T>).IsAssignableFrom(f.FieldType))
                        .ToArray();

                    // The next line's Y position
                    float nextLine = sourceRect.y + lineHeight + spacing;
                    Rect popupRect = new Rect(position.x, nextLine, position.width, lineHeight);

                    if (fields.Length > 0)
                    {
                        // Build a list of names
                        string[] fieldNames = fields.Select(f => f.Name).ToArray();
                        
                        // Determine currently selected field index
                        int currentIndex = Array.IndexOf(fieldNames, fieldNameProp.stringValue);

                        // Draw popup
                        int newIndex = EditorGUI.Popup(popupRect, "Observable Field", currentIndex, fieldNames);
                        if (newIndex != currentIndex && newIndex >= 0)
                        {
                            fieldNameProp.stringValue = fieldNames[newIndex];
                        }
                    }
                    else
                    {
                        // If no fields found
                        EditorGUI.LabelField(popupRect, "Observable Field", $"No Observable<{typeof(T)}> fields found");
                    }
                }

                EditorGUI.indentLevel--;
            }

            EditorGUI.EndProperty();
        }

        // We also need to specify how much vertical space we need to draw all these controls
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            float lineHeight = EditorGUIUtility.singleLineHeight;
            float spacing = EditorGUIUtility.standardVerticalSpacing;
            // If not expanded, just one line
            if (!property.isExpanded) return lineHeight;

            // If expanded, we have:
            //   1 line for the foldout
            //   1 line for the sourceComponent
            //   1 line for the popup
            return lineHeight * 3 + spacing * 2;
        }
    }
}