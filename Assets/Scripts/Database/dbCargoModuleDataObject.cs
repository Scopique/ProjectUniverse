using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class dbCargoModuleDataObject : ScriptableObject {

    [SerializeField]
    public List<CargoDataObject> database;

    //#####################################################################

    void OnEnable()
    {
        if (database == null)
            database = new List<CargoDataObject>();
    }

    public void Add(CargoDataObject dataObject)
    {
        database.Add(dataObject);
    }

    public void Remove(CargoDataObject dataObject)
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
    public CargoDataObject GetCargoModule(int index)
    {
        return database.ElementAt(index);
    }

    public void SortAlphabeticallyAtoZ()
    {
        database.Sort((x, y) => string.Compare(x.name, y.name));
    }
}
