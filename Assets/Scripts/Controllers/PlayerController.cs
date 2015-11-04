using UnityEngine;
using System.Linq;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour
{

    #region Inspector Properties

    public static PlayerController playerController;

    [Header("Player Information")]
    public string playerName;
    public string playerPortrait;
    
    //The Ship
    [Header("Ship Components")]
    public int hullID;
    public int engineID;
    public int platingID;
    public int cargoID;

    //The Crew
    [Header("Crew Members")]
    public int pilotCrewID;                 //Hotbar slot 0
    public int tacticalCrewID;              //Hotbar slot 1
    public int engineeringCrewID;           //Hotbar slot 2
    public int securityCrewID;              //Hotbar slot 3
    public int medicalCrewID;               //Hotbar slot 4
    public int liasonCrewID;                //Hotbar slot 5

    [Header("Finances")]
    public int playerWallet;

    #region Properties for Advanced Stats
       
    //These values aren't visible in the inspector, but 
    //  are available to other scripts
    
    //Armor and structure
    public float StructuralHP { get { return currentHull.baseStrength; } }
    public float ArmorHP { get { return currentPlating.Strength; } }
    public float CurrentStructuralHP { get { return StructuralHP - currentStructuralDamage; } }
    public float CurrentArmorHP { get { return ArmorHP - currentArmorDamage; } }

    //Engine and navigation
    //TODO: Add in CREW skills to the calculation of these values
    public float CurrentThrust { get { return currentEngine.forwardThrust; } }
    public float CurrentPitch { get { return currentEngine.maneuverability.pitch; } }
    public float CurrentYaw { get { return currentEngine.maneuverability.yaw; } }
    public float CurrentRoll { get { return currentEngine.maneuverability.roll; } }

    //Cargo hold 
    public int CargoCapacity { get { return currentCargoHold.capacity; } }
    public int CurrentCargoLoad { get { return InventoryItemList.Sum(i => i.inventoryQuantity); } }

    #endregion

    #endregion

    #region Local Properties

    int currentSectorID;

    List<PlayerInventoryDataObject> InventoryItemList;

    int _hullID;
    int _platingID;
    int _engineID;
    int _cargoID;

    //These buckets hold the equipment that is currently
    //  equipped on the ship
    //Other scripts can use these if they want
    HullDataObject currentHull;
    EngineDataObject currentEngine;
    PlatingDataObject currentPlating;
    CargoDataObject currentCargoHold;

    //Tracking values: These change over time, and are usually
    //  added or subtracted to/from something else
    float currentStructuralDamage = 0.0f;
    float currentArmorDamage = 0.0f;
       
    #endregion

    #region Unity Methods
    
    void Awake()
    {
        if (playerController == null)
        {
            playerController = this;
        }

        InitializeEquipment();
    }

    // Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        CheckEquipmentUpdate();
    }

    #endregion

    #region Public Methods

    #region Inventory Management

    /// <summary>
    /// Adds an item or quantity to the cargo hold collection
    /// </summary>
    /// <param name="ItemID"></param>
    /// <param name="ItemClass"></param>
    /// <param name="ItemType"></param>
    /// <param name="Quantity"></param>
    public void AddItem(int ItemID, PlayerInventoryDataObject.INVENTORY_CLASS ItemClass, PlayerInventoryDataObject.INVENTORY_TYPE ItemType, int Quantity)
    {

        //Make sure we have room. Check the property plus the intended addition
        if (CurrentCargoLoad + Quantity < CargoCapacity)
        {
            //We have room
            PlayerInventoryDataObject pido = InventoryItemList
            .Where(i => i.inventoryObjectID.Equals(ItemID) && i.inventoryObjectClass.Equals(ItemClass) && i.inventoryObjectType.Equals(ItemType))
            .First();

            //Is this an existing item or a new item?
            if (pido != null)
            {
                //Existing. We have already made sure we can fit it, so increment current quantity
                pido.inventoryQuantity += Quantity;
            }
            else
            {
                //A new item! Need to determine if it's a commodity or equipment
                PlayerInventoryDataObject npido = new PlayerInventoryDataObject();

                switch (ItemClass)
                {
                    case PlayerInventoryDataObject.INVENTORY_CLASS.Commodity:
                        //This is a commodity, so all we need is commodity info
                        CommodityDataObject cmdo = DataController.dataController.commodityMasterList.Where(c => c.commodityID.Equals(ItemID)).First();
                        npido.inventoryObjectID = cmdo.commodityID;
                        npido.inventoryObjectClass = PlayerInventoryDataObject.INVENTORY_CLASS.Commodity;
                        npido.inventoryObjectType = ItemType;
                        npido.inventoryQuantity = Quantity;
                        break;
                    case PlayerInventoryDataObject.INVENTORY_CLASS.ShipEquipment:
                        //This is ship equipment. We need to know which DB to query for the details. 
                        //Some items are commented out because it doesn't make sense to be able to carry them around
                        //  Like a hull inside a cargo hold. 
                        //Commented these because I might need to move them elsewhere, like for station storage
                        switch (ItemType)
                        {
                            //case PlayerInventoryDataObject.INVENTORY_TYPE.Hull:
                            //    HullDataObject hdo = DataController.dataController.hullMasterList.Where(h => h.iD.Equals(ItemID)).First();
                            //    npido.inventoryObjectID = hdo.iD;
                            //    npido.inventoryObjectClass = PlayerInventoryDataObject.INVENTORY_CLASS.ShipEquipment;
                            //    npido.inventoryObjectType = ItemType;
                            //    npido.inventoryQuantity = Quantity; 
                            //    break;
                            case PlayerInventoryDataObject.INVENTORY_TYPE.Engine:
                                EngineDataObject edo = DataController.dataController.engineMasterList.Where(e => e.iD.Equals(ItemID)).First();
                                npido.inventoryObjectID = edo.iD;
                                npido.inventoryObjectClass = PlayerInventoryDataObject.INVENTORY_CLASS.ShipEquipment;
                                npido.inventoryObjectType = ItemType;
                                npido.inventoryQuantity = Quantity;
                                break;
                            //case PlayerInventoryDataObject.INVENTORY_TYPE.Cargo:
                            //    CargoDataObject crdo = DataController.dataController.cargoHoldMasterList.Where(c => c.iD.Equals(ItemID)).First();
                            //    npido.inventoryObjectID = crdo.iD;
                            //    npido.inventoryObjectClass = PlayerInventoryDataObject.INVENTORY_CLASS.ShipEquipment;
                            //    npido.inventoryObjectType = ItemType;
                            //    npido.inventoryQuantity = Quantity;
                            //    break;
                            case PlayerInventoryDataObject.INVENTORY_TYPE.Shield:
                                ShieldDataObject shdo = DataController.dataController.shieldMasterList.Where(s => s.iD.Equals(ItemID)).First();
                                npido.inventoryObjectID = shdo.iD;
                                npido.inventoryObjectClass = PlayerInventoryDataObject.INVENTORY_CLASS.ShipEquipment;
                                npido.inventoryObjectType = ItemType;
                                npido.inventoryQuantity = Quantity;
                                break;
                            case PlayerInventoryDataObject.INVENTORY_TYPE.Cannon:
                                CannonDataObject cndo = DataController.dataController.cannonMasterList.Where(c => c.iD.Equals(ItemID)).First();
                                npido.inventoryObjectID = cndo.iD;
                                npido.inventoryObjectClass = PlayerInventoryDataObject.INVENTORY_CLASS.ShipEquipment;
                                npido.inventoryObjectType = ItemType;
                                npido.inventoryQuantity = Quantity;
                                break;
                            case PlayerInventoryDataObject.INVENTORY_TYPE.MissileLauncher:
                                MissileLauncherDataObject mldo = DataController.dataController.missileLauncherMasterList.Where(m => m.iD.Equals(ItemID)).First();
                                npido.inventoryObjectID = mldo.iD;
                                npido.inventoryObjectClass = PlayerInventoryDataObject.INVENTORY_CLASS.ShipEquipment;
                                npido.inventoryObjectType = ItemType;
                                npido.inventoryQuantity = Quantity;
                                break;
                            case PlayerInventoryDataObject.INVENTORY_TYPE.Scanner:
                                ScannerDataObject scdo = DataController.dataController.scannerMasterList.Where(s => s.iD.Equals(ItemID)).First();
                                npido.inventoryObjectID = scdo.iD;
                                npido.inventoryObjectClass = PlayerInventoryDataObject.INVENTORY_CLASS.ShipEquipment;
                                npido.inventoryObjectType = ItemType;
                                npido.inventoryQuantity = Quantity;
                                break;
                            case PlayerInventoryDataObject.INVENTORY_TYPE.FighterBay:
                                FighterBayDataObject fbdo = DataController.dataController.figherBayMasterList.Where(f => f.iD.Equals(ItemID)).First();
                                npido.inventoryObjectID = fbdo.iD;
                                npido.inventoryObjectClass = PlayerInventoryDataObject.INVENTORY_CLASS.ShipEquipment;
                                npido.inventoryObjectType = ItemType;
                                npido.inventoryQuantity = Quantity;
                                break;
                            case PlayerInventoryDataObject.INVENTORY_TYPE.Plating:
                                PlatingDataObject pdo = DataController.dataController.platingMasterList.Where(p => p.iD.Equals(ItemID)).First();
                                npido.inventoryObjectID = pdo.iD;
                                npido.inventoryObjectClass = PlayerInventoryDataObject.INVENTORY_CLASS.ShipEquipment;
                                npido.inventoryObjectType = ItemType;
                                npido.inventoryQuantity = Quantity;
                                break;
                            default:
                                break;
                        }
                        break;
                    default:
                        break;
                }

                InventoryItemList.Add(npido);

            }

        }
        else
        {
            Debug.Log("Not enough room to add more items to cargo");
        }


    }

    /// <summary>
    /// Remove quantity from player inventory
    /// </summary>
    /// <param name="ItemID"></param>
    /// <param name="ItemClass"></param>
    /// <param name="ItemType"></param>
    /// <param name="Quantity"></param>
    /// <remarks>
    /// <para>
    /// Removes an item if Quantity > number we have in inventory. Otherwise, just subtract the quantity
    /// </para>
    /// <para>
    /// If we remove more than we have, we might want to stop that, but it might make sense to track that
    /// at the UI level using a slider or some other UI trick. 
    /// </para>
    /// </remarks>
    public void RemoveItem(int ItemID, PlayerInventoryDataObject.INVENTORY_CLASS ItemClass, PlayerInventoryDataObject.INVENTORY_TYPE ItemType, int Quantity)
    {
        //Do we have the item?
        PlayerInventoryDataObject pido = InventoryItemList
            .Where(i => i.inventoryObjectID.Equals(ItemID) && i.inventoryObjectClass.Equals(ItemClass) && i.inventoryObjectType.Equals(ItemType))
            .First();
        if (pido != null)
        {
            int idx = -1;

            if (pido.inventoryQuantity - Quantity <= 0)
            {
                //Subtraction removes an equal amount or more than equal amount. Remove the item
                idx = InventoryItemList.IndexOf(pido);
                InventoryItemList.RemoveAt(idx);
            }
            else
            {
                //Subtraction leaves some in inventory
                pido.inventoryQuantity -= Quantity;
            }
        }

    }

    #endregion

    #region Financial Management

    public void Credit(int CreditAmount)
    {
        playerWallet += CreditAmount;
    }


    public void Debit(int DebitAmount)
    {
        if (playerWallet - DebitAmount > 0 )
        {
            playerWallet -= DebitAmount;
        }else{
            Debug.Log("Insufficient funds");
        }
    }

    #endregion

    #region Crew Management

    public CrewMemberDataObject GetCrewInSlot(int SlotID)
    {
        CrewMemberDataObject cmdo = new CrewMemberDataObject();

        switch (SlotID)
        {
            case 0:
                cmdo = DataController.dataController.crewMasterList.Where(c => c.ID.Equals(pilotCrewID)).First();
                break;
            case 1:
                cmdo = DataController.dataController.crewMasterList.Where(c => c.ID.Equals(tacticalCrewID)).First();
                break;
            case 2:
                cmdo = DataController.dataController.crewMasterList.Where(c => c.ID.Equals(engineeringCrewID)).First();
                break;
            case 3:
                cmdo = DataController.dataController.crewMasterList.Where(c => c.ID.Equals(securityCrewID)).First();
                break;
            case 4:
                cmdo = DataController.dataController.crewMasterList.Where(c => c.ID.Equals(medicalCrewID)).First();
                break;
            case 5:
                cmdo = DataController.dataController.crewMasterList.Where(c => c.ID.Equals(liasonCrewID)).First();
                break;
            default:
                break;
        }

        return cmdo;
    }

    #endregion

    #endregion

    #region Private Methods

    private void InitializeEquipment()
    {
        CheckEquipmentUpdate();
    }

    private void GetHullValues() {
        HullDataObject hdo = DataController.dataController.hullMasterList
            .Where(h => h.iD.Equals(_hullID))
            .First();

        if (hdo!=null)
        {
            currentHull = hdo;
        }else if (currentHull == null)
        {
            Debug.Log("No hull assigned");
        }
    }
    private void GetPlatingValues() {
        PlatingDataObject pdo = DataController.dataController.platingMasterList
            .Where(p => p.iD.Equals(_platingID))
            .First();

        if (pdo!=null)
        {
            currentPlating = pdo;
        }
        else if (currentPlating == null)
        {
            Debug.Log("No plating assigned");
        }
    }
    private void GetEngineValues() {
        EngineDataObject edo = DataController.dataController.engineMasterList
            .Where(e => e.iD.Equals(_engineID))
            .First();

        if (edo != null)
        {
            currentEngine = edo;
        }
        else if (currentEngine == null)
        {
            Debug.Log("No engine assigned");
        }
    }
    private void GetCargoValues()
    {
        CargoDataObject cdo = DataController.dataController.cargoHoldMasterList
            .Where(c => c.iD.Equals(_cargoID))
            .First();

        if (cdo!=null)
        {
            currentCargoHold = cdo;
        }else if (currentCargoHold == null)
        {
            Debug.Log("No cargo hold assigned");
        }
    }

    /// <summary>
    /// Periodic check to compare the exposed IDs to the internal IDs
    /// and if they don't match, update the internal to the external 
    /// if allowed.
    /// </summary>
    /// <remarks>
    /// <para>
    /// There's no way to observe Inspector variables, so the best we
    /// can do is to store the ID's both in visible and hidden vars. 
    /// In the Update statement, we call this method to check each
    /// set of ID's. If they differ, we need to get the new external
    /// item based on the ID and pull out the relevant values for 
    /// storage into other externally available stats. We then
    /// update the internal IDs with the value of the external IDs.
    /// </para>
    /// </remarks>
    private void CheckEquipmentUpdate()
    {
        if (hullID != _hullID) {_hullID = hullID; GetHullValues(); }

        if (engineID != _engineID) { _engineID = engineID; GetEngineValues();  }

        if (cargoID != _cargoID) { _cargoID = cargoID; GetCargoValues();  }

        if (platingID != _platingID) { _platingID = platingID; GetPlatingValues();  }
    }

    #endregion

    #region Debug

    void OnGUI()
    {
        GUI.Label(new Rect(0, 20, 200, 100), "Hit Points: UNKNOWN");
    }

    #endregion
}

