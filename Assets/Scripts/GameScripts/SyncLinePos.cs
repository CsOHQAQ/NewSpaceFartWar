using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class SyncLinePos : MonoBehaviour
{
    private LineRenderer lineRenderer;
    private Transform start;
    private Transform end;
    private Vector3 startPos;
    private Vector3 endPos;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 2;
    }

    public void SetPos(Transform start, Vector3 startPos, Transform end, Vector3 endPos)
    {
        this.start = start;
        this.end = end;
        this.startPos = start.InverseTransformPoint(startPos);
        this.endPos = end.InverseTransformPoint(endPos);
        SyncPos();
    }

    private void Update()
    {
        SyncPos();
    }

    private void SyncPos()
    {
        lineRenderer.SetPosition(0, start.TransformPoint(startPos));
        lineRenderer.SetPosition(1, end.TransformPoint(endPos));
    }
}
