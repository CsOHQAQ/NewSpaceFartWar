using QxFramework.Core;
using UnityEngine;
using UnityEngine.UI;

public class PackBase : UIBase
{
    [HideInInspector]
    public CargoData[] cargo;//第0个为自己，第1个为其他

    public override void OnDisplay(object args)
    {
        base.OnDisplay(args);
        cargo = args as CargoData[];
        GameMgr.Get<IItemManager>().RefreshAllCargoUI();
        RgstBtn();
    }
    void RgstBtn()
    {
        if (Find("GetAllButton") != null)
        {
            Get<Button>("GetAllButton").onClick.SetListener(() => {
            AudioControl.Instance.PlaySound("Tik");
            for (int i = 0; i < 5; i++)
            {
                GameMgr.Get<IItemManager>().TakeAllItemFrom(cargo);
            }
            GameMgr.Get<IItemManager>().RefreshAllCargoUI();
            });
        }

        if (Find("PutAllButton") != null)
        {
            Get<Button>("PutAllButton").onClick.SetListener(() => {
            AudioControl.Instance.PlaySound("Tik");
            GameMgr.Get<IItemManager>().putAllItemTo(cargo);
            GameMgr.Get<IItemManager>().RefreshAllCargoUI();
            });
        }

        if (Find("ResolveButton") != null)
        {
            Get<Button>("ResolveButton").onClick.SetListener(() =>
            {
                AudioControl.Instance.PlaySound("Tik");
                GameMgr.Get<IItemManager>().ResolvePackage(cargo[0]);
                GameMgr.Get<IItemManager>().RefreshAllCargoUI();
            });
        }
        if (Find("CloseBtn") != null)
        {
            Get<Button>("CloseBtn").onClick.SetListener(() =>
            {
                AudioControl.Instance.PlaySound("Tik");
                UIManager.Instance.Close(this);
            });
        }
    }
    public void RefreshUI()
    {
        if (cargo[0].MaxBattery < Get<Transform>("ItemList").childCount)
        {
            for (int i = cargo[0].MaxBattery; i < Get<Transform>("ItemList").childCount; i++)
            {
                Destroy(Get<Transform>("ItemList").GetChild(i).gameObject);
            }
        }
        for (int j = Get<Transform>("ItemList").childCount; j < cargo[0].MaxBattery; j++)
        {
            GameObject go = ResourceManager.Instance.Instantiate("Prefabs/UI/ItemUIItem", Get<Transform>("ItemList"));
            go.name = "ItemUIItem" + j.ToString();
        }
        for (int i = 0; i < Get<Transform>("ItemList").childCount; i++)
        {
            GameObject go = Get<Transform>("ItemList").GetChild(i).gameObject;
            go.GetComponentInChildren<DragOnPic>().cargo = cargo[0];
            if(cargo.Length > 1)
            {
                go.GetComponentInChildren<DragOnPic>().othercargo = cargo[1];
            }
            go.GetComponentInChildren<DragOnPic>().PosID = i;
            go.GetComponentInChildren<DragOnPic>().itemPile = null;
            go.GetComponentInChildren<DragOnPic>().CanDrag = false;
            foreach (ItemPile itm in cargo[0].itemPiles)
            {
                if (itm.CurrentPosID == i)
                {
                    go.transform.Find("Pivot/ItmImg").GetComponent<Image>().sprite = ResourceManager.Instance.Load<Sprite>("Texture/Property/" + itm.item.ItemImg);
                    go.transform.Find("Pivot/Material").GetComponent<Image>().enabled =(itm.item.ItemType == ItemType.Material);
                    go.transform.Find("Pivot/Count").GetComponent<Text>().text = itm.CurrentPile.ToString();
                    go.GetComponentInChildren<DragOnPic>().itemPile = itm;
                    go.GetComponentInChildren<DragOnPic>().CanDrag = true;
                }
            }
        }
    }
    private void Update()
    {
        if (Find("MoneyText") != null)
        {
            Get<Text>("MoneyText").text = "信用点：" + GameMgr.Get<IItemManager>().GetPlayerItemData().PlayerMoney;
        }
    }
    private void OnDisable()
    {
        UIManager.Instance.Close("ItemBreifUI");
    }
}
