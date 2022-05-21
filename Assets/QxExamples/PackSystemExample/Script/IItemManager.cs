using System;
using System.Collections.Generic;

public interface IItemManager
{
    void RefeshBattery();
    //直接获得物品
    bool AddItem(int Id, int Count, CargoData cargo);
    //直接移除物品
    bool RemoveItem(int PosID, int Count, CargoData[] cargo);
    //按id移除物品
    bool RemoveItemByID(int ItemID, int Count);
    bool RemoveItemByID(int ItemID, int Count, CargoData[] cargo);
    //检查物品是否足够
    bool CheckItemEnough(int ItemID, int Count, CargoData[] cargo);
    bool CheckItemEnough(int ItemID, int Count);
    int GetItemCount(int ItemID, CargoData[] cargo);
    int GetItemCount(int ItemID);
    //是否有空格
    bool CheckEmptyPile(CargoData[] cargo, out CargoData emptyCargo);

    //移除物品到一个仓库
    bool RemoveItemToCargo(int PosID, int Count, CargoData cargo, CargoData cargoTo);
    //从一个仓库拿取所有
    bool TakeAllItemFrom(CargoData[] cargoAll);
    bool putAllItemTo(CargoData[] cargoAll);
    //整理仓库
    void ResolvePackage(CargoData cargo);
    //交换两个仓库的一格物品
    void SwitchPile(CargoData cargoFrom, ItemPile fromPile, CargoData cargoTo, ItemPile toPile);
    //交换两个仓库的一格物品（目标为空格）
    void SwitchPile(CargoData cargoFrom, ItemPile fromPile, CargoData cargoTo, int toPosID);

    Item GetItemStatus(int Id);
    ItemData GetPlayerItemData();

    void RefreshAllCargoUI();
}
