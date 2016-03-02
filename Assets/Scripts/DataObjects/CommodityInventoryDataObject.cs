using UnityEngine;
using System;
using System.Collections.Generic;

[Serializable]
public class CommodityInventoryDataObject  {
    public CommodityDataObject CommodityItem;
    public int Quantity;
    public int PricePaid;

    public CommodityInventoryDataObject(CommodityDataObject CommodityItem, int Quantity, int PricePaid)
    {
        NewCommodityInventoryDataObject(CommodityItem, Quantity, PricePaid);
    }

    private void NewCommodityInventoryDataObject(
        CommodityDataObject CommodityItem,
        int Quantity,
        int PricePaid
        )
    {
        this.CommodityItem = CommodityItem;
        this.Quantity = Quantity;
        this.PricePaid = PricePaid;
    }
}
