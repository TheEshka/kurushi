using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeMove : MonoBehaviour {
	public static int destroyedCubes = 0;
	public static int destroyedMistake = 0;
	public static int blackFallen = 0;
	public static int countAudio = 0;

	public enum CubeKind
	{
		Basic = 1,
		Green = 2,
		Black = 3,
		Platform = 4
	}
	public CubeKind kind;
	public float gravity =0f;

	private bool destroyed = false; // чтобы один кубик не считался два раза




	void Start () {

	}



	void Update () {
		float time = CreatingLevel.timer;
		if ((time <= 0) && (time > -8)) {
			if (gravity == 0 && this.transform.position.y <=1.5f) {
				this.transform.Translate (0, Time.deltaTime * 1.03f / 2, 0, Space.World);
			} else {
				this.transform.Translate (0, gravity*Time.deltaTime, 0, Space.World);
			}
		} else if ((time > 0) &&(time <= 0.375)) {
			if (gravity == 0) {
				this.transform.Rotate (120 * Time.deltaTime, 0, 0);
				this.transform.Translate (0, Time.deltaTime * Mathf.Sqrt (2) / 3, Time.deltaTime *1.33f, Space.World);
			} else {
				this.transform.Translate (0, gravity*Time.deltaTime, 0, Space.World);
			}
		} else if ((time > 0) &&(time <= 0.75)) {
			if (gravity == 0) {
				this.transform.Rotate (120 * Time.deltaTime, 0, 0);
				this.transform.Translate (0, (-1) * Time.deltaTime * Mathf.Sqrt (2) / 3, Time.deltaTime *1.33f, Space.World);
			} else {
				this.transform.Translate (0, gravity*Time.deltaTime, 0, Space.World);
			}
		} else if ((time > 0) &&(time <= 1.5)) {
			this.transform.position = new Vector3(this.transform.position.x,Mathf.Floor (this.transform.position.y)+0.5f,Mathf.Round (this.transform.position.z));
			this.transform.eulerAngles = new Vector3(/*Mathf.Round (this.transform.rotation.eulerAngles.x / 90) * 9*/0,0,0);
			Ray ray = new Ray (this.transform.position, new Vector3(0,-1f,0));
			RaycastHit hit;
			if ((Physics.Raycast (ray, out hit)) == false && (gravity ==0)) {
				gravity = -9.8f;
				countAudio++;
			}
		} else  {
			this.transform.Translate (0, gravity*Time.deltaTime, 0, Space.World);
		}

		if (this.transform.position.y < -20) {
			if ((int) this.kind == 4) {	
				
			}else if ((int) this.kind == 3) {					
				blackFallen++;
				destroyedMistake++;
			}else {
				destroyedMistake++;
			}
			Destroy (this.gameObject);
		}
	}

	public void ReactToHit(bool truth){			
		if ((truth == true) && (destroyed == false)) {
			destroyedCubes++;
			countAudio++;
		} else if ((truth == false) && (destroyed == false)) {
			destroyedMistake++;
			countAudio++;
		}
		this.destroyed = true;
		Destroy (this.gameObject);
	}

	public void cubeFall(){
		this.gravity = -9.8f;
	}
}
