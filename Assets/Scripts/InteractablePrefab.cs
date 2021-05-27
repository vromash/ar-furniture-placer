using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEngine.XR.Interaction.Toolkit.AR
{
    public class InteractablePrefab : MonoBehaviour
    {
        ARInteractions ARInteractions;

        private void Awake()
        {
            ARInteractions = GameObject.Find("AR Interactions").GetComponent<ARInteractions>();
        }

        public void OnSelectedEntered(SelectEnterEventArgs args)
        {
            ARInteractions.ChangeSelectedObject(args);
        }

        public void OnSelectedExited(SelectExitEventArgs args)
        {
            ARInteractions.ClearSelectedObject(args);
        }
    }
}