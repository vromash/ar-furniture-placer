using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

namespace UnityEngine.XR.Interaction.Toolkit.AR
{
    public class ARPlacementInteractableUIBlock : ARPlacementInteractable
    {
        [SerializeField]
        [Tooltip("The LayerMask that is used during an additional raycast when a user touch does not hit any AR trackable planes.")]
        LayerMask m_FallbackLayerMaskNew;

        static readonly List<ARRaycastHit> s_Hits = new List<ARRaycastHit>();

        protected override bool TryGetPlacementPose(TapGesture gesture, out Pose pose)
        {
            // bool isOverUI = gesture.startPosition.IsPointOverUIObject();
            bool isOverUI = Vector2Extensions.IsPointOverUIObject(gesture.startPosition);

            // Raycast against the location the player touched to search for planes.
            if (!isOverUI && GestureTransformationUtility.Raycast(gesture.startPosition, s_Hits, arSessionOrigin, TrackableType.PlaneWithinPolygon, m_FallbackLayerMaskNew))
            {
                pose = s_Hits[0].pose;

                // Use hit pose and camera pose to check if hit test is from the
                // back of the plane, if it is, no need to create the anchor.
                // ReSharper disable once LocalVariableHidesMember -- hide deprecated camera property
                var camera = arSessionOrigin != null ? arSessionOrigin.camera : Camera.main;
                if (camera == null)
                    return false;

                return Vector3.Dot(camera.transform.position - pose.position, pose.rotation * Vector3.up) >= 0f;
            }

            pose = default;
            return false;
        }
    }
}