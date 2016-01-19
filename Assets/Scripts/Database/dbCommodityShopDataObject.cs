using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;


public class dbCommodityShopDataObject : ScriptableObject {
    [SerializeField]
    public List<CommodityShopDataObject> database;

    //#####################################################################

    void OnEnable()
    {
        if (database == null)
            database = new List<CommodityShopDataObject>();
    }

    public void Add(CommodityShopDataObject dataObject)
    {
        database.Add(dataObject);
    }

    public void Remove(CommodityShopDataObject dataObject)
    {
        database.Remove(dataObject);
    }

    public void RemoveAt(int index)
    {
        database.RemoveAt(index);
    }

    public int Count
    {
        get { return database.Count; }
    }

    //.ElementAt() requires the System.Linq
    public CommodityShopDataObject GetCommodityShop(int index)
    {
        return database.ElementAt(index);
    }

    public CommodityShopDataObject GetCommodityShopByStation(int StationID)
    {
        CommodityShopDataObject csdo = database.Find(x => x.stationID.Equals(StationID));
        if (csdo == null) { csdo = new CommodityShopDataObject(); }
        return csdo;
    }

    public void SortAlphabeticallyAtoZ()
    {
        database.Sort((x, y) => string.Compare(x.shopName, y.shopName));
    }
}
