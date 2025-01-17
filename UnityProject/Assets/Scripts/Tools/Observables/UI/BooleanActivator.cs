using System.Collections;
using System.Collections.Generic;
using Clovers.Tools;
using UnityEngine;

namespace Clovers.Tools
{
    public class BooleanActivator : ToolboxMonoBehaviour
    {
        [SerializeField]
        private ObservableBoolReference _bool = new();

        [SerializeField]
        private GameObject _target;
        
        protected override void OnReady()
        {
            OnCleanup += _bool.Value.Track(_target.SetActive);
        }
    }
}
