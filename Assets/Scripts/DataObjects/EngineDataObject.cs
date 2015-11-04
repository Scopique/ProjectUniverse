using System;
using System.Collections.Generic;

[Serializable]
public class EngineDataObject: _BaseHardwareDataObject
{
    public float forwardThrust;
    public Maneuverability maneuverability;

    public EngineDataObject()
    {
        NewEngineDataObject(0,string.Empty,string.Empty,0,string.Empty, 0, 0.0f, new Maneuverability());
    }

    public EngineDataObject(int ID, string Name, string HardwareCode, int HardwareClass, string Portrait, long BaseCost, float ForwardThrust, Maneuverability Maneuverability)
    {
        NewEngineDataObject(ID,Name,HardwareCode,HardwareClass,Portrait,BaseCost, ForwardThrust, Maneuverability);
    }

    private void NewEngineDataObject(int ID, string Name, string HardwareCode, int HardwareClass, string Portrait, long BaseCost, float ForwardThrust, Maneuverability Maneuverability)
    {
        this.iD = ID;
        this.name = Name;
        this.hardwareCode = HardwareCode;
        this.hardwareClass = HardwareClass;
        this.portrait = Portrait;
        this.baseCost = BaseCost;

        this.forwardThrust = ForwardThrust;
        this.maneuverability = Maneuverability;
    }

}

public class Maneuverability
{
    public float pitch;
    public float yaw;
    public float roll;

    public Maneuverability()
    {
        NewManeuverability(0.0f, 0.0f, 0.0f);
    }

    public Maneuverability(float Pitch, float Yaw, float Roll)
    {
        NewManeuverability(Pitch, Yaw, Roll);
    }

    private void NewManeuverability(float Pitch, float Yaw, float Roll)
    {
        this.pitch = Pitch;
        this.yaw = Yaw;
        this.roll = Roll;
    }
}
