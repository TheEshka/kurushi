using UnityEngine.EventSystems;
using UnityEngine;

public class FixedButtonSpace : MonoBehaviour, IPointerUpHandler, IPointerDownHandler
{
	[HideInInspector]
	public bool Pressed;


	// Use this for initialization
	void Start()
	{
	}

	// Update is called once per frame
	void Update()
	{

	}

	public void OnPointerDown(PointerEventData eventData)
	{		
			Pressed = true;
	}

	public void OnPointerUp(PointerEventData eventData)
	{
		Pressed = false;
	}

	public void endSpace(){
		Pressed =false;
	}
}