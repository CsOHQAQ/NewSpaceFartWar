using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;
using QxFramework.Core;
using System;

public class PlayerController : MonoBehaviour
{
    public enum TouchState
    {
        None,
        Pull,
        Touch,
        Throw,
        Sabering,

    }

    public int playerIndex;
    public float bigFart;
    public float bigFartAttack;
    public float animationCounter;
    public float rotateFart;
    public float RotateFart
    {
        get
        {
            if (beanPotTime > 0)
                return rotateFart * beanPotIndex;
            else
                return rotateFart;
        }
    }
    public float maxHP=100;
    public float maxAirAmount=100;
    public float airRecoverSpeed;
    public float airRecoverCounter;
    public float heavyAirConsume;
    public float lightAirConsumeSpeed;
    public float touchDis;
    public float heavyAttackDis;
    public float pushForce;
    public float PushForce
    {
        get
        {
            if (beanPotTime > 0)
                return pushForce * beanPotIndex;
            else
                return pushForce;
        }
    }
    public float throwForce;
    public float ThrowForce
    {
        get
        {
            if (beanPotTime > 0)
                return throwForce * beanPotIndex;
            else
                return throwForce;
        }
    }
    public float threshold;
    public AnimationCurve stunGain;
    public AnimationCurve collisionDamage;
    public AnimationCurve collisionStun;

    public bool isDead = false;

    private float hp;
    private float airAmount;

    private FartParticleController lightParticle;
    private FartParticleController heavyParticle;
    private Player player;
    private Rigidbody2D body;
    private Animator animator;
    private float counter;
    private float touchCounter;
    private float airCounter;
    private float stunCounter;
    private Rigidbody2D touchingBody;
    private SpringJoint2D springJoint;
    private DistanceJoint2D distanceJoint;
    private Vector3 touchingPos;
    private TouchState touchState;

    private float beanPotTime = 0f;
    private float beanPotIndex = 1.5f;

    private float saberTime = 0f;
    private float saberSpeed = 10f;
    private LightSaber lightSaber = null;


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
        airAmount = maxAirAmount;
        animator = transform.Find("Spaceman").GetComponent<Animator>();

