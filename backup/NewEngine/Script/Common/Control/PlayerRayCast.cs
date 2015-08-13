using UnityEngine;
using System.Collections;

public class PlayerRayCast : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetButtonDown("Controller A"))
		{
			DoRayCast();
		}
	}

	private void DoRayCast()
	{
		RaycastHit hit;
		Ray ray= Camera.main.ViewportPointToRay (new Vector3 (0.5f, 0.5f, 0.0f));

		if (SceneObjectMgr.Instance.DynamicObjects != null) 
		{
			foreach (DynamicObject dObj in SceneObjectMgr.Instance.DynamicObjects) 
			{
				if(dObj.gameObject.activeSelf)
				{
					if (dObj.collider.Raycast (ray, out hit, 6)) 
					{
						dObj.Interact ();
					}
				}
			}
		}
	}
}
