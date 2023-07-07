using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;
public class PlayerController : MonoBehaviour
{
    Rigidbody2D rigidbody;
    Transform fartTransform;
    Transform grabTransform;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        fartTransform = transform.Find("fartTransform");
        grabTransform = transform.Find("grabTransform");

    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }
    public void Move()
    {

    }
}
