using UnityEngine;
using System.Collections.Generic;

public class FX_Mission_Mgr : MonoBehaviour {
	[System.Serializable]
	public class missionList{
		public FX_Mission Mission;
		public bool Active;
	}
	public List<missionList> MissionList = new List<missionList>(0);

	void Update(){
		for(int i = 0; i < MissionList.Count; i++){
			if(MissionList[i].Active){
				MissionList[i].Mission.UpdateMission();
			}
		}
	}

	public void SetMissionActive(int m){
		for(int i = 0; i < MissionList.Count; i++){
			MissionList[i].Active = false;
			MissionList[i].Mission.MissionNAVList[0].NAVPoint.GetComponent<FX_3DRadar_RID>().IsActiveNAV[0] = true;
		}
		MissionList[m].Active = true;
	}

	public void SetMissionInactive(int m){
		MissionList[m].Active = false;
		for(int i = 0; i < MissionList.Count; i++){
			MissionList[i].Active = false;
			for(int a = 0; a < MissionList[i].Mission.MissionNAVList.Count; a++){
				MissionList[i].Mission.MissionNAVList[a].NAVPoint.GetComponent<FX_3DRadar_RID>().IsActiveNAV[0] = false;
			}
		}
	}
}
