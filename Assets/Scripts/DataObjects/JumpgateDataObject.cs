using UnityEngine;
using System;
using System.Collections;

[Serializable]
public class JumpgateDataObject
{
    public int jumpgateID;
    public int sectorID;
    public Vector3 jumpgatePosition;
    public int destinationJumpgateID;
    public int fee;


    public JumpgateDataObject()
    {
        NewJumpgateDataObject(0, 0, Vector3.zero, 0, 0);
    }

    public JumpgateDataObject(
        int JumpgateID,
        int SectorID,
        Vector3 JumpgatePosition,
        int DestinationJumpgateID,
        int Fee
        )
    {
        NewJumpgateDataObject(JumpgateID, SectorID, JumpgatePosition, DestinationJumpgateID, Fee);
    }

    public JumpgateDataObject(
        int JumpgateID,
        int SectorID,
        int DestinationJumpgateID,
        int Fee
        )
    {
        NewJumpgateDataObject(JumpgateID, SectorID, Vector3.zero, DestinationJumpgateID, Fee);
    }

    private void NewJumpgateDataObject(
        int JumpgateID,
        int SectorID,
        Vector3 JumpgatePosition,
        int DestinationJumpgateID,
        int Fee
        )
    {
        this.jumpgateID = JumpgateID;
        this.sectorID = SectorID;
        this.jumpgatePosition = JumpgatePosition;
        this.destinationJumpgateID = DestinationJumpgateID;
        this.fee = Fee;
    }
}
