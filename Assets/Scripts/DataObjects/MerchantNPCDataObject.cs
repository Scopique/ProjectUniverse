using UnityEngine;
using System;
using System.Collections.Generic;

[Serializable]
public class MerchantNPCDataObject
{
    #region Master Data Properties
    
    //Demographics
    public int MerchantID;
    public string MerchantFirstName;
    public string MerchantLastName;
    public string MerchantGender;
    public int MerchantFaction;

    //Defines the ship for cargo, speed and defense (MDS)
    public int HullID;
    public int EngineID;
    public int CargoID;
    public int ShieldID;
    public int PlatingID;

    //If they died, don't resurrect them (State)
    public bool IsDestroyed;

    #endregion

    #region State Data Properties

    //Where we headed? 0 if nowhere.
    public int DestinationSectorID;
    //Where we headed SPECIFICALLY? 0 if nowhere.
    public int DestinationStationID;
    //Overwritten before each undocking
    public List<int> CurrentRoute;
    //Updated on tick
    public int CurrentSectorID;
    //Updated on Dock and Undock (0)
    public int CurrentStationID;
    //Updated from CurrentSectorID before reset
    public int LastSectorID;
    //Updated after CurrentSectorID is reset
    public int NextSectorID;
    //Updated when NextSectorID is reset
    public int NextJumpgateID;

    //Tick modifier. Set on new game
    public float MerchantTickModifier = 0.0f;
    //Last time the tick was triggered for this object
    public DateTime LastTickTime = DateTime.MinValue;

    //Current cargo
    public PlayerInventoryDataObject Inventory;
    //NPCs will only buy one good, so this will let us know what they paid for it.
    //Calc of profit will need to count cargo and use remote prices to compare
    //  to this value to see if it's worth the trip.
    public int InventoryPurchaseTotal;

    public int Wallet = 10000;

    #endregion

}
