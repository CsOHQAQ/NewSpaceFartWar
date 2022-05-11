using App.Common;
using QxFramework.Core;
using System.Collections;
using System.Collections.Generic;
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
        CollectObject();
        GameMgr.Get<IItemManager>().RefreshAllCargoUI();
        RgstBtn();
    }
    void RgstBtn()
    {
        if (_gos.ContainsKey("GetAllButton"))
        {
            _gos["GetAllButton"].GetComponent<Button>().onClick.RemoveAllListeners();
            _gos["GetAllButton"].GetComponent<Button>().onClick.AddListener(() => {
                AudioControl.Instance.PlaySound("Tik");
                for(int i = 0; i < 5; i++)
                {
                    GameMgr.Get<IItemManager>().TakeAllItemFrom(cargo);
                }
                GameMgr.Get<IItemManager>().RefreshAllCargoUI();
            });
        }
        if (_gos.ContainsKey("PutAllButton"))
        {
            _gos["PutAllButton"].GetComponent<Button>().onClick.RemoveAllListeners();
            _gos["PutAllButton"].GetComponent<Button>().onClick.AddListener(() => {
                AudioControl.Instance.PlaySound("Tik");
                GameMgr.Get<IItemManager>().putAllItemTo(cargo);
                GameMgr.Get<IItemManager>().RefreshAllCargoUI();
            });
        }
        if (_gos.ContainsKey("ResolveButton"))
        {
            _gos["ResolveButton"].GetComponent<Button>().onClick.RemoveAllListeners();
            _gos["ResolveButton"].GetComponent<Button>().onClick.AddListener(() => {
                AudioControl.Instance.PlaySound("Tik");
                GameMgr.Get<IItemManager>().ResolvePackage(cargo[0]);
                GameMgr.Get<IItemManager>().RefreshAllCargoUI();
            });
        }
        if (_gos.ContainsKey("CloseBtn"))
        {
            _gos["CloseBtn"].GetComponent<Button>().onClick.RemoveAllListeners();
            _gos["CloseBtn"].GetComponent<Button>().onClick.AddListener(() => {
                AudioControl.Instance.PlaySound("Tik");
                UIManager.Instance.Close(this);
            });
        }
    }
    public void RefreshUI()
    {
        if (cargo[0].MaxBattery < _gos["ItemList"].transform.childCount)
        {
            for (int i = cargo[0].MaxBattery; i < _gos["ItemList"].transform.childCount; i++)
            {
                Destroy(_gos["ItemList"].transform.GetChild(i).gameObject);
            }
        }
        for (int j = _gos["ItemList"].transform.childCount; j < cargo[0].MaxBattery; j++)
        {
            GameObject go = ResourceManager.Instance.Instantiate("Prefabs/UI/ItemUIItem", _gos["ItemList"].transform);
            go.name = "ItemUIItem" + j.ToString();
        }
        for (int i = 0; i < _gos["ItemList"].transform.childCount; i++)
        {
            GameObject go = _gos["ItemList"].transform.GetChild(i).gameObject;
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
        if (_gos.ContainsKey("MoneyText"))
        {
            _gos["MoneyText"].GetComponent<Text>().text = "信用点："+GameMgr.Get<IItemManager>().GetPlayerItemData().PlayerMoney;
        }
    }
    private void OnDisable()
    {
        UIManager.Instance.Close("ItemBreifUI");
    }
}
