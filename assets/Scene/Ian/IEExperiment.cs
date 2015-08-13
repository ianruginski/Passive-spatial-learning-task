using UnityEngine;
using System.Collections.Generic;

public class IEExperiment : SceneBase {

	public DynamicObject Shed, BucketWell, StartHome, Tent, Truck, Peru;

	public Camera MainCamera, OrthCamera;

	public Texture2D CrossHairTexture;

	public TerrainCollider TopDownRaycastTerrain;

	private bool displayCrossHair =false;

	private bool beforeVideo = true;

	public static bool IsSecondTime = false;

	public static Vector3 LastReplayPlayerEndPosition;
	public static Vector3 LastReplayPlayerEndDirectionAngle;

	//public Transform HoverNode;
//	private bool hideHover = false;
	private string lastInteractObjName = "";

	public static string PNum;

	public enum ExperiementState
	{
		Designer_Recoding,
		Designer_Replaying,
		Player_Replaying,
		Player_Recording,
	}

	public static ExperiementState CurrentExpState;
	
	private void onInteractive(DynamicObject dynamicObject)
	{
		lastInteractObjName = dynamicObject.name;
		showAlphaText (lastInteractObjName);
	}
	
	protected override void trackModeInit ()
	{
		base.trackModeInit ();
		
		Shed.OnInteract += onInteractive;
		BucketWell.OnInteract += onInteractive;
		StartHome.OnInteract += onInteractive;
		Tent.OnInteract += onInteractive;
		Truck.OnInteract += onInteractive;
		Peru.OnInteract += onInteractive;

		if(CurrentExpState == ExperiementState.Player_Recording)
			createTasks ();
	}

	protected override void replayModeInit ()
	{
		base.replayModeInit ();

		Shed.OnInteract += onInteractive;
		BucketWell.OnInteract += onInteractive;
		StartHome.OnInteract += onInteractive;
		Tent.OnInteract += onInteractive;
		Truck.OnInteract += onInteractive;
		Peru.OnInteract += onInteractive;
	}
	
	protected override void trackModeUpdate ()
	{
		base.trackModeUpdate ();
	}

	// Update is called once per frame
	protected override void Update ()
	{
		base.Update ();
		if(Input.GetKeyDown(KeyCode.Escape))
		{
			Application.LoadLevel("IETesterInfo");
		}

		updateTopDown ();


		Debug.DrawRay (tempRAyDisplay.origin, tempRAyDisplay.direction);

//		if(!hideHover)
//		{
//			HoverNode.Rotate (Vector3.up, 100.0f * Time.deltaTime);
//		}
	}

	protected override void OnGUI ()
	{
		base.OnGUI ();

		onGUIAlphaText ();

		guiTopDown ();

		if(displayCrossHair)
		{
			float size = Screen.width * 0.02f;
			GUI.DrawTexture(new Rect(0.5f * Screen.width - size,
			                         0.5f * Screen.height - size,
			                         size * 2,size * 2),CrossHairTexture);
		}
	}

	private int[] randomOrder(int[] input)
	{
		int[] output = new int[input.Length];
		List<int> temp = new List<int> ();
		foreach(int v in input)
			temp.Add(v);
		int i = 0;
		while(temp.Count > 0)
		{
			int r = Random.Range(0,temp.Count);
			output[i++] = temp[r];
			temp.RemoveAt(r);
		}
		return output;
	}

	private bool IsTheSameOrder(int[] one, int[] two)
	{
		for(int i =0;i<one.Length;i++)
		{
			if(one[i] != two[i])
				return false;
		}
		return true;
	}

	override protected void onReplayEnd ()
	{
		base.onReplayEnd ();

		if(CurrentExpState == ExperiementState.Designer_Replaying)
			Application.LoadLevel("IETesterInfo");
		else if(CurrentExpState == ExperiementState.Player_Replaying)
		{
			//record the end player position
			LastReplayPlayerEndPosition = PlayerObject.Instance.Position;
			LastReplayPlayerEndDirectionAngle = PlayerObject.Instance.DirectionAngle;

			//load next level
			IEExperiment.dataFilePath = string.Format("SAVE_{0}_{1}.dat",IEExperiment.PNum,System.DateTime.Now.ToString("MM_dd_yyyy_HH_mm")); // SAVE_PNumber_month_day_year_hour_minutes
			IEExperiment.SceneMode = SceneBase.SceneModeEnum.Record;
			IEExperiment.CurrentExpState = IEExperiment.ExperiementState.Player_Recording;
			
			Application.LoadLevel("IEExperiment");
		}
	}

