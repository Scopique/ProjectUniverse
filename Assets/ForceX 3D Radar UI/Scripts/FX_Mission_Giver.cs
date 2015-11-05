using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FX_Mission_Giver : MonoBehaviour {
	[System.Serializable]
	public class missionGiverList{
		public FX_Mission Mission;
	}
	public List<missionGiverList> MissionGiverList = new List<missionGiverList>(0);
}
