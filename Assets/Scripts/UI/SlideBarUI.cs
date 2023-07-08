using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using QxFramework.Core;
using System;

public class SlideBarUI : UIBase
{

    private Image fillImg;
    public override void OnDisplay(object args)
    {
        base.OnDisplay(args);
        fillImg = Get<Image>("Fill");
        
    }
    
    public void Refresh(float value)
    {
        fillImg.fillAmount= value;
    }

    protected override void OnClose()
    {
        base.OnClose();
    }
    
}


public class UIArgs<T> : System.EventArgs
{
    public T Data;

    public UIArgs(T data)
    {
        Data = data;
    }
}