        MessageManager.Instance.Get<SpecialItem.ItemFunc>().RegisterHandler(SpecialItem.ItemFunc.Cream, UseCream);
        MessageManager.Instance.Get<SpecialItem.ItemFunc>().RegisterHandler(SpecialItem.ItemFunc.BeanPot, UseBeanPot);
        MessageManager.Instance.Get<SpecialItem.ItemFunc>().RegisterHandler(SpecialItem.ItemFunc.LightSaber, UseLightSaber);

    }

    private void Update()
    {
        if (stunCounter <= 0)
        {
            if (player.GetButtonDown("Heavy") && counter <= 0)
            {
                HeavyFart();
            }
            if (saberTime <= 0)//在光剑时间结束前无法抓取东西
            {
                switch (touchState)
                {
                    case TouchState.None:
                        if (player.GetButtonDown("Touch"))
                        {
                            animator.SetTrigger("Catch");
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
                                    touchCounter = 0.6f;
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
                        if (touchCounter > 0)
                        {
                            touchCounter -= Time.deltaTime;
                        }
                        if (touchCounter < 0 || Vector2.Distance(touchingPos, transform.TransformPoint(body.centerOfMass)) < threshold)
                        {
                            animator.SetTrigger("Touch");
                            springJoint.enabled = false;
                            distanceJoint.enabled = true;
                            touchState = TouchState.Touch;

                            if (touchingBody.GetComponent<SpecialItem>() != null)
                            {
                                SpecialItem specialItem = touchingBody.GetComponent<SpecialItem>();
                                if(specialItem is LightSaber)
                                {
                                    lightSaber= (LightSaber)specialItem;
                                }
                                specialItem.Use();
                            }
                        }
                        if (player.GetButtonDown("Touch"))
                        {
                            animator.SetTrigger("Stop");
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
                                Release();
                            }
                            else if (player.GetButtonDown("Push"))
                            {
                                MessageManager.Instance.Get<TouchState>().DispatchMessage(TouchState.Throw, this, new UIArgs<Rigidbody2D>(touchingBody));
                                animator.SetTrigger("Throw");
                                springJoint.enabled = false;
                                distanceJoint.enabled = false;
                                if (touchingBody != null && touchingBody.mass > body.mass * 2f)
                                {
                                    touchingBody.AddForceAtPosition(transform.localScale.x * transform.right.normalized * pushForce, touchingBody.centerOfMass, ForceMode2D.Impulse);
                                    body.AddForce(-transform.localScale.x * transform.right.normalized * pushForce, ForceMode2D.Impulse);
                                }
                                else
                                {
                                    if (touchingBody.TryGetComponent<PlayerController>(out var com))
                                    {
                                        com.Release();
                                        com.Hurt(10);
                                        com.Stun(3);
                                    }
                                    touchingBody.AddForce(transform.localScale.x * transform.right.normalized * throwForce, ForceMode2D.Impulse);
                                    body.AddForce(-transform.localScale.x * transform.right.normalized * throwForce, ForceMode2D.Impulse);
                                }
                                touchingBody = null;
                                touchState = TouchState.None;
                            }
                        }
                        else//这一段是菊花为了防止使用拾取后道具自己销毁导致卡状态
                        {
                            springJoint.enabled = false;
                            distanceJoint.enabled = false;
                            touchState = TouchState.None;
                        }
                        break;
                }
            }
        }

        if (counter > 0)
        {
            counter -= Time.deltaTime;
        }

        if (stunCounter > 0)
        {
            stunCounter -= Time.deltaTime;
            transform.Find("Spaceman/Stun").gameObject.SetActive(true);
        }
        else
        {
            transform.Find("Spaceman/Stun").gameObject.SetActive(false);
        }

        if (airCounter > 0)
        {
            airCounter -= Time.deltaTime;
        }
        else
        {
            airAmount += Time.deltaTime * airRecoverSpeed;
            if (airAmount > maxAirAmount)
            {
                airAmount = maxAirAmount;
            }
        }

        if(beanPotTime > 0)
        {
            beanPotTime -= Time.deltaTime;
        }
        if (saberTime> 0)
        {
            Debug.Log("Sabering");
            saberTime -= Time.deltaTime;
            animator.SetTrigger("Fart");
            counter = animationCounter;
            body.AddForce(transform.localScale.x * transform.right * bigFart*Time.deltaTime, ForceMode2D.Impulse);
            if(lightSaber!=null)
            {
                lightSaber.Dash(transform.localScale.x * transform.right * bigFart,this);
            }
            else
            {
                Debug.LogError("在Saber时间中的光剑不存在！");
            }
            if (saberTime <= 0)
            {                
                ObjectPool.Recycle(lightSaber); 
                lightSaber = null;

            }
        }

        MessageManager.Instance.Get<PlayerMessage>().DispatchMessage(PlayerMessage.UIRefresh, this, new UIArgs<float>(hp / maxHP));
        MessageManager.Instance.Get<PlayerMessage>().DispatchMessage(PlayerMessage.FartAmount, this, new UIArgs<float>(airAmount / maxAirAmount));
    }

    private void HeavyFart()
    {
        if (UseAir(heavyAirConsume))
        {
            HashSet<Rigidbody2D> rigs = new HashSet<Rigidbody2D>();
            Transform trans = transform.Find("Spaceman/FartPos");
            for (float i = 0; i <= 21; i += 10)
            {
                RaycastHit2D[] raycasts;
                raycasts = Physics2D.RaycastAll(trans.position, ((Vector2)trans.up).Rotate(i), heavyAttackDis);
                foreach (var hit in raycasts)
                {
                    if (hit.rigidbody != null && hit.rigidbody != body)
                    {
                        rigs.Add(hit.rigidbody);
                    }
                }
                raycasts = Physics2D.RaycastAll(trans.position, ((Vector2)trans.up).Rotate(-i), heavyAttackDis);
                foreach (var hit in raycasts)
                {
                    if (hit.rigidbody != null && hit.rigidbody != body)
                    {
                        rigs.Add(hit.rigidbody);
                    }
                }
                foreach (var rig in rigs)
                {
                    rig.AddForce(trans.up * bigFartAttack, ForceMode2D.Impulse);
                    if (rig.TryGetComponent(out PlayerController player))
                    {
                        player.Hurt(10);
                        player.Stun(3);
                    }
                }
            }
            animator.SetTrigger("Fart");
            counter = animationCounter;
            body.AddForce(transform.localScale.x * transform.right * bigFart, ForceMode2D.Impulse);
            heavyParticle.HeavyEmission();
        }
    }

    private void FixedUpdate()
    {
        if (player.GetButton("LightLeft") && !player.GetButton("LightRight") && stunCounter <= 0 && UseAir(lightAirConsumeSpeed * Time.fixedDeltaTime))
        {
            animator.SetBool("RotateForward", true);
            animator.SetBool("RotateBackward", false);
            if (touchingBody != null && touchingBody.mass > body.mass * 1.1f)
            {
                body.AddRelativeForce(-Vector2.up * RotateFart);
            }
            else
            {
                body.AddTorque(RotateFart);
            }
            lightParticle.LightEmission();
        }
        else if (!player.GetButton("LightLeft") && player.GetButton("LightRight") && stunCounter <= 0 && UseAir(lightAirConsumeSpeed * Time.fixedDeltaTime))
        {
            animator.SetBool("RotateBackward", true);
            animator.SetBool("RotateForward", false);
            if (touchingBody != null && touchingBody.mass > body.mass)
            {
                body.AddRelativeForce(Vector2.up * RotateFart);
            }
            else
            {
                body.AddTorque(-RotateFart);
            }
            lightParticle.LightEmission();
        }
        else
        {
            animator.SetBool("RotateBackward", false);
            animator.SetBool("RotateForward", false);
            lightParticle.EndLightEmission();
        }
    }

    public bool UseAir(float amount)
    {
        if (airAmount >= amount)
        {
            airAmount -= amount;
            airCounter = airRecoverCounter;
            return true;
        }
        return false;
    }

    public void Stun(float time)
    {
        stunCounter = time * stunGain.Evaluate(hp / maxHP);
    }

    public void Hurt(float damage)
    {
        hp -= damage;
        hp = Mathf.Clamp(hp, 0, maxHP);
        if (hp < 0)
        {
            Die();
        }
    }

    public void Die()
    {
        hp = 0;
        isDead = true;
        ResourceManager.Instance.Instantiate("Prefabs/Effect/Die").transform.position = transform.position;
        gameObject.SetActive(false);
        MessageManager.Instance.Get<PlayerMessage>().DispatchMessage(PlayerMessage.UIRefresh, this, new UIArgs<float>(hp / maxHP));
        MessageManager.Instance.Get<PlayerMessage>().DispatchMessage(PlayerMessage.Die, this);
        MessageManager.Instance.Get<OffsetControlType>().DispatchMessage(OffsetControlType.Shake, this, new OffsetArgs(0.5f, 0.3f));
    }

    public void Release()
    {
        animator.SetTrigger("Stop");
        springJoint.enabled = false;
        distanceJoint.enabled = false;
        touchingBody = null;
        touchState = TouchState.None;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.attachedRigidbody != null && collision.collider.attachedRigidbody != touchingBody && !collision.collider.attachedRigidbody.TryGetComponent<PlayerController>(out _))
        {
            //Debug.Log(Mathf.Pow(collision.relativeVelocity.magnitude, 2) * collision.collider.attachedRigidbody.mass / 2);
            float damage = collisionDamage.Evaluate(collision.relativeVelocity.magnitude);
            if (damage > 0)
            {
                Hurt(damage);
                Stun(collisionStun.Evaluate(collision.relativeVelocity.magnitude));
            }
        }
    }

    public void UseBeanPot(System.Object sender, EventArgs arg)
    {
        if (touchingBody == null || touchingBody.gameObject != ((SpecialItem)sender).gameObject)
            return;
        ResourceManager.Instance.Instantiate("Prefabs/Effect/UseBean").transform.position = transform.position;
        UIArgs<float> uIArgs = arg as UIArgs<float>;
        float beanTime = uIArgs.Data;
        beanPotTime += beanTime;
    }
    public void UseCream(System.Object sender, EventArgs arg)
    {
        if (touchingBody == null || touchingBody.gameObject != ((SpecialItem)sender).gameObject)
            return;
        ResourceManager.Instance.Instantiate("Prefabs/Effect/UseCream").transform.position = transform.position;
        UIArgs<float> uIArgs=arg as UIArgs<float>;
        float recHP = uIArgs.Data;
        Hurt(-recHP);
    }
    public void UseLightSaber(System.Object sender, EventArgs arg)
    {
        if (touchingBody == null || touchingBody.gameObject != ((SpecialItem)sender).gameObject)
            return;
        UIArgs<float> uIArgs = arg as UIArgs<float>;
        float addTime = uIArgs.Data;
        saberTime += addTime;
        LightSaber saber=(LightSaber)sender;
    }
}

public enum PlayerMessage
{
    UIRefresh,
    FartAmount,
    Die
}