using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMove : MonoBehaviour {
	private float speed = 3.0f;
	private float gravity = -9.8f;
	private CharacterController _charController;
	private GameObject plane;
	private List<GameObject> spheres = new List<GameObject>();

	protected Joystick joystick;
	protected FixedButtonSpace buttonSpace;
	protected FixedButtonQ buttonQ;
	protected FixedButtonE buttonE;

	private AudioSource musicSource;
	public AudioClip planeClip;
	public AudioClip planeCrashClip;
	public AudioClip sphereClip;

	public GameObject sphere;

	void Start () {
		musicSource = this.gameObject.GetComponent<AudioSource>();
		joystick = FindObjectOfType<Joystick> ();
		buttonSpace = FindObjectOfType<FixedButtonSpace> ();
		buttonQ = FindObjectOfType<FixedButtonQ> ();
		buttonE = FindObjectOfType<FixedButtonE> ();
		_charController = GetComponent<CharacterController>();  //чтоб взаимодействовать со стенами
	}


	void Update () {
		float deltaX = Input.GetAxis ("Horizontal") * speed*(-1) + joystick.Horizontal * speed*(-1);
		float deltaZ = Input.GetAxis ("Vertical") * speed*(-1) + joystick.Vertical * speed * (-1);
		Vector3 movement = new Vector3 (deltaX, 0, deltaZ);
		movement = Vector3.ClampMagnitude(movement, speed); //limiting speed
		movement.y = gravity; 

		movement *= Time.deltaTime;  
		movement = transform.TransformDirection (movement); //from local to global
		_charController.Move (movement);


		if (buttonSpace.Pressed || Input.GetKeyDown ("space")) {//choosing place for mark
			musicSource.clip = planeClip;
			musicSource.Play ();
			Destroy (plane); //delete lasp plane
			Ray ray = new Ray (this.transform.position, new Vector3(0,-1f,0)); //checkong floor under
			RaycastHit hit;
			if (Physics.Raycast (ray, out hit)) {
				GameObject hitObject = hit.transform.gameObject;
				PlatformLength platform = hitObject.GetComponent<PlatformLength> ();
				if (platform != null) {
					//Instantiate (plane, new Vector3 (Mathf.Round(this.transform.position.x),0.99f,Mathf.Round(this.transform.position.z)), Quaternion.identity);
					plane = GameObject.CreatePrimitive (PrimitiveType.Plane);
					plane.transform.localScale = new Vector3 (0.1f, 1f, 0.1f);
					plane.GetComponent<Renderer>().material.color = Color.blue;
					plane.transform.position = new Vector3(Mathf.Round(this.transform.position.x),0.99f,Mathf.Round(this.transform.position.z));
				}
			} 
			buttonSpace.endSpace();
				
		}

		if ((buttonQ.Pressed || Input.GetKeyDown ("q")) && plane != null) {  // desroying marked place
			plane.GetComponent<Renderer>().material.color = Color.red;
			if (CreatingLevel.timer < 0) {
				Destroy (plane);
			} else {
				StartCoroutine (ReactPlane (plane));
			}
			buttonQ.endQ ();

		}


		if ((buttonE.Pressed || Input.GetKeyDown ("e")) && spheres.Count != 0) {  //bombing sphere
			int j = spheres.Count;
			for (int i = 0; i < j; i++) {
				StartCoroutine (ReactSphere(spheres[i]));
			}
			buttonE.endE ();
		}

		if (this.transform.position.y < -20) {
			CubeMove.destroyedCubes = 0;
			CubeMove.destroyedMistake = 0;
			CubeMove.blackFallen = 0;
			CubeMove.countAudio = 0;
			SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
		}

	}



	private IEnumerator ReactPlane(GameObject plane){
		yield return new WaitUntil (() => CreatingLevel.timer > 1.5f);

		musicSource.clip = planeCrashClip;
		musicSource.Play ();
		Ray ray = new Ray (plane.transform.position, new Vector3(0, 1f, 0));
		RaycastHit hit;
		Destroy (plane);
		if (Physics.Raycast (ray, out hit)) {
			GameObject hitObject = hit.transform.gameObject;
			CubeMove target = hitObject.GetComponent<CubeMove> ();
			if (target != null) {
				if ((int)target.kind == 2) {    //for green blocks
					spheres.Add (Instantiate (sphere, new Vector3 (Mathf.Round (target.transform.position.x), target.transform.position.y + 1f, Mathf.Round (target.transform.position.z)), Quaternion.identity));
					target.ReactToHit (true);
				} else if ((int)target.kind == 3) {  // for black blocks
					ray = new Ray (new Vector3 (target.transform.position.x, 0.99f, target.transform.position.z), new Vector3 (0, -1f, 0));
					if (Physics.Raycast (ray, out hit)) {
						hitObject = hit.transform.gameObject;
						PlatformLength platform = hitObject.GetComponent<PlatformLength> ();
						if (platform != null) {
							platform.Decrease ();
						}
					} 
					target.ReactToHit (false);
				} else {
					target.ReactToHit (true);
				}
			}
		}
	}
		

	private IEnumerator ReactSphere(GameObject bomb){
		yield return new WaitUntil (() => CreatingLevel.timer > 1.5f);

		musicSource.clip = sphereClip;
		musicSource.Play ();
		RaycastHit[] hits = Physics.SphereCastAll (bomb.transform.position, 1.1f, new Vector3(0,-1f,0), 10f);
		foreach (RaycastHit q in hits) {
			GameObject hitObject = q.transform.gameObject;
			CubeMove target = hitObject.GetComponent<CubeMove> ();
			if (target != null) {
				if ((int)target.kind == 2) {
					spheres.Add (Instantiate (sphere, new Vector3 (Mathf.Round (target.transform.position.x), target.transform.position.y + 1f, Mathf.Round (target.transform.position.z)), Quaternion.identity));
					target.ReactToHit (true);
				} else if ((int)target.kind == 3) {
					Ray ray = new Ray (new Vector3 (target.transform.position.x, 0.99f, target.transform.position.z), new Vector3 (0, -1f, 0));
					RaycastHit hit;
					if (Physics.Raycast (ray, out hit)) {
						hitObject = hit.transform.gameObject;
						PlatformLength platform = hitObject.GetComponent<PlatformLength> ();
						if (platform != null) {
							platform.Decrease ();
						}
					} 
					target.ReactToHit (false);
				} else {
					target.ReactToHit (true);
				}
			}
		}
		spheres.RemoveAt (0);
		Destroy (bomb);
	}

	public void Clean(){
		foreach (GameObject sphere in spheres) {
			Destroy (sphere);
		}
	}
}