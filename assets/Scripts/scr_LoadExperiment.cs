using UnityEngine;
using System.Collections;
using System.IO;
public class scr_LoadExperiment : MonoBehaviour {
	public string subjectNumber;
	public GameObject Left;
	public GameObject Right;
	public GameObject Rocks;
	public GameObject RockParent;
	public GameObject Instructions;
	string[] replayPos;
	Vector3[] positions;
	Vector3[] replayRotations;
	Vector3[] replayCamRotations;
	Vector3[] leftPositions;
	Vector3[] rightPositions;
	Vector3[] rotations;
	int[] setNumbers;
	public GameObject cam;
	public int Spawnrate;
	int count;
	int sample;
	public int InverseSampleRate;
	public bool SpawnRocks;

	// Use this for initialization
	void Start () {
		count=0;
		sample=0;
		positions=new Vector3[50000];
		replayPos=new string[50000];
		replayRotations=new Vector3[50000];
		replayCamRotations=new Vector3[50000];
		leftPositions=new Vector3[50000];
		rightPositions=new Vector3[50000];
		rotations=new Vector3[50000];
		setNumbers = new int[50000];
		MouseLook lookScript=(MouseLook)GetComponent (typeof(MouseLook));
		lookScript.enabled=false;
		FPSInputController FPSscript=(FPSInputController)GetComponent (typeof(FPSInputController));
		FPSscript.enabled=false;
		CharacterMotor Motor=(CharacterMotor)GetComponent (typeof(CharacterMotor));
		Motor.enabled=false;
		MouseLook cameraScript=(MouseLook)cam.GetComponent(typeof(MouseLook));
		cameraScript.enabled=false;
		StreamReader subjectFile = new StreamReader(Application.dataPath + "/Data/SubjectPracticeData.txt");
		string fileContents = subjectFile.ReadToEnd();
		subjectFile.Close();

		string[] lines = fileContents.Split('\n');
		for(int i=4;i< lines.Length-1;i++)
		{
			string[] Numbers=lines[i].Split(' ');
			for(int j=0;j<Numbers.Length;j++)
			{
			}
			replayPos[i-4]=Numbers[3];

			/*
			float positionX=float.Parse(Numbers[0]);
			float positionY=float.Parse(Numbers[1]);
			float positionZ=float.Parse(Numbers[2]);
			//replayPos[i-4]=new Vector3(positionX,positionY,positionZ);
			print (replayPos[i-4]);

			float rotationX=float.Parse(Numbers[9]);
			float rotationY=float.Parse(Numbers[10]);
			float rotationZ=float.Parse(Numbers[11]);
			replayRotations[i-4]=new Vector3(rotationX,rotationY,rotationZ);
			print (replayRotations[i-4]);
			*/

			float camRotationsX=float.Parse(Numbers[6]);
			float camRotationsY=float.Parse(Numbers[7]);
			float camRotationsZ=float.Parse(Numbers[8]);
			float positionX=float.Parse (Numbers[0]);
			float positionY=float.Parse (Numbers[1]);
			float positionZ=float.Parse (Numbers[2]);
			positions[i-4]=new Vector3(positionX,positionY,positionZ);


			float rotationX=float.Parse(Numbers[3]);
			float rotationY=float.Parse (Numbers[4]);
			float rotationZ=float.Parse (Numbers[5]);
			
			rotations[i-4]=new Vector3(rotationX,rotationY,rotationZ);

			replayCamRotations[i-4]=new Vector3(camRotationsX,camRotationsY,camRotationsZ);
			int setNumber=int.Parse(Numbers[9]);
			setNumbers[i-4]=setNumber;
			if(setNumber==998)
			{
				break;
			}
		
		

		}


	}
	
	// Update is called once per frame
	void Update () {
		sample++;
		if(sample==InverseSampleRate)
		{
			//Debug.DrawLine(replayPos[count],replayPos[count+1],Color.red, Mathf.Infinity);
			transform.position=positions[count+1];	
			transform.eulerAngles=rotations[count+1];
			//transform.eulerAngles=replayRotations[count];
			//cam.transform.eulerAngles=replayCamRotations[count];

			cam.transform.eulerAngles=replayCamRotations[count+1];
			/*
			if(replayPos[count]=="w")
			{
				CharacterController thisController=(CharacterController)GetComponent(typeof(CharacterController));
				Vector3 moveDirection=Vector3.Normalize(cam.transform.forward);
				thisController.SimpleMove(10.0f*moveDirection);


				print ("w");
			}

			if(replayPos[count]=="s")
			{
				CharacterController thisController=(CharacterController)GetComponent(typeof(CharacterController));
				Vector3 moveDirection=Vector3.Normalize(cam.transform.forward);
				Spawnrate++;

				thisController.SimpleMove(-10.0f*moveDirection);
				print ("s");
			}
			/*
			if(replayPos[count]!="")
			{
				print (replayPos[count]);

				Vector3 moveDirection = new Vector3(1, 0,
				                        1);
				CharacterController thisController=(CharacterController)GetComponent(typeof(CharacterController));
				thisController.SimpleMove(10.0f*Vector3.one);
			}
			*/

			count++;
			sample=0;

			if(setNumbers[count+1]==998)
			{

				Instructions.SetActive(true);
				print ("SetTrue");
				scr_InstructionScreenClickCallback InstructionsScript=(scr_InstructionScreenClickCallback)Instructions.GetComponent(typeof(scr_InstructionScreenClickCallback));
				InstructionsScript.EnableText(1);
				transform.eulerAngles=new Vector3(0.0f,cam.transform.eulerAngles.y,0.0f);
				this.enabled=false;
			}
		}
		else
		{
			//transform.position=Vector3.Lerp(positions[count],positions[count+1],(1.0f*sample)/InverseSampleRate);
			//cam.transform.eulerAngles=Vector3.Lerp (replayCamRotations[count],replayCamRotations[count+1],(1.0f*sample)/InverseSampleRate);
		}
	}
}
