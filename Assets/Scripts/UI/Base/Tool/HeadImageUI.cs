using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 设置头像
/// </summary>
public class HeadImageUI : MonoBehaviour
{
    public void SetImage(string path)
    {
        transform.GetComponentInChildren<Image>().sprite = Resources.Load<Sprite>(path);
    }

    public void SetImage(Sprite spriteImage)
    {
        transform.GetComponentInChildren<Image>().sprite = spriteImage;
    }

    public void SetImage(Sprite spriteImage,GameObject go)
    {
        go.GetComponent<Image>().sprite = spriteImage;
    }
}