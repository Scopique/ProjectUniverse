using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class FX_Mission : MonoBehaviour {

	FX_3DRadar_Mgr FX3DRM;

	public enum nextCondition {TimeDelay, CompleteObjective};
	public enum objectiveType {None, Destroy, Discover, SeekAndDestroy, GetItem};
	public enum missionNAVType {LinearProgression, DisplayAll, Selectable};
	public missionNAVType MissionNAVType;

	public GameObject MissionGiver;
	public string MissionName;

	public string DescriptionText;
	public AudioClip DescriptionAudio;

	public string AcceptText;
	public AudioClip AcceptAudio;

	public string DeclineText;
	public AudioClip DeclineAudio;

	public string CompleteText;
	public AudioClip CompleteAudio;

	public bool Complete;

	public GameObject NextMission;

	[System.Serializable]
	public class missionNAVList{
		public Transform NAVPoint;
		public string NAVName;
		public float NAVArrivalDistance = 20.0f;
		//Conditions
		public nextCondition NextCondition;
		//Condition Time Delay
		public int Delay;
		//Condition Objective
		public List<objectiveType> ObjectiveType = new List<objectiveType>(0);
		public List<Transform> Objectives = new List<Transform>(0);
		//Dialog
		public string NAVTextIn;
		public string NAVTextOut;
		public AudioClip NAVAudioIn;
		public AudioClip NAVAudioOut;
		public bool DialogInPlayed;
		public bool DialogOutPlayed;
	}

	public List<missionNAVList> MissionNAVList = new List<missionNAVList>(1);

	void Start(){
		FX3DRM = GameObject.Find("_GameMgr").GetComponent<FX_3DRadar_Mgr>();
	}

	public void UpdateMission(){
		//UpdateLinearProgression();
	}

	void  UpdateLinearProgression(){
		if(MissionNAVList.Count > 0){
			float distance = (FX3DRM.PlayerPos - MissionNAVList[0].NAVPoint.position).sqrMagnitude;

			if(distance <= (MissionNAVList[0].NAVArrivalDistance * MissionNAVList[0].NAVArrivalDistance)){
				if(!MissionNAVList[0].DialogInPlayed){
					PlayInDialog(0);
				}
			}

			if(!MissionNAVList[0].DialogOutPlayed && (MissionNAVList[0].Objectives.Count) == 0){
				PlayOutDialog(0);
				MissionNAVList[0].NAVPoint.GetComponent<FX_3DRadar_RID>().DestroyThis();
				MissionNAVList.RemoveAt(0);
				MissionNAVList[0].NAVPoint.GetComponent<FX_3DRadar_RID>().IsActiveNAV[0] = true;
			}
		}
	}

	void PlayInDialog(int i){
		Debug.Log("here");
		MissionNAVList[i].DialogInPlayed = true;
	}

	void PlayOutDialog(int i){
		Debug.Log("Objective Complete");
		MissionNAVList[i].DialogOutPlayed = true;
	}

	public void SetObjectiveStatus(){
		for(int i = 0; i < MissionNAVList.Count; i++){
			for(int a = 0; a < MissionNAVList[i].Objectives.Count; a++){
				if(MissionNAVList[i].Objectives[a] && MissionNAVList[i].Objectives[a].GetComponent<FX_3DRadar_RID>()){
					FX_3DRadar_RID RID = MissionNAVList[i].Objectives[a].GetComponent<FX_3DRadar_RID>();
					RID.IsObjective[0] = true;
					RID.ObjectiveType = MissionNAVList[i].ObjectiveType[a];
				}
			}
		}
	}

	public void ObjectiveDestroyed(Transform This){
		for(int i = 0; i < MissionNAVList.Count; i++){
			for(int a = 0; a < MissionNAVList[i].Objectives.Count; a++){
				if(This == MissionNAVList[i].Objectives[a]){
					MissionNAVList[i].Objectives.RemoveAt(a);
					MissionNAVList[i].ObjectiveType.RemoveAt(a);
					break;
				}
			}
		}
	}
}


