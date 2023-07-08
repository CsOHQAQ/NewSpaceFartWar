using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;
using QxFramework.Core;

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
    public float threshold;

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
            body.AddForce(transform.localScale.x * transform.right * bigFart, ForceMode2D.Impulse);
            heavyParticle.HeavyEmission();
        }

        switch (touchState)
        {
            case TouchState.None:
                if (player.GetButtonDown("Touch"))
                {
                    Transform trans = transform.Find("Spaceman/TouchPos");
                    RaycastHit2D[] raycasts = Physics2D.RaycastAll(trans.position, trans.up, touchDis);
                    foreach (var ray in raycasts)
                    {
                        if (ray.rigidbody != null && ray.rigidbody != body)
                        {
                            touchingPos = ray.point + (Vector2)trans.up * 0.1f;
                            touchingBody = ray.rigidbody;

                            springJoint.enabled = true;
                            springJoint.enableCollision = true;
                            springJoint.anchor = body.centerOfMass;
                            springJoint.connectedBody = ray.rigidbody;
                            springJoint.connectedAnchor = ray.rigidbody.transform.InverseTransformPoint(touchingPos);

                            distanceJoint.enabled = false;
                            distanceJoint.enableCollision = true;
                            distanceJoint.anchor = body.centerOfMass;
                            distanceJoint.connectedBody = ray.rigidbody;
                            distanceJoint.connectedAnchor = ray.rigidbody.transform.InverseTransformPoint(touchingPos);
                            touchState = TouchState.Pull;
                            break;
                        }
                    }
                    if (touchState == TouchState.Pull)
                    {
                        GameObject go = ResourceManager.Instance.Instantiate("Prefabs/Effect/Line2");
                        go.transform.position = new Vector3();
                        LineRenderer line = go.GetComponent<LineRenderer>();
                        line.positionCount = 2;
                        go.GetComponent<SyncLinePos>().SetPos(trans, trans.position, touchingBody.transform, touchingPos);
                        go = ResourceManager.Instance.Instantiate("Prefabs/Effect/TouchEffect");
                        go.transform.position = touchingPos;
                        go.transform.eulerAngles = new Vector3(0, 0, 180 + transform.eulerAngles.z);
                    }
                    else
                    {
                        GameObject go = ResourceManager.Instance.Instantiate("Prefabs/Effect/Line1");
                        go.transform.SetParent(transform);
                        go.transform.position = new Vector3();
                        LineRenderer line = go.GetComponent<LineRenderer>();
                        line.positionCount = 2;
                        go.GetComponent<SyncLinePos>().SetPos(trans, trans.position, trans, trans.position + (trans.up * touchDis));
                    }
                }
                break;
            case TouchState.Pull:
                if (Vector2.Distance(touchingPos, transform.TransformPoint(body.centerOfMass)) < threshold)
                {
                    springJoint.enabled = false;
                    distanceJoint.enabled = true;
                    touchState = TouchState.Touch;
                }
                if (player.GetButtonDown("Touch"))
                {
                    springJoint.enabled = false;
                    distanceJoint.enabled = false;
                    touchingBody = null;
                    touchState = TouchState.None;
                }
                break;
            case TouchState.Touch:
                if (touchingBody != null)
                {
                    if (player.GetButtonDown("Touch"))
                    {
                        springJoint.enabled = false;
                        distanceJoint.enabled = false;
                        touchingBody = null;
                        touchState = TouchState.None;
                    }
                    else if (player.GetButtonDown("Push"))
                    {
                        springJoint.enabled = false;
                        distanceJoint.enabled = false;
                        touchingBody.AddForceAtPosition(transform.localScale.x * transform.right * pushForce, touchingPos, ForceMode2D.Impulse);
                        body.AddForce(-transform.localScale.x * transform.right * pushForce, ForceMode2D.Impulse);
                        touchingBody = null;
                        touchState = TouchState.None;
                    }
                }
                break;
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

    public void Hurt(float damage)
    {
        hp -= damage;
        //发消息更新UI
        if (hp < 0)
        {
            //死亡
        }
    }
}
