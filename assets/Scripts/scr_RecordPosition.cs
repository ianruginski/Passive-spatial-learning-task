	using UnityEngine;
using System.Collections;
using System.IO;
public class scr_RecordPosition : MonoBehaviour {
	Vector3[] positions;
	Vector3[] rotations;
	Vector3[] camRotations;
	public GameObject cam;
	public int subjectNumber;
	public int sampleRate;														//The number of times data is sampled per second

	public GameObject StartLandmark;
	public GameObject LeftLocation;
	public GameObject RightLocation;

	private string FrameInput;
	Vector3 toObject1;
	Vector3 toObject2;
	Vector3 toObject3;
	Vector3 toObject4;
	Vector3 toObject5;
	private GameObject[] ObjectsArray;
	private int[] objectIDs;
	private int currentObjectID;
	private int sample;
	private string filePath;
	private string rockLocationPath;
	private string pointPath;
	private int objnum;
	public Vector3 StartPosition;
	int count;
	public int objectEncounter;
	public bool displayGUI;
	public bool finalGUI;
	private Vector3 ZeroAngle;
	private int objDirCount;
	public scr_StartingText subjectInfo;
	private float totalTime;
	private string drawingPointPath;
	public scr_ReproducePath TracingScript;
	//Ians Variables
	public int SetNumber;
	public GameObject InstructionScreen;
	//bool replay;
	bool displayCrosshairs;
	// Use this for initialization
	void Awake()
	{
		StartPosition=transform.position;
	}
	void Start () {
		count=0;
		objectEncounter=0;
		sample=0;
		SetNumber=1;
		objnum=1;
		objDirCount=0;
		totalTime=0.0f;
		FrameInput="";
		//replay=false;
		ZeroAngle=transform.forward;
		displayCrosshairs=false;
		displayGUI=false;
		finalGUI=false;
		                    
		GameObject TextObject=GameObject.Find("TextObject");
		subjectInfo=(scr_StartingText)TextObject.GetComponent(typeof(scr_StartingText));
		if (!Directory.Exists(Application.dataPath+"/Data")) {
			Directory.CreateDirectory (Application.dataPath+"/Data");
		}


	

		//pointPath=filePath+"Pointing.txt";
		if(subjectInfo.Trial)
		{
			filePath=Application.dataPath+"/Data/Subject"+subjectInfo.Number;
			drawingPointPath=Application.dataPath+"/Data/TracingPoints"+subjectInfo.Number;
			while(System.IO.File.Exists (filePath+".txt"))
			{
				filePath=filePath+"+";
			}
			filePath=filePath+".txt";

			while(System.IO.File.Exists (drawingPointPath+".txt"))
			{
				drawingPointPath=drawingPointPath+"+";
			}
			drawingPointPath=drawingPointPath+".txt";
		}
		if(subjectInfo.Record)
		{
			SetNumber=999;
			filePath=Application.dataPath+"/Data/SubjectPracticeData.txt";
		}
		rockLocationPath=Application.dataPath+"/Data/RockLocations.txt";
		print ("Practice");
		print (Application.dataPath);
		if(subjectInfo.Record)
			System.IO.File.WriteAllText(rockLocationPath,"");
		System.IO.File.WriteAllText (filePath,"SubjectNumber:"+subjectInfo.Number+"\r\n"+"Gender:"+subjectInfo.Gender+"\r\n"+"Age:"+subjectInfo.Age+"\r\n");
		System.IO.File.AppendAllText(filePath,"    Pos(x,y,z)      "+"              Rot(x,y,z)    "+"                     Camera(x,y,z)   "+"  SetNumber"+"\r\n");	

 	}
	// Update is called once per frame
	void Update () {
		totalTime+=Time.deltaTime;

		if(!displayCrosshairs)
		{
			System.IO.File.AppendAllText(filePath,transform.position.x.ToString("F3")+" "+transform.position.y.ToString("F3")+" "+transform.position.z.ToString("F3")+" "+
			                             transform.eulerAngles.x.ToString("F3")+" "+transform.eulerAngles.y.ToString("F3")+" "+transform.eulerAngles.z.ToString("F3")+" "+
			                             cam.transform.eulerAngles.x.ToString("F1")+" "+cam.transform.eulerAngles.y.ToString("F1")+" "+cam.transform.eulerAngles.z.ToString("F1")+" "+SetNumber+"\r\n");
			
			if(subjectInfo.Record)
			{
				if(Input.GetKeyUp(KeyCode.KeypadEnter))
				{
					System.IO.File.AppendAllText(filePath,transform.position.x.ToString("F3")+" "+transform.position.y.ToString("F3")+" "+transform.position.z.ToString("F3")+" ");
					System.IO.File.AppendAllText(filePath,transform.eulerAngles.x.ToString("F3")+" "+transform.eulerAngles.y.ToString("F3")+" "+transform.eulerAngles.z.ToString("F3")+" ");
					System.IO.File.AppendAllText(filePath,cam.transform.eulerAngles.x.ToString("F1")+" "+cam.transform.eulerAngles.y.ToString("F1")+" "+cam.transform.eulerAngles.z.ToString("F1")+" "+998+"\r\n");;
				}
			}
		}
		


		//Start the next set
		if(Input.GetKeyUp(KeyCode.KeypadEnter))
		{
			SetNumber++;
			if(SetNumber==2)
			{
				InstructionScreen.SetActive(true);
				scr_InstructionScreenClickCallback InstructionScript=(scr_InstructionScreenClickCallback)InstructionScreen.GetComponent(typeof(scr_InstructionScreenClickCallback));
				InstructionScript.EnableText(SetNumber);
				this.enabled=false;
				transform.position=StartPosition;

				MouseLook lookScript=(MouseLook)GetComponent (typeof(MouseLook));
				lookScript.enabled=false;
				
				CharacterMotor Motor=(CharacterMotor)GetComponent (typeof(CharacterMotor));
				Motor.enabled=false;
				FPSInputController FPSscript=(FPSInputController)GetComponent (typeof(FPSInputController));
				FPSscript.enabled=false;
				MouseLook cameraScript=(MouseLook)cam.GetComponent(typeof(MouseLook));
				cameraScript.enabled=false;
				this.enabled=false;
			}
			if(SetNumber==3)
			{
				InstructionScreen.SetActive(true);
				scr_InstructionScreenClickCallback InstructionScript=(scr_InstructionScreenClickCallback)InstructionScreen.GetComponent(typeof(scr_InstructionScreenClickCallback));
				InstructionScript.EnableText(SetNumber);
				this.enabled=false;
				transform.position=StartPosition;
				TracingScript.DataPath=drawingPointPath;
				MouseLook lookScript=(MouseLook)GetComponent (typeof(MouseLook));
				lookScript.enabled=false;
				
				CharacterMotor Motor=(CharacterMotor)GetComponent (typeof(CharacterMotor));
				Motor.enabled=false;
				FPSInputController FPSscript=(FPSInputController)GetComponent (typeof(FPSInputController));
				FPSscript.enabled=false;
				MouseLook cameraScript=(MouseLook)cam.GetComponent(typeof(MouseLook));
				cameraScript.enabled=false;
				this.enabled=false;
			}
		}



	}

}
