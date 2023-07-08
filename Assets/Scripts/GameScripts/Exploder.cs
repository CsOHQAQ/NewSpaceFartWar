using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QxFramework.Core;
using System;

public class Exploder : MonoBehaviour
{
    public int explodePiece = 6;
    public float explodeForce = 10f;
    public float explodeSpeed = 5f;
    public bool explodable = false;
    private void Start()
    {
        MessageManager.Instance.Get<PlayerController.TouchState>().RegisterHandler(PlayerController.TouchState.Throw, SetExplodable);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(explodable)
        {
            Explode();
        }
    }
    
    private void SetExplodable(System.Object sender, EventArgs arg)
    {
        if (this== null)
            return;
        UIArgs<Rigidbody2D> rigbody=(UIArgs<Rigidbody2D>)arg;
        GameObject gameObj = rigbody.Data.gameObject;
        if (gameObj.GetHashCode()==this.gameObject.GetHashCode())
        {
            explodable = true;
        }
    }

     private void Explode()
    {
        GetComponent<PolygonCollider2D>().enabled = false;
        for (int i = 1; i <= explodePiece; i++)
        {
            float curAngle = 2 * 3.14f * i / explodePiece;
            GameObject piece = ResourceManager.Instance.Instantiate("Prefabs/Trash/Debris (6)");
            piece.transform.position = transform.position + new Vector3(Mathf.Cos(curAngle), Mathf.Sin(curAngle));
            piece.GetComponent<Rigidbody2D>().AddForce(new Vector2(Mathf.Cos(curAngle), Mathf.Sin(curAngle)) * explodeForce);
            //piece.GetComponent<Rigidbody2D>().velocity = new Vector2(Mathf.Cos(curAngle), Mathf.Sin(curAngle)) * explodeForce;
        }
        ObjectPool.Recycle(this.gameObject);
    }
    private IEnumerator LateSummonExplode(Vector2 pos)
    {
        Debug.Log("跳过第一帧");
        yield return 0;
        Debug.Log("开始执行后续");
        for (int i = 1; i <= explodePiece; i++)
        {
            float curAngle = 2 * 3.14f * i / explodePiece;
            GameObject piece = ResourceManager.Instance.Instantiate("Prefabs/Trash/Debris (6)");
            piece.transform.position = pos + new Vector2(Mathf.Cos(curAngle), Mathf.Sin(curAngle));
            //piece.GetComponent<Rigidbody2D>().AddForce(new Vector2(Mathf.Cos(curAngle), Mathf.Sin(curAngle)) * explodeForce);
            piece.GetComponent<Rigidbody2D>().velocity = new Vector2(Mathf.Cos(curAngle), Mathf.Sin(curAngle)) * explodeForce;
        }

    }
}
