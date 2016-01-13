using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// The save game bucket
/// </summary>
/// <remarks>
/// <para>
/// Everything that needs to be saved between sessions should find
/// a home in this meta-object.
/// </para>
/// <para>
/// This object contains buckets for variables, collections, and 
/// other objects that should be serialized to and deserialized from
/// disk. 
/// </para>
/// <para>
/// Like DataController, it's a singleton because we can only ever have
/// one instance of this object. We load data into this object, overwriting
/// any other data we previously had. 
/// </para>
/// <para>
/// One a NEW GAME, this object is populated using generation methods
/// (such as assigning inventory to commodity shops in the game)
/// </para>
/// </remarks>
[System.Serializable]
public class SaveGameDataController : MonoBehaviour
{
    public static SaveGameDataController SaveGameAccess;
    
    #region Unity Methods

    void Start()
    {
        CommodityShopInventoryList = new List<CommodityShopInventoryDataObject>();
    }

    void Awake()
    {
        if (SaveGameAccess == null)
        {
            //DontDestroyOnLoad() is handled by the 
            //  DataController GameObject this script
            //  is attached to.
            SaveGameAccess = this;
        }
    }

    #endregion

    #region Variables to Save

    public string SaveGameName;                 //File name without path
    public DateTime LastSaveDate;               //Last time game was saved; Reset on load or new; check for autosave interval
    public DateTime LastLoadDate;               //Last time game was loaded; Use for session stats
    
    #endregion

    #region Lists to Save

    //Accessed through the DataController, though it can be accessed through here.
    public List<CommodityShopInventoryDataObject> CommodityShopInventoryList;

    #endregion

}
