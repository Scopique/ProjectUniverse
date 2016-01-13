using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

//TODO: Crew database editor needs access to Crew Skills
public class dbCrewDataObject : ScriptableObject
{

    [SerializeField]
    public List<CrewMemberDataObject> database;

    //#####################################################################

    void OnEnable()
    {
        if (database == null)
            database = new List<CrewMemberDataObject>();
    }

    public void Add(CrewMemberDataObject dataObject)
    {
        database.Add(dataObject);
    }

    public void Remove(CrewMemberDataObject dataObject)
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
    public CrewMemberDataObject GetCrewMember(int index)
    {
        return database.ElementAt(index);
    }

    public void SortAlphabeticallyAtoZ()
    {
        database.Sort((x, y) => string.Compare(x.Name, y.Name));
    }
}
