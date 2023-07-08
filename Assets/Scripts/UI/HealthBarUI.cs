using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using QxFramework.Core;
using System;

public class HealthBarUI : UIBase
{
    public override void OnDisplay(object args)
    {
        base.OnDisplay(args);
        MessageManager.Instance.Get<PlayerMessage>().RegisterHandler(PlayerMessage.UIRefresh,Refresh);
    }
    protected override void OnClose()
    {
        base.OnClose();
    }

    private void Refresh(System.Object sender, EventArgs arg)
    {
        PlayerController player = (PlayerController)sender;
        UIArgs<float> hpPercent = (UIArgs<float>)arg;
        
        Get<Image>($"P{player.playerIndex}Fill").fillAmount = hpPercent.Data;
    }
}
