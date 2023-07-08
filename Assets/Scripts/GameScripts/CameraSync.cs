using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
[ExecuteAlways]
public class CameraSync : MonoBehaviour
{
    [Tooltip("Parent camera to sync. Empty for its parent.")]
    public Camera parent;
    private Camera self;

    private void Update()
    {
        if (CheckParentCamera())
        {
            self.orthographic = parent.orthographic;
            self.fieldOfView = parent.fieldOfView;
            self.orthographicSize = parent.orthographicSize;
        }
    }

    public bool CheckParentCamera()
    {
        if (parent == null)
        {
            parent = transform.parent.GetComponentInParent<Camera>();
        }
        if (self == null)
        {
            self = GetComponent<Camera>();
        }
        return parent != null;
    }
}