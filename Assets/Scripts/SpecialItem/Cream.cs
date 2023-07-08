using QxFramework.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cream : SpecialItem
{
    public float recoverValue = 50f;
    public override void Use()
    {
        base.Use();
        MessageManager.Instance.Get<ItemFunc>().DispatchMessage(ItemFunc.Cream, this, new UIArgs<float>(recoverValue));
        ObjectPool.Recycle(this);
    }
}
