//#pragma strict
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class FX_Faction_Mgr : MonoBehaviour {

	public enum _fmSetup {SetupFactions, SetPlayerFactionAndRelations, SetGlobalFactionRelations}
	public _fmSetup FMSetup;

	//var PGB : Texture;
	public string[] Factions = new string[2] {"Red Team", "Blue Team"}; // Array that stores the names of the factions
	public int[] FactionID = new int[2]; // Array that stores the FactionID numbers for all factions
	public Dictionary<int,float> FactionRelations = new Dictionary<int,float>(); //Dictionary that holds the faction relation values
	public float[] StartRelations = new float[2]; // Array that stores the start relatoinship values and applies them to the dictionary and is applied at awake. If you have a proper method of saving and loading information then you will only want to run this once. After that you will want to save and load the dictionary and other values manually.
	public float[] PlayerRelations = new float[2]; // Array that stores the Players start relatoinship values with the factions.
	public int[] HFS = new int[2] {-300, 300}; // Array that stores the values for Hostile and Friendly cutoff values
	
	public int PlayerFaction; // The Players faction number
	public int PlayerFactionID; // The Players FactionID number

	bool Initialize;//Initialize = true; //Set this value to true to prevent the dictionary from being rebuilt and resetting it's values if you are using a proper method of saving and loading information.

	void Awake(){
		if(!Initialize){	
			if(PlayerFaction > Factions.Length - 1){
				PlayerFaction = 0;
			}
			CreateFactionList();
		}
		Initialize = true;
	}
	
	void CreateFactionList(){
		//Assign the FactionID numbers for each faction
		FactionID = new int[Factions.Length];
		for (int i = 0; i < FactionID.Length; i++){
			if(i == 0){
				FactionID[i] = 1;
			}else{
				FactionID[i] = (int)Mathf.Pow(2, i);
			}
		}
		
		//Set the Player FactionID based on the current faction selection
		PlayerFactionID = FactionID[PlayerFaction];
		
		//Assign the Key values to the dictionary and the corrosponding starting relationship values.
		int[] cnt = new int[3];
		
		while(cnt[0] < (StartRelations.Length)){
			for(int x = 0; x < ((FactionID.Length - 1) - cnt[2]); x++){
				int a = cnt[2];
				int b = ((cnt[2] + cnt[1]) + 1);
				int c = (FactionID[a] + FactionID[b]);
				FactionRelations.Add(c, StartRelations[cnt[0]]);
				
				if(cnt[0] < (StartRelations.Length)){
					cnt[0]++;
				}
				cnt[1]++;
			}
			cnt[1] = 0;
			cnt[2]++;
		}
	}
	
	public void GetRelations(int SomeFaction){
		Debug.Log("Example for displaying all relations for one faction. In this case the players faction. \n Disable this example in the FX_Faction_Mgr.GetRelations");
		
		for(int i = 0; i < FactionID.Length; i++){
			if(i != SomeFaction){
				float ThisRelations = FactionRelations[(FactionID[SomeFaction] + FactionID[i])];
				string SomeFactionName = Factions[SomeFaction].ToString();
				string CompName = Factions[i].ToString();
				
				Debug.Log(SomeFactionName + " ----> " + CompName + "  :  " + ThisRelations);
			}
		}
	}
	

}
