using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSprint : MonoBehaviour
{
    private LineRenderer line;
    private SpringJoint2D spring;
    private float moveSpeed = 15f;
    private GameObject catchedObj=null;
    private bool isCatching = false;
    // Start is called before the first frame update
    void Start()
    {
        line = GetComponent<LineRenderer>();    
        spring = GetComponent<SpringJoint2D>(); 

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Space"))
        {
            if(isCatching)
            {
                isCatching= false; 
                catchedObj = null;
                line.SetPosition(1,line.GetPosition(0)+new Vector3(1,1,0));
            }
            else
            {
                isCatching= true;
            }
        }
        if (!isCatching)
        {
            if (Input.GetButton("Q"))
            {
            }
        }
        else//正在伸长或是已经抓住了物体
        {

        }
    }

}