	#region tasks
	private void createTasks()
	{


		if(!IEExperiment.IsSecondTime)
		#region first time
		{
			addPopupTextTask ("Your knowledge of the environment you just learned will now be tested. You will be asked to point from one landmark (where you will be standing) to another landmark in the environment. In these tests, the landmarks are removed from the environment, so you will not be able to see the landmark you are pointing at even if you are directly pointing at it. Please press “A” on the controller to begin the task whenever you are ready.",false,true);

			//random
			DynamicObject[] objects = new DynamicObject[]{BucketWell, StartHome, Truck};
			int[] objArr = randomOrder(new int[]{0,1,2});

			for(int to =0;to<3;to++)
			{
				//short cut
				//addFindObjTask (objects[objArr[to]],1, string.Format("Please find a short cut from {0} to {1}",objects[objArr[from]].name,objects[objArr[to]].name),
				//              true, objects[objArr[from]].transform.position + new Vector3(0,10,0), Vector3.zero);
				//pointing
				addPointTask (objects[objArr[to]], string.Format(
					"Please point as accurately as possible to where you believe the “{0}” is located. Press “A” on the controller to input your response once you have arrived at your most accurate judgment."
				   ,objects[objArr[to]].name),
				              true, LastReplayPlayerEndPosition, LastReplayPlayerEndDirectionAngle);
			}

			addPopupTextTask ("Please do the pointing test again",false,true);

			int[] secondArr;

			while(true)
			{
				secondArr = randomOrder(objArr);
				if(!IsTheSameOrder(objArr,secondArr))
					break;
			}


			for(int to =0;to<3;to++)
			{
				//short cut
				//addFindObjTask (objects[objArr[to]],1, string.Format("Please find a short cut from {0} to {1}",objects[objArr[from]].name,objects[objArr[to]].name),
				//              true, objects[objArr[from]].transform.position + new Vector3(0,10,0), Vector3.zero);
				//pointing
				addPointTask (objects[secondArr[to]], string.Format(
					"Please point as accurately as possible to where you believe the “{0}” is located. Press “A” on the controller to input your response once you have arrived at your most accurate judgment."
					,objects[secondArr[to]].name),
				              true, LastReplayPlayerEndPosition, LastReplayPlayerEndDirectionAngle);
			}

			//play second video
			addPlaySecondVideoTask();

			taskMgr.NextTask();

		}
		#endregion
		else
		#region second time
		{

			addPopupTextTask ("Your knowledge of the environment you just learned will now be tested. You will be asked to point from one landmark (where you will be standing) to another landmark in the environment. In these tests, the landmarks are removed from the environment, so you will not be able to see the landmark you are pointing at even if you are directly pointing at it. Please press “A” on the controller to begin the task whenever you are ready.",false,true);
			
			//random
			DynamicObject[] objects = new DynamicObject[]{Shed, Tent, Peru};
			int[] objArr = randomOrder(new int[]{0,1,2});
			
			for(int from =0;from<3;from++)
			{
				int to = from + 1;
				if(to >= 3) to = 0;
				//short cut
				//addFindObjTask (objects[objArr[to]],1, string.Format("Please find a short cut from {0} to {1}",objects[objArr[from]].name,objects[objArr[to]].name),
				//              true, objects[objArr[from]].transform.position + new Vector3(0,10,0), Vector3.zero);
				//pointing
				addPointTask (objects[objArr[to]], string.Format(
					"Please point as accurately as possible to where you believe the “{0}” is located. Press “A” on the controller to input your response once you have arrived at your most accurate judgment."
					,objects[objArr[to]].name),
				              true, LastReplayPlayerEndPosition, LastReplayPlayerEndDirectionAngle);
			}

			addPopupTextTask ("Please do the pointing test again",false,true);
			
			int[] secondArr;
			
			while(true)
			{
				secondArr = randomOrder(objArr);
				if(!IsTheSameOrder(objArr,secondArr))
					break;
			}
			
			
			for(int to =0;to<3;to++)
			{
				//short cut
				//addFindObjTask (objects[objArr[to]],1, string.Format("Please find a short cut from {0} to {1}",objects[objArr[from]].name,objects[objArr[to]].name),
				//              true, objects[objArr[from]].transform.position + new Vector3(0,10,0), Vector3.zero);
				//pointing
				addPointTask (objects[secondArr[to]], string.Format(
					"Please point as accurately as possible to where you believe the “{0}” is located. Press “A” on the controller to input your response once you have arrived at your most accurate judgment."
					,objects[secondArr[to]].name),
				              true, LastReplayPlayerEndPosition, LastReplayPlayerEndDirectionAngle);
			}

			addPopupTextTask ("Thank you for completing the first test. Now, as a second test, you will be provided with an above-ground, map-like view of the environment that you previously learned. In these tests, the landmarks are removed from the environment, so you will not be able to see the landmark you are pointing at even if you are directly pointing at it. Please press “A” on the controller to begin the task whenever you are ready.",false,true);


			objects = new DynamicObject[]{BucketWell, StartHome, Truck, Shed, Tent, Peru};
			for(int i =0;i<6;i++)
			{
				addTopDownTask (objects[i], string.Format("Please point as accurately as possible to where you believe the “{0}” is located. Use the right-hand controller joystick to control the pointing direction. Press “A” on the controller to input your response once you have arrived at your most accurate judgment. Press “B” on the controller to hide/redisplay the text.",objects[i].name));
			}



	//		//experiment ends
			addPopupTextTask ("You have successfully completed the first part of our experiment! There will be one more separate follow-up test and survey.",true,true);

			taskMgr.NextTask();
		}
		#endregion
	}
	
