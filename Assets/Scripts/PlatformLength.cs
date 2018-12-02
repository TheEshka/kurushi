using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformLength : MonoBehaviour {

	public GameObject platformCube;

	public IEnumerator Enlargement(){
		yield return new WaitForSeconds (3);
		this.transform.localScale += new Vector3(0, 0, 1f);
		this.GetComponent<Renderer>().material.mainTextureScale = new Vector2 (this.transform.localScale.x,this.transform.localScale.z);
		this.transform.Translate (0, 0, 0.5f);
	}

	public void Decrease(){
		this.transform.localScale -= new Vector3(0, 0, 1f);
		this.GetComponent<Renderer>().material.mainTextureScale = new Vector2 (this.transform.localScale.x,this.transform.localScale.z);
		this.transform.Translate (0, 0, -0.5f);
		float z = /*this.transform.position.z + */this.transform.localScale.z/* / 2*/;
		for (int i = 0; i < this.transform.localScale.x; i++) {
			Instantiate (platformCube, new Vector3 (i, 0.48f, z), Quaternion.identity);
		}
	}

	public int PlatformWidth(int i){
		int j = (int) this.transform.localScale.x;
		this.transform.position = new Vector3 (((float)i - 1) / 2, this.transform.position.y, this.transform.position.z);
		this.transform.localScale = new Vector3(i,this.transform.localScale.y,this.transform.localScale.z);
		this.GetComponent<Renderer>().material.mainTextureScale = new Vector2 (this.transform.localScale.x,this.transform.localScale.z);
		return j;
	}
}
