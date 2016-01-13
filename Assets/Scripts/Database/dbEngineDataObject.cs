using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;


public class dbEngineDataObject : ScriptableObject
{

    [SerializeField]
    public List<EngineDataObject> database;

    //#####################################################################

    void OnEnable()
    {
        if (database == null)
            database = new List<EngineDataObject>();
    }

    public void Add(EngineDataObject dataObject)
    {
        database.Add(dataObject);
    }

    public void Remove(EngineDataObject dataObject)
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
    public EngineDataObject GetEngine(int index)
    {
        return database.ElementAt(index);
    }

    public void SortAlphabeticallyAtoZ()
    {
        database.Sort((x, y) => string.Compare(x.name, y.name));
    }
}
