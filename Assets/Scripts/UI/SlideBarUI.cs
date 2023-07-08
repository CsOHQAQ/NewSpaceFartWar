using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using QxFramework.Core;

public class SlideBarUI : UIBase
{
    private Image fillImg;
    public override void OnDisplay(object args)
    {
        base.OnDisplay(args);
        fillImg = Get<Image>("Fill");
    }
    
    public void Refresh(float value,Vector2 pos)
    {
        fillImg.fillAmount= value;
        fillImg.transform.position= Camera.main.WorldToScreenPoint(pos);
    }

    protected override void OnClose()
    {
        base.OnClose();
    }
}
