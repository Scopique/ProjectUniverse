﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

//TODO: Not sure how to work with the Sector ID of this. 
//  Maybe we don't assign it here, and set it up when placed...?
public class dbJumpgateDataObject : ScriptableObject
{

    [SerializeField]
    public List<JumpgateDataObject> database;

    //#####################################################################

    void OnEnable()
    {
        if (database == null)
            database = new List<JumpgateDataObject>();
    }

    public void Add(JumpgateDataObject dataObject)
    {
        database.Add(dataObject);
    }

    public void Remove(JumpgateDataObject dataObject)
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
    public JumpgateDataObject GetJumpgate(int index)
    {
        return database.ElementAt(index);
    }

    //.ElementAt() requires the System.Linq
    public JumpgateDataObject GetJumpgateByID(int ID)
    {
        JumpgateDataObject jdo = new JumpgateDataObject(0, string.Empty, 0, 0, 0, 0);
        if (ID>0)
        {
            jdo = database.Find(x => x.jumpgateID.Equals(ID));
            if (jdo.jumpgateID == null) { jdo = new JumpgateDataObject(0, string.Empty, 0, 0, 0, 0); }
        }

        return jdo;
    }

}
