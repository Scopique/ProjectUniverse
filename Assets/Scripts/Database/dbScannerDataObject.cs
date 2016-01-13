using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class dbScannerDataObject : ScriptableObject {

    [SerializeField]
    public List<ScannerDataObject> database;

    //#####################################################################

    void OnEnable()
    {
        if (database == null)
            database = new List<ScannerDataObject>();
    }

    public void Add(ScannerDataObject dataObject)
    {
        database.Add(dataObject);
    }

    public void Remove(ScannerDataObject dataObject)
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
    public ScannerDataObject GetScanner(int index)
    {
        return database.ElementAt(index);
    }

    public void SortAlphabeticallyAtoZ()
    {
        database.Sort((x, y) => string.Compare(x.name, y.name));
    }
}
