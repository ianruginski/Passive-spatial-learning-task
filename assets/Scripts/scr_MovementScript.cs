using UnityEngine;
using System.Collections;

public class scr_MovementScript : MonoBehaviour {
	CharacterController cont;
	Vector3 moveDirection;
	public GameObject cam;
	// Use this for initialization
	void Start () {
		cont=(CharacterController)GetComponent(typeof(CharacterController));

	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKey(KeyCode.W))
		{
			moveDirection=Vector3.Normalize(cam.transform.forward);
			cont.SimpleMove(10*moveDirection);
		}
		if(Input.GetKey(KeyCode.S))
		{
			moveDirection=Vector3.Normalize(cam.transform.forward);
			cont.SimpleMove(-10*moveDirection);
		}
	}
}
