using UnityEngine;
using System;
using System.Collections.Generic;


[Serializable]
public class CommodityShopDataObject
{
    public int stationID;
    public string shopName;
    public string shopDescription;
    public string shopkeeperPortait;

    public CommodityShopDataObject()
    {
        NewCommodityShopDataObject(
            0,
            string.Empty,
            string.Empty,
            string.Empty
            );
    }

    public CommodityShopDataObject(
        int StationID,
        string ShopName,
        string ShopDescription,
        string ShopkeeperPortrait
        )
    {
        NewCommodityShopDataObject(
            StationID,
            ShopName,
            ShopDescription,
            ShopkeeperPortrait
            );
    }

    private void NewCommodityShopDataObject(
        int StationID,
        string ShopName,
        string ShopDescription,
        string ShopkeeperPortrait
        )
    {
        this.stationID = StationID;
        this.shopName = ShopName;
        this.shopDescription = ShopDescription;
        this.shopkeeperPortait = ShopkeeperPortrait;
    }
}

[Serializable]
public class CommodityShopInventoryDataObject
{
    public int stationID;
    public int commodityID;
    public int commodityQuantity;
    public int commodityLastQuantity;
    public string shopBuysOrSells;      //Either B (if the shop buys) or S (if the shop sells)



    public CommodityShopInventoryDataObject()
    {
        NewCommodityShopInventoryDataObject(0, 0, 0, 0, string.Empty);
    }

    public CommodityShopInventoryDataObject(int StationID, int CommodityID, int CommodityQuantity, int CommodityLastQuantity, string ShopBuysOrSells)
    {
        NewCommodityShopInventoryDataObject(StationID, CommodityID, CommodityQuantity, CommodityLastQuantity, ShopBuysOrSells);
    }

    private void NewCommodityShopInventoryDataObject(int StationID, int CommodityID, int CommodityQuantity, int CommodityLastQuantity, string ShopBuysOrSells)
    {
        this.stationID = StationID;
        this.commodityID = CommodityID;
        this.commodityQuantity = CommodityQuantity;
        this.commodityLastQuantity = CommodityLastQuantity;
        this.shopBuysOrSells = ShopBuysOrSells;
    }
}

[Serializable]
public class PlayerInventoryDataObject
{

    public enum INVENTORY_CLASS
    {
        Commodity,
        ShipEquipment
    }

    public enum INVENTORY_TYPE
    {
        Commodity,
        Hull,
        Engine,
        Cargo,
        Shield,
        Cannon,
        MissileLauncher,
        Scanner,
        FighterBay,
        Plating,
        CannonAmmo,
        MissileAmmo
    }

    public int inventoryObjectID;
    public int inventoryQuantity;
    public INVENTORY_CLASS inventoryObjectClass;
    public INVENTORY_TYPE inventoryObjectType;


    public PlayerInventoryDataObject()
    {
        NewPlayerInventoryDataObject(0, 0, INVENTORY_CLASS.Commodity, INVENTORY_TYPE.Commodity);
    }

    public PlayerInventoryDataObject(int InventoryObjectID, int InventoryQuantity, INVENTORY_CLASS InventoryObjectClass, INVENTORY_TYPE InventoryObjectType)
    {
        NewPlayerInventoryDataObject(InventoryObjectID, InventoryQuantity, InventoryObjectClass, InventoryObjectType);
    }

    private void NewPlayerInventoryDataObject(int InventoryObjectID, int InventoryQuantity, INVENTORY_CLASS InventoryObjectClass, INVENTORY_TYPE InventoryObjectType)
    {
        this.inventoryObjectID = InventoryObjectID;
        this.inventoryQuantity = InventoryQuantity;
        this.inventoryObjectClass = InventoryObjectClass;
        this.inventoryObjectType = InventoryObjectType;

    }
}
