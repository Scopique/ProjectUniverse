﻿using UnityEngine;
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

    public void SortAlphabeticallyAtoZ()
    {
        database.Sort((x, y) => string.Compare(x.sectorName, y.sectorName));
    }





}