	private void addShowHoverTask(DynamicObject hoverObj, string text)
	{
		Task task = new Task ();
		task.TaskText = text;
		task.OnTaskStart += (Task self) => {
			//showHover(hoverObj);
			PlayerObject.Instance.EnableMovement(false);
			ShowTaskInfo(self.TaskText); 
			return false;
		};
		task.OnTaskProgressCheck += (Task self) => {return PlayerInput.IsInteractiveKeyDown();};
		task.OnTaskEnd += (Task self) => {
			PlayerObject.Instance.EnableMovement(true);
			//hideCurrentHover();
			return false;
		};
		taskMgr.PushTask(task);
	}

	private void addPlaySecondVideoTask()
	{
		Task task = new Task ();
		task.TaskText = "PLEASE ALERT YOUR EXPERIMENTER THAT YOU HAVE FINISHED THE FIRST PORTION OF THE EXPERIMENT. Thank you for completing the first part of pointing test.  You will now be taken on a separate route of the environment. Please pay attention to the locations of the major named landmarks encountered in the environment.";
		task.OnTaskStart += (Task self) => {
			PlayerObject.Instance.EnableMovement(false);
			PlayerObject.Instance.EnableLooking(false);
			ShowTaskInfo(self.TaskText,true); 

			MainCamera.gameObject.SetActive(false);
			OrthCamera.gameObject.SetActive(false);

			return false;
		};
		task.OnTaskProgressCheck += (Task self) => {return PlayerInput.IsInteractiveKeyDown();};
		task.OnTaskEnd += (Task self) => {
			PlayerObject.Instance.EnableMovement(true);
			PlayerObject.Instance.EnableLooking(true);

			MainCamera.gameObject.SetActive(true);
			OrthCamera.gameObject.SetActive(false);

			//play second video
			IEExperiment.dataFilePath = string.Format("Ian_Replay2.dat");
			IEExperiment.SceneMode = SceneBase.SceneModeEnum.Replay;
			IEExperiment.CurrentExpState = IEExperiment.ExperiementState.Player_Replaying;
			IEExperiment.IsSecondTime = true;
			IEExperiment.Append = true; // append the same file
			
			Application.LoadLevel("IEExperiment");

			return false;
		};
		taskMgr.PushTask(task);
	}

