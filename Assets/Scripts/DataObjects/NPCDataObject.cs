using UnityEngine;
using System;
using System.Collections.Generic;

[Serializable]
public class NPCDataObject
{
    public int NPCID;
    public string NPCName;
    public string NPCShipName;
    public string NPCFaction;


    public NPCDataObject()
    {
        NewNPCDataObject(0, string.Empty, string.Empty, string.Empty);
    }

    public NPCDataObject(
        int NPCID,
        string NPCName,
        string NPCShipName,
        string NPCFaction)
    {
        NewNPCDataObject(NPCID, NPCName, NPCShipName, NPCFaction);
    }

    private void NewNPCDataObject(
        int NPCID,
        string NPCName,
        string NPCShipName,
        string NPCFaction)
    {
        this.NPCID = NPCID;
        this.NPCName = NPCName;
        this.NPCShipName = NPCShipName;
        this.NPCFaction = NPCFaction;
    }
}

public class DestinationObject
{
    public GameObject destinationObject;
    public Vector3 destinationPos;

    public DestinationObject()
    {
        NewDestinationObject(null);
    }

    public DestinationObject(GameObject Destination)
    {
        NewDestinationObject(Destination);
    }

    private void NewDestinationObject(GameObject destinationObject)
    {
        this.destinationObject = destinationObject;
        this.destinationPos = destinationObject.transform.position;
    }
}