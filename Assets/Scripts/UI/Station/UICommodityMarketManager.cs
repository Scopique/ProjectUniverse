using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class UICommodityMarketManager : MonoBehaviour
{

    #region Inspector Properties

    [Header("UI References")]
    public GameObject contentPanel;             //Needed to load items into the grid

    [Header("Prefabs")]
    public GameObject commodityGridItem;        //What to use to populate the grid

    #endregion

    #region Unity Methods
    
    // Use this for initialization
	void Start () {
       
	}
	
	// Update is called once per frame
	void Update () {

    }

    void OnEnable()
    {
        PopulateGrid();
    }

    #endregion

    #region Public Methods
    
    #endregion

    #region Private Methods
    
    /// <summary>
    /// Populate the commodity grid based on the current station.
    /// Current station can be had from the PlayerController.
    /// We'll ASSUME it's correct and not a leftover value.
    /// This is called when the panel opens
    /// </summary>
    private void PopulateGrid()
    {
        int stationID = PlayerController.playerController.stationID;
        if (stationID > 0) { 
            List<CommodityShopInventoryDataObject> items = DataController.dataController.GetShopInventory(PlayerController.playerController.stationID);

            foreach(CommodityShopInventoryDataObject item in items)
            {
                GameObject newGridItem = (GameObject)Instantiate(commodityGridItem);
                UICommodityGridItemManager gim = newGridItem.GetComponent<UICommodityGridItemManager>();
                //gim.commodityImage.overrideSprite = null;          //Need to Resource.Load this
                //gim.commodityBuySell.overrideSprite = null;        //Also need to Resource.Load this, but we might want to keep this one around longer
                gim.commodityQuantity.text = item.commodityQuantity.ToString();
                gim.commodityPrice.text = "$100";                      //TODO: Price needs access to Market to calculate the price. Component on SectorController
                newGridItem.transform.SetParent(contentPanel.transform);
            }
            
        }

    }

    #endregion
}
