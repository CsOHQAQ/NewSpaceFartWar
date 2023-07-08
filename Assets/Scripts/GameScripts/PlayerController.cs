using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class PlayerController : MonoBehaviour
{
    public int playerIndex;
    public float bigFart;
    public float animationCounter;
    public float rotateFart;
    public float maxHP;
    public float touchDis;

    private float hp;

    private Player player;
    private Rigidbody2D body;
    private float counter;
    private bool touching;

    private void Start()
    {
        body = GetComponent<Rigidbody2D>();
        GetComponent<SpringJoint2D>().enabled = false;
        player = ReInput.players.GetPlayer(playerIndex);
        hp = maxHP;
    }

    private void Update()
    {
        if (player.GetButtonDown("Heavy") && counter <= 0)
        {
            counter = animationCounter;
            body.AddForce(transform.up * bigFart, ForceMode2D.Impulse);
        }

        if (touching)
        {
            if (player.GetButtonDown("Touch"))
            {
                SpringJoint2D springJoint2D = GetComponent<SpringJoint2D>();
                springJoint2D.enabled = false;
            }
        }
        else
        {
            if (player.GetButtonDown("Touch"))
            {
                if (TryTouch())
                {
                    touching = true;
                }
            }
        }

        if (counter > 0)
        {
            counter -= Time.deltaTime;
            transform.Find("Spaceman").GetComponent<Animator>().SetBool("Speed", true);
        }
        else
        {
            transform.Find("Spaceman").GetComponent<Animator>().SetBool("Speed", false);
        }
    }

    private void FixedUpdate()
    {
        if (player.GetButton("LightLeft") && !player.GetButton("LightRight"))
        {
            TurnTo(false);
            transform.Find("Spaceman").GetComponent<Animator>().SetBool("Rotate", true);
            Transform trans = transform.Find("Spaceman/FartPos");
            body.AddForceAtPosition(trans.up * rotateFart, trans.position);
        }
        else if (!player.GetButton("LightLeft") && player.GetButton("LightRight"))
        {
            TurnTo(true);
            transform.Find("Spaceman").GetComponent<Animator>().SetBool("Rotate", true);
            Transform trans = transform.Find("Spaceman/FartPos");
            body.AddForceAtPosition(trans.up * rotateFart, trans.position);
        }
        else
        {
            transform.Find("Spaceman").GetComponent<Animator>().SetBool("Rotate", false);
        }
    }

    private void TurnTo(bool right)
    {
        transform.Find("Spaceman").localScale = new Vector3(right ? 1 : -1, 1, 1);
    }

    public void Hurt(float damage)
    {
        hp -= damage;
        //发消息更新UI
        if (hp < 0)
        {
            //死亡
        }
    }

    public bool TryTouch()
    {
        Transform trans = transform.Find("Spaceman/TouchPos");
        RaycastHit2D[] raycasts = Physics2D.RaycastAll(trans.position, trans.up * touchDis);
        foreach (var ray in raycasts)
        {
            if (ray.rigidbody != null && ray.rigidbody != body)
            {
                SpringJoint2D springJoint2D = GetComponent<SpringJoint2D>();
                springJoint2D.enabled = true;
                springJoint2D.enableCollision = true;
                springJoint2D.anchor = body.centerOfMass;
                springJoint2D.connectedBody = ray.rigidbody;
                springJoint2D.connectedAnchor = ray.rigidbody.transform.InverseTransformPoint(ray.point + (Vector2)trans.up * 0.1f);
                return true;
            }
        }
        return false;
    }
}
