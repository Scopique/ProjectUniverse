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

    public void Add(CommodityDataObject dataObject)
    {
        database.Add(dataObject);
    }

    public void Remove(CommodityDataObject dataObject)
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
    public CommodityDataObject GetCommodity(int index)
    {
        return database.ElementAt(index);
    }

    public void SortAlphabeticallyAtoZ()
    {
        database.Sort((x, y) => string.Compare(x.commodityName, y.commodityName));
    }


    


}
