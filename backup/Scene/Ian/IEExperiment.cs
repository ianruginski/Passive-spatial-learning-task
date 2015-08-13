using UnityEngine;
using System.Collections.Generic;

public class IEExperiment : SceneBase {

	public DynamicObject Shed, BucketWell, StartHome, Tent, Truck, Peru;

	public Camera MainCamera, OrthCamera;

	public Texture2D CrossHairTexture;

	public TerrainCollider TopDownRaycastTerrain;

	private bool displayCrossHair =false;

	private bool beforeVideo = true;

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

	override protected void onReplayEnd ()
	{
		base.onReplayEnd ();

		if(CurrentExpState == ExperiementState.Designer_Replaying)
			Application.LoadLevel("IETesterInfo");
		else if(CurrentExpState == ExperiementState.Player_Replaying)
		{
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
		addPopupTextTask ("Your knowledge of the environment you just learned will now be tested. You will be asked to point from one landmark (where you will be standing) to another landmark in the environment. In these tests, the landmarks are removed from the environment, so you will not be able to see the landmark you are pointing at even if you are directly pointing at it. Please press “A” on the controller to begin the task whenever you are ready.",false,true);

		//random
		DynamicObject[] objects = new DynamicObject[]{Shed, BucketWell, StartHome, Tent, Truck, Peru};
		int[] objArr = randomOrder(new int[]{0,1,2,3,4,5});

		for(int from =0;from<6;from++)
		{
			int to = from + 1;
			if(to >= 6) to = 0;
			//short cut
			//addFindObjTask (objects[objArr[to]],1, string.Format("Please find a short cut from {0} to {1}",objects[objArr[from]].name,objects[objArr[to]].name),
			//              true, objects[objArr[from]].transform.position + new Vector3(0,10,0), Vector3.zero);
			//pointing
			addPointTask (objects[objArr[to]], string.Format(
				"You are currently located at the “{0}” Please point as accurately as possible to where you believe the “{1}” is located. Press “A” on the controller to input your response once you have arrived at your most accurate judgment."
			   ,objects[objArr[from]].name,objects[objArr[to]].name),
			              true, objects[objArr[from]].transform.position, Vector3.zero);
		}


		addPopupTextTask ("Thank you for completing the first test. Now, as a second test, you will be provided with an above-ground, map-like view of the environment that you previously learned. In these tests, the landmarks are removed from the environment, so you will not be able to see the landmark you are pointing at even if you are directly pointing at it. Please press “A” on the controller to begin the task whenever you are ready.",false,true);


		for(int i =0;i<6;i++)
		{
			addTopDownTask (objects[i], string.Format("Please point as accurately as possible to where you believe the “{0}” is located. Use the right-hand controller joystick to control the pointing direction. Press “A” on the controller to input your response once you have arrived at your most accurate judgment. Press “B” on the controller to hide/redisplay the text.",objects[i].name));
		}


//		//cart
//		addShowHoverTask(Cart, "Here is the first object that you will look for. This is the size and appearance of the object. When you press Button A, the object will be hidden and you will go find it.");
//		addFindObjTask (Cart, 1,"Find the cart you just saw. When you found the first object, hit Button A to record.");
//		addFindObjTask (StartLandmark, 1, "Well done, you find it, then go back to the start.");
//		addFindObjTask (Cart, 2, "You made it back to the start. Now you will return to the Cart as quickly as possible.");
//		addFindObjTask (StartLandmark, 2, "Good Job! You made it back to the Cart, now go back to the start and you'll look for a new object.");
//		
//		//locked chest
//		addShowHoverTask(LockedChest, "Now you will look for the gold coins in the Locked Chest. As soon as you hit Button A, the locked chest will be hidden and you will go find it.");
//		addFindObjTask (LockedChest, 1, "Find the locked chest you just saw. When you found the first object, hit Button A to record.");
//		addFindObjTask (StartLandmark, 1, "Good Job! You found the locked chest. Now, go back to the start with your gold!");
//		addFindObjTask (LockedChest, 2, "You made it back to the start. Now you will return to the Locked Chest as quickly as possible.");
//		addFindObjTask (StartLandmark, 2, "Good Job! You made it back to the Locked Chest, now go back to the start and you'll look for a new object.");
//		
//		//the well
//		addShowHoverTask(Well, "Now you will look for the well. As soon as you hit Button A, the well will be hidden and you will go find it.");
//		addFindObjTask (Well, 1, "Find the well you just saw. When you found the first object, hit Button A to record.");
//		addFindObjTask (StartLandmark, 1, "Good Job! You found the well! Now, go back to the start.");
//		addFindObjTask (Well, 2, "You made it back to the start. Now you will return to the Well as quickly as possible.");
//		addFindObjTask (StartLandmark, 2, "Good Job! You made it back to the Well, now go back to the start.");
//		
//		//start pointing
//		addPopupTextTask ("You've found all of the objects. Next, you will point in the direction you would travel if you were to visit the object.");
//		
//		//from landmark
//		addPointTask (Cart, "Now point in the direction you would travel if you were to go back to the Cart. Hit Button A when you've finalized your answer.",
//		              true, StartLandmark.transform.position + new Vector3(10,0,0), Vector3.zero);
//		addPointTask (LockedChest, "Now point in the direction you would travel if you were to go back to the Locked Chest. Hit Button A when you've finalized your answer.",
//		              false,Vector3.zero,Vector3.zero);
//		addPointTask(Well, "Now point in the direction you would travel if you were to go back to the Well. Hit Button A when you've finalized your answer.",
//		             false,Vector3.zero,Vector3.zero);
//		
//		//from cart
//		addPointTask (StartLandmark, "Now point in the direction you would travel if you were to go back to the StartLandmark. Hit Button A when you've finalized your answer.",
//		              true, Cart.transform.position + new Vector3(5,0,0), Vector3.zero);
//		addPointTask (LockedChest, "Now point in the direction you would travel if you were to go back to the Locked Chest. Hit Button A when you've finalized your answer.",
//		              false,Vector3.zero,Vector3.zero);
//		addPointTask(Well, "Now point in the direction you would travel if you were to go back to the Well. Hit Button A when you've finalized your answer.",
//		             false,Vector3.zero,Vector3.zero);
//		
//		//from chest
//		addPointTask (Cart, "Now point in the direction you would travel if you were to go back to the Cart. Hit Button A when you've finalized your answer.",
//		              true, LockedChest.transform.position + new Vector3(5,0,0), Vector3.zero);
//		addPointTask (StartLandmark, "Now point in the direction you would travel if you were to go back to the StartLandmark. Hit Button A when you've finalized your answer.",
//		              false,Vector3.zero,Vector3.zero);
//		addPointTask(Well, "Now point in the direction you would travel if you were to go back to the Well. Hit Button A when you've finalized your answer.",
//		             false,Vector3.zero,Vector3.zero);
//		
//		//from well
//		addPointTask (Cart, "Now point in the direction you would travel if you were to go back to the Cart. Hit Button A when you've finalized your answer.",
//		              true, Well.transform.position + new Vector3(5,0,0), Vector3.zero);
//		addPointTask (LockedChest, "Now point in the direction you would travel if you were to go back to the Locked Chest. Hit Button A when you've finalized your answer.",
//		              false,Vector3.zero,Vector3.zero);
//		addPointTask(StartLandmark, "Now point in the direction you would travel if you were to go back to the StartLandmark. Hit Button A when you've finalized your answer.",
//		             false,Vector3.zero,Vector3.zero);
//	

//		//experiment ends
		addPopupTextTask ("You have successfully completed the first part of our experiment! There will be one more separate follow-up test and survey.",true,true);

		taskMgr.NextTask();
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

