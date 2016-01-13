using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class dbStationDataObject : ScriptableObject {

    [SerializeField]
    public List<StationDataObject> database;

    //#####################################################################

    void OnEnable()
    {
        if (database == null)
            database = new List<StationDataObject>();
    }

    public void Add(StationDataObject dataObject)
    {
        database.Add(dataObject);
    }

    public void Remove(StationDataObject dataObject)
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
    public StationDataObject GetStation(int index)
    {
        return database.ElementAt(index);
    }

    public void SortAlphabeticallyAtoZ()
    {
        database.Sort((x, y) => string.Compare(x.stationName, y.stationName));
    }
}
