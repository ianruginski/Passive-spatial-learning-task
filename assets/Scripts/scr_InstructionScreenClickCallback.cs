using UnityEngine;
using System.Collections;
public class scr_InstructionScreenClickCallback : MonoBehaviour {
	public GameObject Player;
	public scr_RecordPosition RecordingScript;
	public scr_LoadExperiment LoadingScript;
	public GameObject cam;
	public GameObject skyCam;
	public GameObject[] Instructions;

	public int CurrentSet;
	public bool ReturnPlayerToStart;
	public bool StartInstructions;
	// Use this for initialization
	void Start () {
		CurrentSet = 0;
	}
	
	// Update is called once per frame
	void Update () {
	
		if(Input.GetKeyUp(KeyCode.KeypadPlus))
	    {
			if(StartInstructions)
			{
				StartInstructions=false;
				LoadingScript.enabled=true;
				
				
			}
			if(CurrentSet==1 || CurrentSet==2)
			{
				RecordingScript.enabled=true;
				MouseLook lookScript=(MouseLook)Player.GetComponent (typeof(MouseLook));
				lookScript.enabled=true;
				CharacterMotor Motor=(CharacterMotor)Player.GetComponent (typeof(CharacterMotor));
				Motor.enabled=true;
				FPSInputController FPSscript=(FPSInputController)Player.GetComponent (typeof(FPSInputController));
				FPSscript.enabled=true;
				MouseLook cameraScript=(MouseLook)cam.GetComponent(typeof(MouseLook));
				cameraScript.enabled=true;

			}
			if(CurrentSet==3)
			{
				transform.parent.parent.gameObject.SetActive(false);
				skyCam.SetActive(true);
			}
			

			gameObject.SetActive(false);


		}
	}

	

	public void EnableText(int SetNumber)
	{
		print ("NoHome");
		CurrentSet = SetNumber;
		if(SetNumber==1)
		{
			Instructions[1].SetActive(true);
			Instructions[0].SetActive(false);
			MouseLook lookScript=(MouseLook)Player.GetComponent (typeof(MouseLook));
			lookScript.enabled=false;
			CharacterMotor Motor=(CharacterMotor)Player.GetComponent (typeof(CharacterMotor));
			Motor.enabled=false;
			FPSInputController FPSscript=(FPSInputController)Player.GetComponent (typeof(FPSInputController));
			FPSscript.enabled=false;
			MouseLook cameraScript=(MouseLook)cam.GetComponent(typeof(MouseLook));
			cameraScript.enabled=false;
		}
		if(SetNumber==2)
		{
			print ("GOHOME");
			Instructions[1].SetActive(false);
			Instructions[2].SetActive(true);
			scr_RecordPosition RecordingScript=(scr_RecordPosition)Player.GetComponent(typeof(scr_RecordPosition));
			print (Player.transform.position);
			print (RecordingScript.StartPosition);	
			Player.transform.position=RecordingScript.StartPosition;

			MouseLook lookScript=(MouseLook)Player.GetComponent (typeof(MouseLook));
			lookScript.enabled=false;
			CharacterMotor Motor=(CharacterMotor)Player.GetComponent (typeof(CharacterMotor));
			Motor.enabled=false;
			FPSInputController FPSscript=(FPSInputController)Player.GetComponent (typeof(FPSInputController));
			FPSscript.enabled=false;
			MouseLook cameraScript=(MouseLook)cam.GetComponent(typeof(MouseLook));
			cameraScript.enabled=false;

		}

		if(SetNumber==3)
		{
			print ("GOHOME");
			Instructions[2].SetActive(false);
			Instructions[3].SetActive(true);
			scr_RecordPosition RecordingScript=(scr_RecordPosition)Player.GetComponent(typeof(scr_RecordPosition));
			print (Player.transform.position);
			print (RecordingScript.StartPosition);	
			MouseLook lookScript=(MouseLook)Player.GetComponent (typeof(MouseLook));
			lookScript.enabled=false;
			CharacterMotor Motor=(CharacterMotor)Player.GetComponent (typeof(CharacterMotor));
			Motor.enabled=false;
			FPSInputController FPSscript=(FPSInputController)Player.GetComponent (typeof(FPSInputController));
			FPSscript.enabled=false;
			MouseLook cameraScript=(MouseLook)cam.GetComponent(typeof(MouseLook));
			cameraScript.enabled=false;
		}
	}
}
