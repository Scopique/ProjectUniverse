﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
public class dbHullDataObject : ScriptableObject
{

    [SerializeField]
    public List<HullDataObject> database;

    //#####################################################################

    void OnEnable()
    {
        if (database == null)
            database = new List<HullDataObject>();
    }

    public void Add(HullDataObject dataObject)
    {
        database.Add(dataObject);
    }

    public void Remove(HullDataObject dataObject)
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
    public HullDataObject GetHull(int index)
    {
        return database.ElementAt(index);
    }

    public void SortAlphabeticallyAtoZ()
    {
        database.Sort((x, y) => string.Compare(x.name, y.name));
    }
}
