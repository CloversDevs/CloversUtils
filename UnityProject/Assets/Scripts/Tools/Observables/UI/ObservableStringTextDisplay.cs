using System;
using Clovers.Tools;
using TMPro;
using UnityEngine;

namespace Clovers.Tools
{
    [RequireComponent(typeof(TMP_Text))]
    public class ObservableStringTextDisplay : ToolboxMonoBehaviour
    {
        [SerializeField] 
        private ObservableStringReference _targetObservable;
        
        [SerializeField, HideInInspector] 
        private TMP_Text _text;

        private void OnValidate()
        {
            _text ??= GetComponent<TMP_Text>();
        }

        protected override void OnReady()
        {
            OnCleanup += _targetObservable.Value.Track(OnValueChange);
        }

        private void OnValueChange(string value)
        {
            _text.text = value;
        }
    }
}

