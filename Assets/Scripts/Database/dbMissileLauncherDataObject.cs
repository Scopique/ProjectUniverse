using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class dbMissileLauncherDataObject : ScriptableObject
{

    [SerializeField]
    public List<MissileLauncherDataObject> database;

    //#####################################################################

    void OnEnable()
    {
        if (database == null)
            database = new List<MissileLauncherDataObject>();
    }

    public void Add(MissileLauncherDataObject dataObject)
    {
        database.Add(dataObject);
    }

    public void Remove(MissileLauncherDataObject dataObject)
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
    public MissileLauncherDataObject GetMissileLauncher(int index)
    {
        return database.ElementAt(index);
    }

    public void SortAlphabeticallyAtoZ()
    {
        database.Sort((x, y) => string.Compare(x.name, y.name));
    }
}
