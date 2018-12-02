using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class CreatingLevel : MonoBehaviour {
	public static float timer;
	public  static bool endWave;

	private int countToDestroy;
	private int generalCount;
	private int level;
	private Level sv = new Level();

	private GUIStyle guiStyle = new GUIStyle();

	public GameObject basicCube;
	public GameObject greenCube;
	public GameObject blackCube;

	private AudioSource musicSource;

	//string path;	



	void Start () {
		timer = -8f;
		endWave = false;
		level = 1;

		//Cursor.lockState = CursorLockMode.Locked;
		//Cursor.visible = false;

		/*sv.wave = "3 1 1 1 1 2 3 1/2 1 1 1 2 1 3 1/3 3 1 2 1 1 2 3";
		path = Application.dataPath + "/Resources/level3.json"; 
		string json = JsonUtility.ToJson(sv);
		File.WriteAllText (path, json);
		sv = null;*/

		StartCoroutine(Create ());

		musicSource = this.gameObject.GetComponent<AudioSource>();

	}

	void Update () {
		if (timer <= 0.75f) {
			timer += Time.deltaTime;
		} else if (timer < 1.5f) {
			timer = 1.5f;
			if (CubeMove.countAudio < generalCount) musicSource.Play ();
		} else if ((timer < 3.5f)  && (endWave == false)) {
			timer += Time.deltaTime;
		} else {
			timer = 0;
		}



		if ((countToDestroy == CubeMove.destroyedCubes) && (endWave == false))
		{
			endWave = true;

		} else if (generalCount == CubeMove.destroyedCubes+CubeMove.destroyedMistake) 
		{
			timer = -8f;

			if (endWave == true) {
				level++;
				if (CubeMove.blackFallen == CubeMove.destroyedMistake) {
					PlatformLength platform = this.GetComponent<PlatformLength> ();
					StartCoroutine(platform.Enlargement ());
				}
			} else {
				PlatformLength platform = this.GetComponent<PlatformLength> ();
				platform.Decrease ();
			}
			endWave = false;
			CubeMove.destroyedCubes = 0;
			CubeMove.destroyedMistake = 0;
			CubeMove.blackFallen = 0;
			CubeMove.countAudio = 0;
			Debug.Log (level);
			StartCoroutine(Create ());
		}
	}

	void OnGUI(){
		guiStyle.fontSize = 20;
		GUI.Label (new Rect (10, 10, 150, 150), "Destroyed " + CubeMove.destroyedCubes, guiStyle);
		GUI.Label (new Rect (10, 30, 150, 150), "From " + countToDestroy, guiStyle);
		GUI.Label (new Rect (10, 50, 150, 150), "Platform length " + this.transform.localScale.z, guiStyle);
		GUI.Label (new Rect (10, 70, 150, 150), "Mistaken " + CubeMove.destroyedMistake, guiStyle);
		GUI.Label (new Rect (10, 90, 150, 150), "Audio " + CubeMove.countAudio, guiStyle);
		GUI.Label (new Rect (10, 110, 150, 150), "Timer " + timer, guiStyle);
	}


	public IEnumerator Create(){
		countToDestroy = 0;
		int x=0;
		int z=0;
		TextAsset asset = Resources.Load ("level"+level) as TextAsset;
		sv = JsonUtility.FromJson<Level> (asset.text);
		string[] lines = sv.wave.Split(new char[] { '/' });

		x = (lines [0].Length + 1) / 2;
		generalCount = lines.Length * x;
		countToDestroy = generalCount;

		SimpleCharacterControl player = FindObjectOfType<SimpleCharacterControl>();
		player.Clean ();
		//CameraScript cam = player.GetComponentInChildren<CameraScript>();
		CameraScript cam = Camera.main.GetComponent<CameraScript>();


		PlatformLength platform = GetComponent<PlatformLength> ();
		if (platform != null) {				
			if (x != platform.PlatformWidth (x)) {				
				cam.Posit (x);
			}
		}

		x = 0;

		yield return new WaitUntil (() => timer > (-lines.Length -1f) );
		foreach (String line in lines) 
		{
			x=0;
			Debug.Log(line);
			string[] blocks = line.Split(new char[] { ' ' });
			foreach(String block in blocks)
			{
				switch (block)
				{
				case "1":
					Instantiate (basicCube, new Vector3 (x, 0.47f, z), Quaternion.identity);
					countToDestroy++;
					break;
				case "2":
					Instantiate (greenCube, new Vector3 (x, 0.47f, z), Quaternion.identity);
					countToDestroy++;
					break;
				case "3":
					Instantiate (blackCube, new Vector3 (x, 0.47f, z), Quaternion.identity);
					break;
				}
				x++;
			}
			yield return new WaitForSeconds (0.5f);
			z++;

		}
		countToDestroy -= generalCount;
	}
}