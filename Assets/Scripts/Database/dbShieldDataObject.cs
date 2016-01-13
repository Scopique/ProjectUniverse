using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;


public class dbShieldDataObject : ScriptableObject {

    [SerializeField]
    public List<ShieldDataObject> database;

    //#####################################################################

    void OnEnable()
    {
        if (database == null)
            database = new List<ShieldDataObject>();
    }

    public void Add(ShieldDataObject dataObject)
    {
        database.Add(dataObject);
    }

    public void Remove(ShieldDataObject dataObject)
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
    public ShieldDataObject GetShield(int index)
    {
        return database.ElementAt(index);
    }

    public void SortAlphabeticallyAtoZ()
    {
        database.Sort((x, y) => string.Compare(x.name, y.name));
    }
}
