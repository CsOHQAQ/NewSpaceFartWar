using QxFramework.Core;
using UnityEngine.UI;

public class PackTestUI : UIBase
{
    public override void OnDisplay(object args)
    {
        base.OnDisplay(args);
        RgstBtn();
    }
    void RgstBtn()
    {
        Get<Button>("PackBtn").onClick.RemoveAllListeners();
        Get<Button>("PackBtn").onClick.AddListener(() => {
            CloseAllPack();
            UIManager.Instance.Open("Cargo_BaseUI", args: new CargoData[] { GameMgr.Get<IItemManager>().GetPlayerItemData().PlayerCargo });
            GameMgr.Get<IItemManager>().RefreshAllCargoUI();
        });
        Get<Button>("GroundPackBtn").onClick.RemoveAllListeners();
        Get<Button>("GroundPackBtn").onClick.AddListener(() => {
            CloseAllPack();
            UIManager.Instance.Open("Cargo_BaseUI", args: new CargoData[] { GameMgr.Get<IItemManager>().GetPlayerItemData().PlayerCargo, GameMgr.Get<IItemManager>().GetPlayerItemData().GroundCargo });
            UIManager.Instance.Open("Cargo_GroundUI", args: new CargoData[] { GameMgr.Get<IItemManager>().GetPlayerItemData().GroundCargo, GameMgr.Get<IItemManager>().GetPlayerItemData().PlayerCargo });
            GameMgr.Get<IItemManager>().RefreshAllCargoUI();
        });
        Get<Button>("ShopBtn").onClick.RemoveAllListeners();
        Get<Button>("ShopBtn").onClick.AddListener(() => {
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
