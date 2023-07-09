using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class DynamicSlash : MonoBehaviour
{
    public int pointCount = 10;
    public int lineCount = 50;
    public Transform start;
    public Transform end;

    private float counter;
    private bool _rendering;
    private MeshFilter meshFilter;
    private Queue<Vector3> _pointCacheUp = new Queue<Vector3>();
    private Queue<Vector3> _pointCacheDown = new Queue<Vector3>();
    private AutoCurve curveUp;
    private AutoCurve curveDown;

    public void StartRendering()
    {
        _rendering = true;
    }

    public void StopRendering()
    {
        _rendering = false;
    }

    private void Awake()
    {
        meshFilter = GetComponent<MeshFilter>();
        var meshRenderer = GetComponent<MeshRenderer>();
        curveUp = new AutoCurve(lineCount);
        curveDown = new AutoCurve(lineCount);
    }

    private void FixedUpdate()
    {
        if (start != null && end != null)
        {
            if (_rendering)
            {
                /*
                counter -= Time.deltaTime;
                if (counter <= 0)
                {
                    counter += time;
                    _pointCacheUp.Enqueue(start.transform.position);
                }
                */
                _pointCacheUp.Enqueue(start.transform.position);
                if (_pointCacheUp.Count > pointCount)
                {
                    _pointCacheUp.Dequeue();
                }
                _pointCacheDown.Enqueue(end.transform.position);
                if (_pointCacheDown.Count > pointCount)
                {
                    _pointCacheDown.Dequeue();
                }
            }
            else
            {
                _pointCacheUp.Enqueue(start.transform.position);
                if (_pointCacheUp.Count > pointCount)
                {
                    _pointCacheUp.Dequeue();
                }
                _pointCacheDown.Enqueue(end.transform.position);
                if (_pointCacheDown.Count > pointCount)
                {
                    _pointCacheDown.Dequeue();
                }
                if (_pointCacheUp.Count > 0)
                {
                    _pointCacheUp.Dequeue();
                }
                if (_pointCacheDown.Count > 0)
                {
                    _pointCacheDown.Dequeue();
                }
                if (_pointCacheUp.Count > 0)
                {
                    _pointCacheUp.Dequeue();
                }
                if (_pointCacheDown.Count > 0)
                {
                    _pointCacheDown.Dequeue();
                }
            }
            if (_pointCacheUp != null && _pointCacheUp.Count > 2 && _pointCacheDown != null && _pointCacheDown.Count > 2)
            {
                curveUp.LineCount = lineCount;
                curveUp.Point = _pointCacheUp.ToArray();
                curveDown.LineCount = lineCount;
                curveDown.Point = _pointCacheDown.ToArray();
                meshFilter.mesh = GenerateMesh();
            }
            else
            {
                meshFilter.mesh = null;
            }
        }
    }

    private Mesh GenerateMesh()
    {
        Mesh mesh = new Mesh();

        List<Vector3> topVertices = new List<Vector3>();
        List<Vector3> endVertices = new List<Vector3>();
        float topLength = 0;
        float endLength = 0;

        Vector3 lastPoint = curveUp.GetHermitAtTime(0);
        for (int i = 1; i < curveUp.LineCount; i++)
        {
            float time = i / (float)lineCount;
            Vector3 point = curveUp.GetHermitAtTime(time);
            topLength += Vector3.Distance(point, lastPoint);
            topVertices.Add(lastPoint);
            lastPoint = point;
        }
        lastPoint = curveDown.GetHermitAtTime(0);
        for (int i = 1; i < curveDown.LineCount; i++)
        {
            float time = i / (float)lineCount;
            Vector3 point = curveDown.GetHermitAtTime(time);
            endLength += Vector3.Distance(point, lastPoint);
            endVertices.Add(lastPoint);
            lastPoint = point;
        }

        float distanceTop = 0;
        float distanceEnd = 0;
        var vertices = new Vector3[topVertices.Count * 2];
        var uv = new Vector2[topVertices.Count * 2];
        var triangles = new int[(topVertices.Count - 1) * 6];
        for (int i = 0; i < topVertices.Count; i++)
        {
            vertices[i] = topVertices[topVertices.Count - i - 1];
            vertices[i + topVertices.Count] = endVertices[endVertices.Count - i - 1];
            if (i != 0)
            {
                distanceTop += Vector3.Distance(vertices[i], vertices[i - 1]);
                distanceEnd += Vector3.Distance(vertices[i + topVertices.Count], vertices[i + topVertices.Count - 1]);
            }
            uv[i] = new Vector2(distanceTop / topLength, 1);
            uv[i + topVertices.Count] = new Vector2(distanceEnd / endLength, 0);
            if (i != topVertices.Count - 1)
            {
                triangles[6 * i] = i;
                triangles[6 * i + 1] = i + topVertices.Count;
                triangles[6 * i + 2] = i + 1;
                triangles[6 * i + 3] = i + topVertices.Count;
                triangles[6 * i + 4] = i + 1 + topVertices.Count;
                triangles[6 * i + 5] = i + 1;
            }
        }
        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;
        //mesh.RecalculateNormals();
        //mesh.RecalculateBounds();
        return mesh;
    }
}