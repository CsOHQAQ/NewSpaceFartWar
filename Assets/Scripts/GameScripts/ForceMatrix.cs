using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceMatrix : MonoBehaviour
{
    public float baseF = 4f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.GetComponent<Rigidbody2D>() != null)
        {
            float dis=Vector2.Distance(transform.position, collision.transform.position);
            collision.GetComponent<Rigidbody2D>().AddForce(new Vector2(collision.transform.position.x - transform.position.x, collision.transform.position.y - transform.position.y).normalized*baseF/(dis*dis));
        }
    }
}
