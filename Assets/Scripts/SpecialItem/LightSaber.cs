using QxFramework.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightSaber : SpecialItem
{
    public float buffTime=10f;
    public override void Use()
    {
        base.Use();
        MessageManager.Instance.Get<ItemFunc>().DispatchMessage(ItemFunc.LightSaber,this,new UIArgs<float>(buffTime));
        
    }
}