	private void addPopupTextTask(string text, bool isEnd = false, bool blackMode = false)
	{
		Task task = new Task ();
		task.TaskText = text;
		task.OnTaskStart += (Task self) => {
			PlayerObject.Instance.EnableMovement(false);
			PlayerObject.Instance.EnableLooking(false);
			ShowTaskInfo(self.TaskText,blackMode); 
			if(blackMode)
			{
				MainCamera.gameObject.SetActive(false);
				OrthCamera.gameObject.SetActive(false);
			}
			return false;
		};
		task.OnTaskProgressCheck += (Task self) => {return PlayerInput.IsInteractiveKeyDown();};
		task.OnTaskEnd += (Task self) => {
			PlayerObject.Instance.EnableMovement(true);
			PlayerObject.Instance.EnableLooking(true);
			if(blackMode)
			{
				MainCamera.gameObject.SetActive(true);
				OrthCamera.gameObject.SetActive(false);
			}
			if(isEnd)
				Application.LoadLevel("IETesterInfo");
			return false;
		};
		taskMgr.PushTask(task);
	}
	
	private void addFindObjTask(DynamicObject targetObj, int count, string text, bool teleport, Vector3 telePos, Vector3 teleDir)
	{
		Task task = new Task ();
		task.TaskText = text;
		task.OnTaskStart += (Task self) => {
			ShowTaskInfo(self.TaskText); 
			if(teleport)
				PlayerObject.Instance.TeleportTo(telePos, teleDir);
			return false;
		};
		task.OnTaskProgressCheck += (Task self) => {
			return checkInteractedObject(targetObj.name);
		};
		task.OnTaskEnd += (Task self) => {
			TextInfoAction tAction = new TextInfoAction();
			tAction.Record(string.Format("[Interactive]TargetName:{0},Count:{1}", targetObj.name, count));
			ActionTracker.TrackAction(tAction);
			return false;
		};
		taskMgr.PushTask(task);
	}
	
	private void addPointTask(DynamicObject targetObj, string text, bool teleport, Vector3 telePos, Vector3 teleDir)
	{
		Task task = new Task ();
		task.TaskText = text;
		task.OnTaskStart += (Task self) => {
			ShowTaskInfo(self.TaskText); 
			if(teleport)
				PlayerObject.Instance.TeleportTo(telePos, teleDir);
			startPointingTarget(targetObj);
			PlayerObject.Instance.EnableMovement(false);
			displayCrossHair = true;
			SetDynamicObjsVisible(false);
			return false;
		};
		task.OnTaskProgressCheck += (Task self) => {
			if(PlayerInput.IsInteractiveKeyDown())
			{
				endPointingTarget();
				displayCrossHair = false;
				return true;
			}
			return false;
		};
		task.OnTaskEnd += (Task self) => {
			PlayerObject.Instance.EnableMovement(true);
			SetDynamicObjsVisible(true);
			return false;
		};
		taskMgr.PushTask(task);
	}

	private void addTopDownTask(DynamicObject targetObj, string text)
	{
		Task task = new Task ();
		task.TaskText = text;
		task.OnTaskStart += (Task self) => {
			ShowTaskInfo(self.TaskText); 
			startTopDown();
			PlayerObject.Instance.EnableMovement(false);
			PlayerObject.Instance.EnableLooking(false);
			SetDynamicObjsVisible(false);
			return false;
		};
		task.OnTaskProgressCheck += (Task self) => {
			if(PlayerInput.IsInteractiveKeyDown())
			{
				endTopDown(targetObj.transform.position);
				return true;
			}
			return false;
		};
		task.OnTaskEnd += (Task self) => 
		{
			PlayerObject.Instance.EnableMovement(true);
			PlayerObject.Instance.EnableLooking(true);
			SetDynamicObjsVisible(true);
			return false;
		};
		taskMgr.PushTask(task);
	}
	
	#endregion

