using App.Common;
using QxFramework.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PackTestUI : UIBase
{
    public override void OnDisplay(object args)
    {
        base.OnDisplay(args);
        CollectObject();
        RgstBtn();
    }
    void RgstBtn()
    {
        _gos["PackBtn"].GetComponent<Button>().onClick.RemoveAllListeners();
        _gos["PackBtn"].GetComponent<Button>().onClick.AddListener(() => {
            CloseAllPack();
            UIManager.Instance.Open("Cargo_BaseUI", args: new CargoData[] { GameMgr.Get<IItemManager>().GetPlayerItemData().PlayerCargo });
            GameMgr.Get<IItemManager>().RefreshAllCargoUI();
        });
        _gos["GroundPackBtn"].GetComponent<Button>().onClick.RemoveAllListeners();
        _gos["GroundPackBtn"].GetComponent<Button>().onClick.AddListener(() => {
            CloseAllPack();
            UIManager.Instance.Open("Cargo_BaseUI", args: new CargoData[] { GameMgr.Get<IItemManager>().GetPlayerItemData().PlayerCargo, GameMgr.Get<IItemManager>().GetPlayerItemData().GroundCargo });
            UIManager.Instance.Open("Cargo_GroundUI", args: new CargoData[] { GameMgr.Get<IItemManager>().GetPlayerItemData().GroundCargo, GameMgr.Get<IItemManager>().GetPlayerItemData().PlayerCargo });
            GameMgr.Get<IItemManager>().RefreshAllCargoUI();
        });
        _gos["ShopBtn"].GetComponent<Button>().onClick.RemoveAllListeners();
        _gos["ShopBtn"].GetComponent<Button>().onClick.AddListener(() => {
            CloseAllPack();
            UIManager.Instance.Open("Cargo_BaseUI", args: new CargoData[] { GameMgr.Get<IItemManager>().GetPlayerItemData().PlayerCargo, GameMgr.Get<IItemManager>().GetPlayerItemData().ShopCargo });
            UIManager.Instance.Open("Cargo_ShopUI", args: new CargoData[] { GameMgr.Get<IItemManager>().GetPlayerItemData().ShopCargo, GameMgr.Get<IItemManager>().GetPlayerItemData().PlayerCargo });
            GameMgr.Get<IItemManager>().RefreshAllCargoUI();
        });
    }
    void CloseAllPack()
    {
        PackBase[] packBases = FindObjectsOfType<PackBase>();
        for(int i=0;i< packBases.Length; i++)
        {
            UIManager.Instance.Close(packBases[i]);
        }
    }
}
