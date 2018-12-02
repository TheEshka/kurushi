using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour {
	 
	//Quaternion iniRot;

	// Use this for initialization
	void Start () {
		//iniRot = transform.rotation;
	}
	
	// Update is called once per frame
	void Update () {
		
		
	}

	public void Posit(int x){
		this.transform.Translate ((float) -x / 2 + 0.5f+this.transform.position.x, 0, 0);
	}
}
