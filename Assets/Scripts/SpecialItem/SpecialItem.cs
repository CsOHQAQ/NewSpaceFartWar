using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialItem : MonoBehaviour
{
    public virtual void Use()
    {
    }
    public enum ItemFunc
    {
        BeanPot,
        Cream,
        LightSaber,
    }
}
