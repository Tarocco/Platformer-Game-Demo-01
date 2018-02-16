using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SubjectNerd.Utilities;
using System;

[ExecuteInEditMode]
public class PixelCameraZoom : MonoBehaviour, ISerializationCallbackReceiver
{
    public Camera Camera;
    public PixelCamera PixelCamera;

    public float NominalFOV = 30f;
    public int NominalHeight = 150;

    void Start ()
    {
		
	}

    private static float GetFOV(float nominal_fov, float nominal_height, float height)
    {
        float z = nominal_height / Mathf.Tan(nominal_fov * 0.5f * Mathf.Deg2Rad);
        return 2.0f * Mathf.Atan2(height, z) * Mathf.Rad2Deg;
    }
    private static float GetPerspectiveZ(float fov, float height)
    {
        return 0.5f * height / Mathf.Tan(fov * 0.5f * Mathf.Deg2Rad);
    }

    void UpdatePixelCamera()
    {
        if (PixelCamera)
        {
            var zoom = 1 + Screen.height / NominalHeight;
            PixelCamera.ZoomLevel = zoom;
            if (!Camera.orthographic)
            {
                //Camera.fieldOfView = NominalFOV;
                Camera.fieldOfView = GetFOV(NominalFOV, NominalHeight, Screen.height);

                var perspective_z = GetPerspectiveZ(Camera.fieldOfView, Screen.height / PixelCamera.PixelsPerUnit);
                PixelCamera.PerspectiveZ = perspective_z;

                var camera_position = Camera.transform.localPosition;
                camera_position.z = -perspective_z / zoom;
                Camera.transform.localPosition = camera_position;
            }
        }
    }

	void LateUpdate ()
    {
        UpdatePixelCamera();
	}

#if UNITY_EDITOR
    void OnValidate()
    {
        UpdatePixelCamera();
        //UnityEditorInternal.InternalEditorUtility.RepaintAllViews();
    }
#endif

    public void OnBeforeSerialize()
    {
        if (Camera == null)
            Camera = GetComponent<Camera>();
        PixelCamera = PixelCamera ?? GetComponent<PixelCamera>();
    }

    public void OnAfterDeserialize()
    {
    }
}