	#region GUI Alpha Text
	private float alphaText_alpha = 0.0f;
	private float alphaText_fadeSpeed = 0.33f;
	private string alphaText_text = "";
	private void showAlphaText(string text)
	{
		alphaText_alpha = 2.0f;
		alphaText_text = text;
	}
	private void onGUIAlphaText()
	{
		if(alphaText_alpha > 0.0f)
		{
			GUIHelper.LableAlpha (alphaText_text, alphaText_alpha);
			alphaText_alpha -= Time.deltaTime * alphaText_fadeSpeed;
		}
	}
	#endregion

	private bool checkInteractedObject(string objName)
	{
		return (lastInteractObjName == objName);
	}
	
	private void startPointingTarget(DynamicObject target)
	{
		TextInfoAction tAction = new TextInfoAction();
		tAction.Record(string.Format("[StartPoint]PlayerPos:{0},TargetName:{1},TargetPos:{2}", PlayerObject.Instance.Position, target.name, target.transform.position));
		ActionTracker.TrackAction(tAction);
	}
	
	private void endPointingTarget()
	{
		TextInfoAction tAction = new TextInfoAction();
		tAction.Record(string.Format("[EndPoint]PointAngle:{0},PointDir:{1}", PlayerObject.Instance.DirectionAngle, PlayerObject.Instance.Direction));
		ActionTracker.TrackAction(tAction);
	}

	#region TopDown

	private bool topDownEnable = false;
	private Vector2 crosshairPos;

	private void startTopDown()
	{
		OrthCamera.gameObject.SetActive (true);
		MainCamera.gameObject.SetActive (false);
		topDownEnable = true;
		crosshairPos = new Vector2 (0.5f, 0.5f);
	}

	private void endTopDown(Vector3 targetPos)
	{
		recordTopDown (targetPos);
		OrthCamera.gameObject.SetActive (false);
		MainCamera.gameObject.SetActive (true);
		topDownEnable = false;
	}

	private void guiTopDown()
	{
		if(topDownEnable)
		{
			float size = Screen.width * 0.02f;
			GUI.DrawTexture(new Rect(crosshairPos.x * Screen.width - size,
			                         crosshairPos.y * Screen.height - size,
			                         size * 2,size * 2),CrossHairTexture);
		}
	}

	private void updateTopDown()
	{
		if(topDownEnable)
		{
			crosshairPos.x = Mathf.Clamp01(crosshairPos.x + Input.GetAxis("Controller X") * Time.deltaTime * 2.0f);
			crosshairPos.y = Mathf.Clamp01(crosshairPos.y - Input.GetAxis("Controller Y") * Time.deltaTime * 2.0f);
		}
	}

	private void recordTopDown(Vector3 targetPos)
	{


		Ray r = OrthCamera.ScreenPointToRay(new Vector3(crosshairPos.x * Screen.width, (1.0f - crosshairPos.y) * Screen.height,0));

		tempRAyDisplay = r;
		RaycastHit hit;

		TextInfoAction tAction = new TextInfoAction();

		if(TopDownRaycastTerrain.Raycast (r, out hit, 10000))
		{
			tAction.Record(string.Format("[TopDown]Player:{0},Target:{1}", new Vector2(hit.point.x,hit.point.z), new Vector2(targetPos.x,targetPos.z)));
		}
		else
		{
			tAction.Record("[TopDown]Out Of Terrain!");
		}

		ActionTracker.TrackAction(tAction);
	}

	private Ray tempRAyDisplay;

	#endregion

	private void SetDynamicObjsVisible(bool visible)
	{
		Shed.transform.parent.gameObject.SetActive (visible);
	}

//	private void showHover(DynamicObject dObj)
//	{
//		foreach (Transform trans in HoverNode.transform)
//			GameObject.Destroy (trans.gameObject);
//		if(dObj != null)
//		{
//			GameObject hover = (GameObject)GameObject.Instantiate (dObj.gameObject);
//			hover.transform.parent = HoverNode;
//			hover.transform.localPosition = Vector3.zero;
//		}
//		hideHover = false;
//		HoverNode.gameObject.SetActive(true);
//	}
//	
//	private void hideCurrentHover()
//	{
//		hideHover = true;
//		HoverNode.gameObject.SetActive(false);
//	}
}

