using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(HighlightEdge))]
[ExecuteInEditMode]
public class HighlightDirection : MonoBehaviour
{
    public enum HighlightDirectionMode
    {
        Global,
        Local,
    }

    public HighlightDirectionMode directionMode;
    public Transform lightSource;
    [ColorUsage(true, true)]
    public Color lightColor;
    [Range(0, 1)]
    public float lightDistance;
    public Vector2 lightDir;
    [Range(0, 1)]
    public float shadowDistance;
    public Vector2 shadowDir;
    [Range(0, 10)]
    public float shadowIntensity;
    [Range(0, 360)]
    public float angle;
    [Range(-180, 180)]
    public float angleOffset;

    private HighlightEdge _highlightEdge;

    public void Update()
    {
        if (_highlightEdge == null)
        {
            _highlightEdge = GetComponent<HighlightEdge>();
        }

        if (directionMode == HighlightDirectionMode.Global)
        {
            _highlightEdge.SetProperties(lightDir, lightColor, shadowDir, shadowIntensity, angle, angleOffset);
        }
        else if (directionMode == HighlightDirectionMode.Local)
        {
            if (lightSource != null)
            {
                Vector2 dir = (lightSource.position - transform.position).normalized;
                dir = dir.Rotate(-transform.eulerAngles.z);
                _highlightEdge.SetProperties(dir * lightDistance, lightColor, -dir * shadowDistance, shadowIntensity, angle, angleOffset);
            }
        }
    }
}
