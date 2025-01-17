using System.Collections;
using System.Collections.Generic;
using Clovers.Tools;
using TMPro;
using UnityEngine;

namespace Clovers.Tools
{
    public class ObservableIntTextDisplay : ToolboxMonoBehaviour
    {
        [SerializeField] 
        private ObservableIntReference _targetObservable;
        
        [SerializeField] 
        private TMP_Text _text;
    
        protected override void OnReady()
        {
            OnCleanup += _targetObservable.Value.Track(OnValueChange);
        }

        private void OnValueChange(int value)
        {
            _text.text = value.ToString();
        }
    }
}
