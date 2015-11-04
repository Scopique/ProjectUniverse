using UnityEngine;
using System;
using System.Collections;

[Serializable]
public class JumpgateDataObject
{
    public int jumpgateID;
    public int sectorID;
    public Vector3 jumpgatePosition;
    public int destinationSectorID;
    public int fee;


    public JumpgateDataObject()
    {
        NewJumpgateDataObject(0, 0, Vector3.zero, 0, 0);
    }

    public JumpgateDataObject(
        int JumpgateID,
        int SectorID,
        Vector3 JumpgatePosition,
        int DestinationSectorID,
        int Fee
        )
    {
        NewJumpgateDataObject(JumpgateID, SectorID, JumpgatePosition, DestinationSectorID, Fee);
    }

    public JumpgateDataObject(
        int JumpgateID,
        int SectorID,
        int DestinationSectorID,
        int Fee
        )
    {
        NewJumpgateDataObject(JumpgateID, SectorID, Vector3.zero, DestinationSectorID, Fee);
    }

    private void NewJumpgateDataObject(
        int JumpgateID,
        int SectorID,
        Vector3 JumpgatePosition,
        int DestinationSectorID,
        int Fee
        )
    {
        this.jumpgateID = JumpgateID;
        this.sectorID = SectorID;
        this.jumpgatePosition = JumpgatePosition;
        this.destinationSectorID = DestinationSectorID;
        this.fee = Fee;
    }
}
