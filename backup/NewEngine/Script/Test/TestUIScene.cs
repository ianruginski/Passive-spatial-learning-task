using UnityEngine;
using System.Collections;

public class TestUIScene : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnGUI()
	{
		if(GUI.Button(new Rect(100,100,100,30),"Record"))
		{
			TestScene.IsRecordingMode = true;
			Application.LoadLevel("test");
		}

		if(GUI.Button(new Rect(100,150,100,30),"Replay"))
		{
			TestScene.IsRecordingMode = false;
			Application.LoadLevel("test");
		}
	}
}
