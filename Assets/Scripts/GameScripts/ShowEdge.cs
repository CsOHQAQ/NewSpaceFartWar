using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class ShowEdge : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        float x = Camera.main.orthographicSize * Camera.main.aspect,
            y = Camera.main.orthographicSize;
        Debug.DrawLine(new Vector3(x,y),new Vector3(x,-y),Color.red);
        Debug.DrawLine(new Vector3(-x, y), new Vector3(-x, -y), Color.red);
        Debug.DrawLine(new Vector3(x, -y), new Vector3(-x, -y), Color.red);
        Debug.DrawLine(new Vector3(x, y), new Vector3(-x, y), Color.red);
    }
}
