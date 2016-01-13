using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class dbFighterBayDataObject : ScriptableObject
{

    [SerializeField]
    public List<FighterBayDataObject> database;

    //#####################################################################

    void OnEnable()
    {
        if (database == null)
            database = new List<FighterBayDataObject>();
    }

    public void Add(FighterBayDataObject dataObject)
    {
        database.Add(dataObject);
    }

    public void Remove(FighterBayDataObject dataObject)
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
    public FighterBayDataObject GetFighterBay(int index)
    {
        return database.ElementAt(index);
    }

    public void SortAlphabeticallyAtoZ()
    {
        database.Sort((x, y) => string.Compare(x.name, y.name));
    }
}
