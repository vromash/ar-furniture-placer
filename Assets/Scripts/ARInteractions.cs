using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;

namespace UnityEngine.XR.Interaction.Toolkit.AR
{
    public class ARInteractions : MonoBehaviour
    {
        [SerializeField]
        private ARPlaneManager ARPlaneManager;
        [SerializeField]
        private ARPointCloudManager ARPointCloudManager;
        [SerializeField]
        private UIManager UIManager;
        [SerializeField]
        private ARSession ARSession;
        public List<GameObject> placedObjects;
        private GameObject selectedObject;

        public void AddPlacedObject(ARObjectPlacementEventArgs args)
        {
            placedObjects.Add(args.placementObject);
        }

        public void RemoveAllPlacedObjects()
        {
            foreach (GameObject placed in placedObjects)
            {
                Destroy(placed);
            }
            placedObjects.Clear();
        }

        public void ResetScene()
        {
            ARSession.Reset();
            RemoveAllPlacedObjects();
        }

        public void TogglePlaneDetection()
        {
            ARPlaneManager.enabled = !ARPlaneManager.enabled;
            ARPointCloudManager.enabled = !ARPointCloudManager.enabled;
            TogglePlaneVizualization(ARPlaneManager.enabled);
            TogglePointCloudVizualization(ARPointCloudManager.enabled);
            UIManager.TogglePlaneHideButton();
        }

        private void TogglePlaneVizualization(bool value)
        {
            foreach (var plane in ARPlaneManager.trackables)
                plane.gameObject.SetActive(value);
        }

        private void TogglePointCloudVizualization(bool value)
        {
            foreach (var pointCloud in ARPointCloudManager.trackables)
                pointCloud.gameObject.SetActive(value);
        }

        public void DeleteSelectedObject()
        {
            Destroy(selectedObject);
        }

        public void ChangeSelectedObject(SelectEnterEventArgs args)
        {
            UIManager.ToggleDeleteObjectButton();
            selectedObject = args.interactable.gameObject;
        }

        public void ClearSelectedObject(SelectExitEventArgs args)
        {
            UIManager.ToggleDeleteObjectButton();
            selectedObject = null;
        }
    }
}