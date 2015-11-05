using UnityEngine;
using System.Collections;

public class MouseLook : MonoBehaviour {
	public Transform Player;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		Player.Rotate(new Vector3(-Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse X"), 0) * Time.deltaTime * 100);
	}
}
