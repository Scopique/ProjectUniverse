using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

//http://git.burgzergarcade.net/Petey/ScriptableObject-Database-Example

public class dbCommodityDataObject : ScriptableObject {

    [SerializeField]
    public List<CommodityDataObject> database;

    //#####################################################################

    void OnEnable()
    {
        if (database == null)
            database = new List<CommodityDataObject>();
    }

    public void Add(CommodityDataObject commodity)
    {
        database.Add(commodity);
    }

    public void Remove(CommodityDataObject commodity)
    {
        database.Remove(commodity);
    }

    public void RemoveAt(int index)
    {
        database.RemoveAt(index);
    }

    public int COUNT
    {
        get { return database.Count; }
    }

    //.ElementAt() requires the System.Linq
    public CommodityDataObject Commodity(int index)
    {
        return database.ElementAt(index);
    }

    public void SortAlphabeticallyAtoZ()
    {
        database.Sort((x, y) => string.Compare(x.commodityName, y.commodityName));
    }


    


}
