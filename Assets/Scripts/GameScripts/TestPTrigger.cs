using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPTrigger : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("物品Collision检测到物体进入");
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        Debug.Log("物品Collision检测到物体");
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        Debug.Log("物品Trigger检测到物体");
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("物品Trigger检测到物体进入");
    }
}
