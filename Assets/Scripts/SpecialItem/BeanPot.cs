using QxFramework.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeanPot : SpecialItem
{
    public float buffTime = 20f;
    public override void Use()
    {
        base.Use();
        MessageManager.Instance.Get<ItemFunc>().DispatchMessage(ItemFunc.BeanPot, this, new UIArgs<float>(buffTime));
        ObjectPool.Recycle(this);
    }
}
