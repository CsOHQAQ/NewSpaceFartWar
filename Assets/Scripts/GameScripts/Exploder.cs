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
    private bool explodable = false;
    private void Start()
    {
        explodable = false;
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
        if (this == null)
            return;
        UIArgs<Rigidbody2D> rigbody = (UIArgs<Rigidbody2D>)arg;
        GameObject gameObj = rigbody.Data.gameObject;
        if (gameObj.GetHashCode() == this.gameObject.GetHashCode())
        {
            explodable = true;
        }
    }

    private void Explode()
    {
        /*GetComponent<PolygonCollider2D>().enabled = false;
        for (int i = 1; i <= explodePiece; i++)
        {
            float curAngle = 2 * 3.14f * i / explodePiece;
            GameObject piece = ResourceManager.Instance.Instantiate("Prefabs/Trash/Debris (6)");
            piece.transform.position = transform.position + new Vector3(Mathf.Cos(curAngle), Mathf.Sin(curAngle));
            piece.GetComponent<Rigidbody2D>().AddForce(new Vector2(Mathf.Cos(curAngle), Mathf.Sin(curAngle)) * explodeForce, ForceMode2D.Impulse);
            //piece.GetComponent<Rigidbody2D>().velocity = new Vector2(Mathf.Cos(curAngle), Mathf.Sin(curAngle)) * explodeForce;
        }
        ObjectPool.Recycle(this.gameObject);*/
        Launcher.Instance.StartCoroutine(LateSummonExplode(transform.position));
        ObjectPool.Recycle(this.gameObject);
    }

    private IEnumerator LateSummonExplode(Vector2 pos)
    {
        Debug.Log("跳过第一帧");
        yield return new WaitForSeconds(0.05f);
        Debug.Log("开始执行后续");
        for (int i = 1; i <= explodePiece; i++)
        {
            float curAngle = 2 * 3.14f * i / explodePiece;
            GameObject piece = ResourceManager.Instance.Instantiate($"Prefabs/Explode/Debris ({i})");
            ResourceManager.Instance.Instantiate("Prefabs/Effect/Diffuse").transform.position = pos;
            ResourceManager.Instance.Instantiate("Prefabs/Effect/Flash").transform.position = pos;
            piece.transform.position = pos + new Vector2(Mathf.Cos(curAngle), Mathf.Sin(curAngle));
            Rigidbody2D rig = piece.GetComponent<Rigidbody2D>();
            rig.WakeUp();
            rig.AddForce(new Vector2(Mathf.Cos(curAngle), Mathf.Sin(curAngle)) * explodeForce, ForceMode2D.Impulse);
        }
    }
}
