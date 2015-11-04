using UnityEngine;
using System.Collections;

public class RouteDataObject  {

    public string DestinationName { get; set; }
    public string DestinationTag { get; set; }
    public Vector3 Destination { get; set; }


    public RouteDataObject()
    {
        NewRouteDataObject(string.Empty, string.Empty, Vector3.zero);
    }

    public RouteDataObject(string DestinationName, string DestinationTag, Vector3 Destination)
    {
        NewRouteDataObject(DestinationName, DestinationTag, Destination);
    }

    private void NewRouteDataObject(string DestinationName, string DestinationTag, Vector3 Destination)
    {
        this.DestinationName = DestinationName;
        this.DestinationTag = DestinationTag;
        this.Destination = Destination;
    }
}
