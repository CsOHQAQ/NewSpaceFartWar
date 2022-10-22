using QxFramework.Core;
using UnityEngine.UI;

public class ItemBreifUI : UIBase
{
    public override void OnDisplay(object args)
    {
        base.OnDisplay(args);
        ShowWord(args as string[]);
    }
    private void ShowWord(string[] Words)
    {
        Get<Text>("TitleText").text = Words[0];
        Get<Text>("ContentText").text = Words[1];
    }
}
