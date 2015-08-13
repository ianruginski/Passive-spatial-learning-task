using UnityEngine;
using System.Collections;

public class TestScene : MonoBehaviour {

	public static bool IsRecordingMode = false; // true = recording, false = replaying

	public DynamicObject[] Cubes;
	public DynamicObject BaseCube;
	private int GoalCubeIdx = -1;
	private bool BaseBacked = false;

	private bool fillPlayerInfo = false;
	private string name = "Peter";
	private string food = "Apple Pie";

	public ControllerLook ControllerLook;

	// Use this for initialization
	void Start () {

		for(int i = 0;i<5;i++)
		{
			DynamicObject cube = Cubes[i];
			cube.OnInteract += (DynamicObject dynamicObject) => 
			{
				Debug.Log(i);
				if(BaseBacked && dynamicObject == Cubes[GoalCubeIdx])
				{
					cube.gameObject.SetActive(false);
					BaseBacked = false;
					ShowInfo(string.Format("Well Done, Plz go back to base!", GoalCubeIdx));
				}
			};
		}

		BaseCube.OnInteract += (DynamicObject dynamicObject) => {
			if(BaseBacked == false)
			{
				BaseBacked = true;
				GoalCubeIdx++;
				ShowInfo(string.Format("Next Target, find Cube {0}", GoalCubeIdx));
			}
		};

		ControllerLook.enabled = IsRecordingMode;
		fillPlayerInfo = !IsRecordingMode;
	}
	
	// Update is called once per frame
	void Update () {

		if (ActionTracker.IsTracking)
			ActionTracker.Update (Time.deltaTime);

		if (ActionReplayer.IsPlaying)
			ActionReplayer.Update (Time.deltaTime);

		if(Input.GetKeyDown(KeyCode.Y))
		{
			if(!IsRecordingMode) StartPlay();
			else StartTrack();
		}
		else if(Input.GetKeyDown(KeyCode.U))
		{
			if(!IsRecordingMode) EndPlay();
			else EndTrack();
		}
	}

	void OnGUI()
	{
		if(showInfo)
		{
			GUI.TextArea(new Rect(100,100,300,200),infoText);
			if(GUI.Button(new Rect(100,320,50,30),"OK"))
				showInfo = false;
		}

		if(!fillPlayerInfo)
		{
			name = GUI.TextField(new Rect(100,100,100,25),name);
			food = GUI.TextField(new Rect(100,130,100,25),food);
			if(GUI.Button(new Rect(100,160,100,25),"OK"))
			{
				StartTrack();
				TextInfoAction tAction = new TextInfoAction();
				tAction.Record(string.Format("name:{0},food:{1}",name,food));
				ActionTracker.TrackAction(tAction);
				fillPlayerInfo = true;
			}
		}
	}

	private bool showInfo = false;
	private string infoText = "";

	public void ShowInfo(string text)
	{
		infoText = text;
		showInfo = true;
	}

	public void StartTrack()
	{
		ActionTracker.StartTracking ();
		PlayerActionSampler.Instance.ResetSamplingTime ();
	}

	public void EndTrack()
	{
		ActionTracker.EndTracking ();
		ActionTracker.OutputTracking ("test_data.txt",false);
		Application.LoadLevel ("testMenu");
	}

	public void StartPlay()
	{
		ActionReplayer.LoadActions ("test_data.txt");
		PlayerActionPlayer.Instance.ResetPlayer ();
		ActionReplayer.PlayFromBeginning ();
	}

	public void EndPlay()
	{
		ActionReplayer.EndPlaying ();
		Application.LoadLevel ("testMenu");
	}
}
