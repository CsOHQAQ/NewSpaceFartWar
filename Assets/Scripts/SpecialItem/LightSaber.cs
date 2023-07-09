using QxFramework.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightSaber : SpecialItem
{
    public float buffTime=10f;
    public float pushForce = 7f;
    private Collider2D col;
    public override void Use()
    {
        base.Use();
        MessageManager.Instance.Get<ItemFunc>().DispatchMessage(ItemFunc.LightSaber,this,new UIArgs<float>(buffTime));
        col=GetComponent<BoxCollider2D>();

    }

    public void Dash(Vector2 dashDirection,PlayerController player)
    {
        ContactFilter2D contactFilter = new ContactFilter2D();
        contactFilter.useTriggers = true;
        Collider2D[] colliders=new Collider2D[10];
        int count= col.OverlapCollider(contactFilter, colliders);
        for(int i=0;i<count;i++) 
        {
            var obj = colliders[i];
            if (obj.gameObject == player.gameObject)
            {
                continue;
            }
            Rigidbody2D rig=obj.GetComponent<Rigidbody2D>();
            if(rig != null)
            {
                rig.AddForce(dashDirection.normalized * pushForce);
            }
        }

    }
}
