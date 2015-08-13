using UnityEngine;
using System.Collections;

public class DynamicObject : MonoBehaviour {

	public delegate void InteractDelegate(DynamicObject dynamicObject);

	public event InteractDelegate OnInteract;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void Interact()
	{
		//tracking
		if(ActionTracker.IsTracking)
		{
			InteractAction action = new InteractAction();
			action.Record(this);
			ActionTracker.TrackAction(action);
		}

		//
		if (OnInteract != null)
			OnInteract.Invoke (this);

		Debug.Log(string.Format("interacted with {0}",this.name));
	}
}