using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class MonoBase : MonoBehaviour
{
    [SerializeField]
    public BaseStatus baseStatus;
    public Transform DirectDummy;

    [HideInInspector]
    public RealTimeStatus realTimeStatus;

    public class RealTimeStatus
    {
        //Hp
        public int CurrentHp;
        //Aim
        public Vector3 AimDummyPos;
        public Quaternion AimRealDir;
        //Move
        public Vector2 MoveDir;
        public float CurrentMoveSpeed;
    }
    [Serializable]
    public class BaseStatus
    {        
        //Hp
        public int MaxHp = 100;
        //Aim
        public float RotateSpeed = 1.0f;
        //Move
        public float MaxMoveSpeed = 1.0f;
    }

    public void OnEnable()
    {
        Init();
    }
    public virtual void Init()
    {
        realTimeStatus = new RealTimeStatus();
        realTimeStatus.CurrentHp = baseStatus.MaxHp;
        realTimeStatus.AimDummyPos = Vector3.zero;
        realTimeStatus.AimRealDir = Quaternion.Euler(0, 0, 0);
        realTimeStatus.MoveDir = Vector2.zero;
        realTimeStatus.CurrentMoveSpeed = 0;
    }
    public void Update()
    {
        UpdateStatus();
    }
    public void UpdateStatus()
    {
        ControlMove();
        ControlRotate();
    }

    public virtual void ControlMove()
    {
        realTimeStatus.CurrentMoveSpeed = Mathf.Max(0, realTimeStatus.CurrentMoveSpeed);
        realTimeStatus.CurrentMoveSpeed = Mathf.Min(realTimeStatus.CurrentMoveSpeed, baseStatus.MaxMoveSpeed);
        GetComponent<Rigidbody2D>().AddForce(realTimeStatus.MoveDir.normalized * realTimeStatus.CurrentMoveSpeed);
    }

    private float lightAngle;
    public virtual void ControlRotate()
    {
        var t = this.transform;
        lightAngle = Mathf.Atan2(realTimeStatus.AimDummyPos.y, realTimeStatus.AimDummyPos.x) * Mathf.Rad2Deg;
        realTimeStatus.AimRealDir = Quaternion.AngleAxis(RealLightAngle(lightAngle), Vector3.forward);
        DirectDummy.rotation = realTimeStatus.AimRealDir; 
    }
    private float RealLightAngle(float lightAngle)
    {
        float CurrentAngle = DirectDummy.eulerAngles.z;
        return Mathf.LerpAngle(CurrentAngle, lightAngle, Time.deltaTime * baseStatus.RotateSpeed);
    }

}
