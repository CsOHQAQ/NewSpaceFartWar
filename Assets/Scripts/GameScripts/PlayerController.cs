using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class PlayerController : MonoBehaviour
{
    public enum TouchState
    {
        None,
        Pull,
        Touch,
    }

    public int playerIndex;
    public float bigFart;
    public float animationCounter;
    public float rotateFart;
    public float maxHP;
    public float touchDis;
    public float pushForce;

    private float hp;

    private FartParticleController lightParticle;
    private FartParticleController heavyParticle;
    private Player player;
    private Rigidbody2D body;
    private float counter;
    private Rigidbody2D touchingBody;
    private SpringJoint2D springJoint;
    private DistanceJoint2D distanceJoint;
    private Vector3 touchingPos;
    private TouchState touchState;

    private void Start()
    {
        springJoint = GetComponent<SpringJoint2D>();
        distanceJoint = GetComponent<DistanceJoint2D>();
		lightParticle = transform.Find("Spaceman/FartPos/LightFartParticle").GetComponent<FartParticleController>();
        heavyParticle = transform.Find("Spaceman/FartPos/HeavyFartParticle").GetComponent<FartParticleController>();
        body = GetComponent<Rigidbody2D>();
        springJoint.enabled = false;
        distanceJoint.enabled = false;
        player = ReInput.players.GetPlayer(playerIndex);
        touchState = TouchState.None;
        hp = maxHP;
    }

    private void Update()
    {
        if (player.GetButtonDown("Heavy") && counter <= 0)
        {
            counter = animationCounter;
            body.AddForce(transform.right * bigFart, ForceMode2D.Impulse);
            heavyParticle.HeavyEmission();
        }

        if (touchingBody != null)
        {
            if (player.GetButtonDown("Touch"))
            {
                springJoint.enabled = false;
                touchingBody = null;
            }
            else if (player.GetButtonDown("Push"))
            {
                springJoint.enabled = false;
                touchingBody.AddForceAtPosition(transform.right * pushForce, touchingPos, ForceMode2D.Impulse);
                body.AddForce(-transform.right * pushForce, ForceMode2D.Impulse);
                touchingBody = null;
            }
        }
        else
        {
            if (player.GetButtonDown("Touch"))
            {
                TryTouch();
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
            transform.Find("Spaceman").GetComponent<Animator>().SetBool("Rotate", true);
            if (touchingBody != null && touchingBody.mass > body.mass)
            {
                body.AddRelativeForce(-Vector2.up * rotateFart);
            }
            else
            {
                body.AddTorque(rotateFart);
            }
            lightParticle.LightEmission();
        }
        else if (!player.GetButton("LightLeft") && player.GetButton("LightRight"))
        {
            transform.Find("Spaceman").GetComponent<Animator>().SetBool("Rotate", true);
            if (touchingBody != null && touchingBody.mass > body.mass)
            {
                body.AddRelativeForce(Vector2.up * rotateFart);
            }
            else
            {
                body.AddTorque(-rotateFart);
            }
            lightParticle.LightEmission();
        }
        else
        {
            transform.Find("Spaceman").GetComponent<Animator>().SetBool("Rotate", false);
            lightParticle.EndLightEmission();
        }
    }

    /*private void TurnTo(bool right)
    {
        isFacingRight = right;
        transform.Find("Spaceman").localScale = new Vector3(right ? 1 : -1, 1, 1);
    }*/

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
        RaycastHit2D[] raycasts = Physics2D.RaycastAll(trans.position, trans.up, touchDis);
        foreach (var ray in raycasts)
        {
            if (ray.rigidbody != null && ray.rigidbody != body)
            {
                springJoint.enabled = true;
                springJoint.enableCollision = true;
                springJoint.anchor = body.centerOfMass;
                springJoint.connectedBody = ray.rigidbody;
                touchingPos = ray.point + (Vector2)trans.up * 0.1f;
                springJoint.connectedAnchor = ray.rigidbody.transform.InverseTransformPoint(touchingPos);
                touchingBody = ray.rigidbody;
                return true;
            }
        }
        return false;
    }
}
