using UnityEngine;
using System;
using System.Collections.Generic;


[Serializable]
public class CommodityDataObject
{
    public enum COMMODITYCLASS
    {
        Common,
        Luxury,
        Food,
        Minerals,
        Medical,
        Military,
        Industrial
    }

    public int commodityID;
    public string commodityName;
    public int commodityBasePrice;
    public int commodityCurrentPrice;
    public int commodityQuantity;
    public COMMODITYCLASS commodityClass;

    /*
     * Other props, like legality, size/weight, etc
     */


    public CommodityDataObject()
    {
        NewCommodityDataObject(
            0,
            string.Empty,
            0,
            0,
            0,
            COMMODITYCLASS.Common
            );
    }

    public CommodityDataObject(
        int CommodityID,
        string CommodityName,
        int CommodityBasePrice,
        int CommodityCurrentPrice,
        int CommodityQuantity,
        COMMODITYCLASS CommodityClass
        )
    {
        NewCommodityDataObject(
            CommodityID,
            CommodityName,
            CommodityBasePrice,
            CommodityCurrentPrice,
            CommodityQuantity,
            CommodityClass
            );
    }

    private void NewCommodityDataObject(
        int CommodityID,
        string CommodityName,
        int CommodityBasePrice,
        int CommodityCurrentPrice,
        int CommodityQuantity,
        COMMODITYCLASS CommodityClass
        )
    {
        this.commodityID = CommodityID;
        this.commodityName = CommodityName;
        this.commodityBasePrice = CommodityBasePrice;
        this.commodityCurrentPrice = CommodityCurrentPrice;
        this.commodityQuantity = CommodityQuantity;
        this.commodityClass = CommodityClass;
    }
}

