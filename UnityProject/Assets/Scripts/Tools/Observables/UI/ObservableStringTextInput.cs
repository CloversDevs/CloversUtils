using System;
using Clovers.Tools;
using TMPro;
using UnityEngine;

namespace Clovers.Tools
{
    /// <summary>
    /// Script to set an observable string value from the Unity UI.
    /// </summary>
    [RequireComponent(typeof(TMP_InputField))]
    public class ObservableStringTextInput : ToolboxMonoBehaviour
    {
        [SerializeField] 
        private ObservableStringReference _targetObservable;
        
        [SerializeField, HideInInspector]
        private TMP_InputField _inputField;

        /// <summary>
        /// Update Observable value.
        /// </summary>
        public void OnChangeValue(string value)
        {
            _targetObservable.Value.Value = value;
        }

        protected override void OnReady()
        {
            if(_inputField == null) return;

            _inputField.onValueChanged.AddListener(OnChangeValue);
            _inputField.onEndEdit.AddListener(OnChangeValue);
            
            OnCleanup += ()=> _inputField.onValueChanged.RemoveListener(OnChangeValue);
            OnCleanup += ()=> _inputField.onValueChanged.RemoveListener(OnChangeValue);
        }
        
        private void OnValidate()
        {
            _inputField ??= GetComponent<TMP_InputField>();
        }
    }
}

