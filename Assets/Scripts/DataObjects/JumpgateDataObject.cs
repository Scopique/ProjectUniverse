using UnityEngine;
using System;
using System.Collections;

[Serializable]
public class JumpgateDataObject
{
    public int jumpgateID;
    public string jumpgateName;
    public int sectorID;
    public Vector3 jumpgatePosition;
    public int destinationSectorID;
    public int destinationJumpgateID;
    public int fee;


    public JumpgateDataObject()
    {
        NewJumpgateDataObject(0, string.Empty, 0, Vector3.zero, 0, 0, 0);
    }

    public JumpgateDataObject(
        int JumpgateID,
        string JumpgateName,
        int SectorID,
        Vector3 JumpgatePosition,
        int DestinationSectorID,
        int DestinationJumpgateID,
        int Fee
        )
    {
        NewJumpgateDataObject(JumpgateID, JumpgateName, SectorID, JumpgatePosition, DestinationSectorID, DestinationJumpgateID, Fee);
    }

    public JumpgateDataObject(
        int JumpgateID,
        string JumpgateName,
        int SectorID,
        int DestinationSectorID,
        int DestinationJumpgateID,
        int Fee
        )
    {
        NewJumpgateDataObject(JumpgateID,JumpgateName, SectorID, Vector3.zero, DestinationSectorID, DestinationJumpgateID, Fee);
    }

    private void NewJumpgateDataObject(
        int JumpgateID,
        string JumpgateName,
        int SectorID,
        Vector3 JumpgatePosition,
        int DestinationSectorID,
        int DestinationJumpgateID,
        int Fee
        )
    {
        this.jumpgateID = JumpgateID;
        this.jumpgateName = JumpgateName;
        this.sectorID = SectorID;
        this.jumpgatePosition = JumpgatePosition;
        this.destinationSectorID = DestinationSectorID;
        this.destinationJumpgateID = DestinationJumpgateID;
        this.fee = Fee;
    }
}
