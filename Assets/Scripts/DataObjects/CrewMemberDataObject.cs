using System;
using System.Collections.Generic;

[Serializable]
public class CrewMemberDataObject
{
    public int ID;
    public string Name;
    public string Gender;
    public string Portrait;
    public string Biography;
    public int PayPerPeriod;
    public int CurrentLevel;
    public float CurrentExperience;
    public int HealthTotal;
    public int HealthCurrent;
    public int HomeStationID;
    public CrewMemberSkillsDataObject Skills;

    public CrewMemberDataObject()
    {
        NewCrewMemberDataObject(
            0,
            string.Empty,
            string.Empty,
            string.Empty,
            string.Empty,
            0,
            0,
            0.0f,
            0,
            0,
            0,
            new CrewMemberSkillsDataObject());
    }



    public CrewMemberDataObject(
        int ID,
        string Name,
        string Gender,
        string Portrait,
        string Biography,
        int PayPerPeriod,
        int CurrentLevel,
        float CurrentExperience,
        int HealthTotal,
        int HealthCurrent,
        int HomeStationID,
        CrewMemberSkillsDataObject Skills)
    {
        NewCrewMemberDataObject(
            ID,
            Name,
            Gender,
            Portrait,
            Biography,
            PayPerPeriod,
            CurrentLevel,
            CurrentExperience,
            HealthTotal,
            HealthCurrent,
            HomeStationID,
            Skills);
    }

    private void NewCrewMemberDataObject(
        int ID,
        string Name,
        string Gender,
        string Portrait,
        string Biography,
        int PayPerPeriod,
        int CurrentLevel,
        float CurrentExperience,
        int HealthTotal,
        int HealthCurrent,
        int HomeStationID,
        CrewMemberSkillsDataObject Skills)
    {
        this.ID = ID;
        this.Name = Name;
        this.Gender = Gender;
        this.Portrait = Portrait;
        this.Biography = Biography;
        this.Skills = Skills;
        this.PayPerPeriod = PayPerPeriod;
        this.CurrentLevel = CurrentLevel;
        this.CurrentExperience = CurrentExperience;
        this.HealthTotal = HealthTotal;
        this.HealthCurrent = HealthCurrent;
        this.HomeStationID = HomeStationID;
    }
}

public class CrewMemberSkillsDataObject
{
    public float Bargaining;        //Getting better prices
    public float Cannons;           //Using cannons
    public float Medical;           //Patching up the crew
    public float Missiles;          //Using missiles
    public float Networking;        //Making contacts with NPCs
    public float Piloting;          //Flying and evading
    public float Repair;            //Fixing the ship
    public float Scanners;          //Using scanners


    public CrewMemberSkillsDataObject()
    {
        NewCrewMemberSkillsDataObject(0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f);
    }

    public CrewMemberSkillsDataObject(float Bargaining, float Cannons, float Medical, float Missiles, float Networking, float Piloting, float Repair, float Scanners)
    {
        NewCrewMemberSkillsDataObject(Bargaining, Cannons, Medical, Missiles, Networking, Piloting, Repair, Scanners);
    }

    private void NewCrewMemberSkillsDataObject(float Bargaining, float Cannons, float Medical, float Missiles, float Networking, float Piloting, float Repair, float Scanners)
    {
        this.Bargaining = Bargaining;
        this.Cannons = Cannons;
        this.Medical = Medical;
        this.Missiles = Missiles;
        this.Networking = Networking;
        this.Piloting = Piloting;
        this.Repair = Repair;
        this.Scanners = Scanners;
    }
}