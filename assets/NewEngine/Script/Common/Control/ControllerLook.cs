using UnityEngine;
using System.Collections;
public class ControllerLook : MonoBehaviour {

	public float sensitivityX = 15F;
	public float sensitivityY = 15F;
	
	public float minimumX = -360F;
	public float maximumX = 360F;
	
	public float minimumY = -60F;
	public float maximumY = 60F;
	
	float rotationY = 0F;
	
	void Update ()
	{

			float rotationX = transform.localEulerAngles.y + Input.GetAxis("Controller X") * sensitivityX * Time.deltaTime * 100.0f;
			
			rotationY += Input.GetAxis("Controller Y") * sensitivityY * Time.deltaTime * 100.0f;
			rotationY = Mathf.Clamp (rotationY, minimumY, maximumY);
			
			transform.localEulerAngles = new Vector3(-rotationY, rotationX, 0);
	}
	
	void Start ()
	{
		// Make the rigid body not change rotation
		if (GetComponent<Rigidbody>())
			GetComponent<Rigidbody>().freezeRotation = true;
	}
}