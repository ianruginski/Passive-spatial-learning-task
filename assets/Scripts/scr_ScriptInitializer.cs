using UnityEngine;
using System.Collections;

public class scr_ScriptInitializer : MonoBehaviour {
	public Camera PlayerCam;
	private GameObject TextObject;
	public GameObject Instructions;
	private scr_StartingText subjectInfo;
	private scr_RecordPosition recordingScript;
	private scr_LoadExperiment loadingScript;

	void Awake()
	{
		GameObject TextObject=GameObject.Find("TextObject");
		subjectInfo=(scr_StartingText)TextObject.GetComponent(typeof(scr_StartingText));
		//Initialize some variables based on whether this is a Practice session or a Trial session
		if(subjectInfo.Record)
		{
			Instructions.SetActive(false);
			recordingScript=(scr_RecordPosition)gameObject.GetComponent(typeof(scr_RecordPosition));
			recordingScript.enabled=true;
			loadingScript=(scr_LoadExperiment)gameObject.GetComponent(typeof(scr_LoadExperiment));
			loadingScript.enabled=false;
		}

		else
		{
			scr_InstructionScreenClickCallback InstructionsScript=(scr_InstructionScreenClickCallback)Instructions.GetComponent(typeof(scr_InstructionScreenClickCallback));
			InstructionsScript.StartInstructions=true;

			recordingScript=(scr_RecordPosition)gameObject.GetComponent(typeof(scr_RecordPosition));
			recordingScript.enabled=false;
			loadingScript=(scr_LoadExperiment)gameObject.GetComponent(typeof(scr_LoadExperiment));
			loadingScript.InverseSampleRate=int.Parse (subjectInfo.PlayBackSpeed);
			//loadingScript.enabled=true;

			MouseLook lookScript=(MouseLook)GetComponent (typeof(MouseLook));
			lookScript.enabled=false;
			CharacterMotor Motor=(CharacterMotor)GetComponent (typeof(CharacterMotor));
			Motor.enabled=false;
			FPSInputController FPSscript=(FPSInputController)GetComponent (typeof(FPSInputController));
			FPSscript.enabled=false;
			MouseLook cameraScript=(MouseLook)PlayerCam.GetComponent(typeof(MouseLook));
			cameraScript.enabled=false;
			loadingScript.SpawnRocks=subjectInfo.EnableRocks;
		}

		if( subjectInfo.TopView)
		{
			PlayerCam.transform.position=new Vector3(PlayerCam.transform.position.x,PlayerCam.transform.position.y+10.0f,PlayerCam.transform.position.z-5.0f);
			PlayerCam.transform.eulerAngles=new Vector3(45.0f,0.0f,0.0f);
		}

	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
