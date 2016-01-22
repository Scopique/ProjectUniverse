using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

//http://git.burgzergarcade.net/Petey/ScriptableObject-Database-Example

public class dbSectorDataObject : ScriptableObject
{

    [SerializeField]
    public List<SectorDataObject> database;

    //#####################################################################

    void OnEnable()
    {
        if (database == null)
            database = new List<SectorDataObject>();
    }

    public void Add(SectorDataObject dataObject)
    {
        database.Add(dataObject);
    }

    public void Remove(SectorDataObject dataObject)
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
    public SectorDataObject GetSector(int index)
    {
        return database.ElementAt(index);
    }

    //.ElementAt() requires the System.Linq
    public SectorDataObject GetSectorByID(int ID)
    {
        SectorDataObject sdo = new SectorDataObject(0, "New Sector");

        if (ID > 0) { 
            sdo = database.Find(x => x.sectorID.Equals(ID));
            if (string.IsNullOrEmpty(sdo.sectorName)) { sdo = new SectorDataObject(0, "New Sector"); }
        }

        return sdo;
    }

    public void SortAlphabeticallyAtoZ()
    {
        database.Sort((x, y) => string.Compare(x.sectorName, y.sectorName));
    }





}
