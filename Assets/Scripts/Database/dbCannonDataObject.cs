using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class dbCannonDataObject : ScriptableObject {

    [SerializeField]
    public List<CannonDataObject> database;

    //#####################################################################

    void OnEnable()
    {
        if (database == null)
            database = new List<CannonDataObject>();
    }

    public void Add(CannonDataObject dataObject)
    {
        database.Add(dataObject);
    }

    public void Remove(CannonDataObject dataObject)
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
    public CannonDataObject GetCannon(int index)
    {
        return database.ElementAt(index);
    }

    public void SortAlphabeticallyAtoZ()
    {
        database.Sort((x, y) => string.Compare(x.name, y.name));
    }
}
