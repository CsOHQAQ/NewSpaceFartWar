using QxFramework.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlashInstantiate : MonoBehaviour
{
    public Transform start;
    public Transform end;
    public bool rendering;

    private DynamicSlash _slash;
    private bool _lastRendering;

    private void Awake()
    {
        if (start == null)
        {
            start = transform.Find("Start");
        }
        if (end == null)
        {
            end = transform.Find("End");
        }
        if (start != null && end != null)
        {
            _slash = ResourceManager.Instance.Instantiate("Prefabs/Effect/DynamicSlash").GetComponent<DynamicSlash>();
            _slash.transform.position = new Vector3();
            _slash.start = start;
            _slash.end = end;
        }
    }

    private void Update()
    {
        if (_slash != null)
        {
            if (_lastRendering != rendering)
            {
                if (rendering)
                {
                    _slash.StartRendering();
                }
                else
                {
                    _slash.StopRendering();
                }
            }
            _lastRendering = rendering;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(start.position, end.position);
    }

    private void OnDisable()
    {
        _slash.StopRendering();
    }
}
