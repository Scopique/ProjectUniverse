using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public class MarketController : MonoBehaviour {

    #region Inspector Vars

    public float perItemMax = 1500.0f;

    #endregion


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

    }

    #region Big Bang Setup

/// <summary>
    /// Will be loaded during initial game setup; Here only for testing purposes
    /// </summary>
    internal void LoadCommodityShopInventories()
    {
        //Only happens on a NEW GAME
        //loop through all stores
        foreach (CommodityShopDataObject csd in DataController.DataAccess.commodityShopMasterList)
        {

            List<CommodityShopInventoryDataObject> tempShopInventory = new List<CommodityShopInventoryDataObject>();

            //create an inventory item for each master item record we have
            //  and assign it a quantity
            foreach (CommodityDataObject cd in DataController.DataAccess.commodityMasterList)
            {
                int even = (int)perItemMax / DataController.DataAccess.commodityMasterList.Count;
                int share = even - (DataController.DataAccess.GetRandomInt(0, 100) / 10);
                string shopBuysOrSells = DataController.DataAccess.GetRandomInt(0, 50) > 25 ? "B" : "S";

                CommodityShopInventoryDataObject cid = new CommodityShopInventoryDataObject(csd.stationID, cd.commodityID, share, 0, shopBuysOrSells);

                tempShopInventory.Add(cid);
            }

            //move some of the inventory quantity around so it's not so uniform.
            //TODO: Check perItemMin and keep it under perItemMax
            foreach (CommodityShopInventoryDataObject cid in tempShopInventory)
            {
                int rndAmt = DataController.DataAccess.GetRandomInt(1, cid.commodityQuantity);
                int rndItem = DataController.DataAccess.GetRandomInt(0, tempShopInventory.Count - 1);

                tempShopInventory.ElementAt(rndItem).commodityQuantity += rndAmt;
                cid.commodityQuantity -= rndAmt;
            }

            //The DataController property is a pass-through to the SaveGame object
            DataController.DataAccess.CommodityShopInventoryList.AddRange(tempShopInventory);
        }
    }

    #endregion  
}
