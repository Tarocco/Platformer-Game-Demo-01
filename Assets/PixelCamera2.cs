using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
public class PixelCamera2 : MonoBehaviour, ISerializationCallbackReceiver
{
    [SerializeField]
    private Camera _Camera;
    public Camera Camera
    {
        get { return _Camera; }
        set { _Camera = value; }
    }

    private Camera _OutputCamera;
    public Camera OutputCamera
    {
        get { return _OutputCamera; }
    }

    [SerializeField]
    private float _NominalFOV = 30f;
    public float NominalFOV
    {
        get { return _NominalFOV; }
        set { _NominalFOV = value; }
    }
    [SerializeField]
    private int _NominalHeight = 150;
    public int NominalHeight
    {
        get { return _NominalHeight; }
        set { _NominalHeight = value; }
    }

    private void OnRenderImage(RenderTexture src, RenderTexture dst)
    {

    }

    public void OnBeforeSerialize()
    {
        if (Camera == null)
            Camera = GetComponent<Camera>();
    }

    public void OnAfterDeserialize()
    {
    }
}
