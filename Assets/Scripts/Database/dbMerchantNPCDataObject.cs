using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class dbMerchantNPCDataObject : ScriptableObject
{
    [SerializeField]
    public List<MerchantNPCDataObject> database;

    //#####################################################################

    void OnEnable()
    {
        if (database == null)
            database = new List<MerchantNPCDataObject>();
    }

    public void Add(MerchantNPCDataObject dataObject)
    {
        database.Add(dataObject);
    }

    public void Remove(MerchantNPCDataObject dataObject)
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
    public MerchantNPCDataObject GetMerchant(int index)
    {
        return database.ElementAt(index);
    }

    public MerchantNPCDataObject GetMerchantByID(int MerchantID)
    {
        MerchantNPCDataObject mnpc = database.Find(x => x.MerchantID.Equals(MerchantID));
        if (mnpc.MerchantFirstName == string.Empty) { mnpc = new MerchantNPCDataObject(); }
        return mnpc;
    }

    public void SortAlphabeticallyAtoZ()
    {
        database.Sort((x, y) => string.Compare(x.MerchantLastName, y.MerchantLastName));
    }
}

