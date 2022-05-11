using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBase
{
    public override void Init()
    {
        base.Init();
    }
    public override void ControlMove()
    {
        var joy = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        realTimeStatus.MoveDir = joy;
        realTimeStatus.CurrentMoveSpeed = baseStatus.MaxMoveSpeed;
        base.ControlMove();
    }

    public Transform _cameraOrth;
    public override void ControlRotate()
    {
        var t = this.transform;
        realTimeStatus.AimDummyPos = t.InverseTransformPoint(_cameraOrth.GetComponent<Camera>().ScreenToWorldPoint(Input.mousePosition));
        base.ControlRotate();
    }
}
