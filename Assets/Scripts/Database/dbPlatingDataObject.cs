using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class dbPlatingDataObject : ScriptableObject {

    [SerializeField]
    public List<PlatingDataObject> database;

    //#####################################################################

    void OnEnable()
    {
        if (database == null)
            database = new List<PlatingDataObject>();
    }

    public void Add(PlatingDataObject dataObject)
    {
        database.Add(dataObject);
    }

    public void Remove(PlatingDataObject dataObject)
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
    public PlatingDataObject GetPlating(int index)
    {
        return database.ElementAt(index);
    }

    public void SortAlphabeticallyAtoZ()
    {
        database.Sort((x, y) => string.Compare(x.name, y.name));
    }
}
