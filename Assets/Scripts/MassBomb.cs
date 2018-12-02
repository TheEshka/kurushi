using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MassBomb : MonoBehaviour {
	public float timer = 0f;


	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (timer <= 0.75) {
			//this.transform.Rotate (0, 180 * Time.deltaTime, 0);
			this.transform.Translate (0, Time.deltaTime * 0.5f, 0);
			timer += Time.deltaTime;
		} else if (timer <= 1.5) {
			//this.transform.Rotate (0, 180 * Time.deltaTime, 0);
			this.transform.Translate (0, Time.deltaTime * (-0.5f), 0);
			timer += Time.deltaTime;
		}else{
			timer = 0;
		}
	}
}
