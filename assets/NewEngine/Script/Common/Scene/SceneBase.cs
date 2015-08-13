using UnityEngine;
using System.Collections;

public abstract class SceneBase : MonoBehaviour {

	public enum SceneModeEnum { Record = 0,Replay };

	public static SceneModeEnum SceneMode;
	public static string PlayerInfo;
	public static string dataFilePath;
	public static bool Append = false;

	protected TaskManager taskMgr = new TaskManager();

	// Use this for initialization
	protected virtual void Start () {
		if(SceneMode == SceneModeEnum.Record)
			StartTrack ();
		else if(SceneMode == SceneModeEnum.Replay)
			StartPlay();

		taskMgr.ClearTasks ();

		if(SceneMode == SceneModeEnum.Record)
			trackModeInit();
		else if(SceneMode == SceneModeEnum.Replay)
			replayModeInit();
	}

	protected virtual void OnDestroy()
	{
		if(SceneMode == SceneModeEnum.Record)
			EndTrack ();
		else if(SceneMode == SceneModeEnum.Replay)
			EndPlay();
	}

	protected virtual void OnGUI()
	{
		if(showTaskInfo && taskMgr.CurrentTask != null)
		{
			GUIHelper.TextInfo(taskMgr.CurrentTask.TaskText,textCenterLoc);
		}
	}

	protected virtual void trackModeInit()
	{
		//record player data
		TextInfoAction tAction = new TextInfoAction();
		tAction.Record(PlayerInfo);
		ActionTracker.TrackAction(tAction);
	}

	protected virtual void trackModeUpdate()
	{
	}

	protected virtual void replayModeInit()
	{
	}
	
	protected virtual void replayModeUpdate()
	{
		if(ActionReplayer.IsPlaying == false)
		{
			onReplayEnd();
		}
	}
	
	// Update is called once per frame
	protected virtual void Update () {
		
		if (ActionTracker.IsTracking)
			ActionTracker.Update (Time.deltaTime);
		
		if (ActionReplayer.IsPlaying)
			ActionReplayer.Update (Time.deltaTime);

		if(SceneMode == SceneModeEnum.Record)
			trackModeUpdate();
		else if(SceneMode == SceneModeEnum.Replay)
			replayModeUpdate();

		if(PlayerInput.CurrentControlMode == PlayerInput.ControlMode.XBoxController &&
		   PlayerInput.IsAssistKeyDown())
			showTaskInfo = !showTaskInfo;

		taskMgr.UpdateCurrentTask ();
	}
	
	private bool showTaskInfo = false;
	private bool textCenterLoc = false;
	
	public void ShowTaskInfo(string text,bool textCenterLoc = false)
	{
		showTaskInfo = true;
		this.textCenterLoc = textCenterLoc;
	}
	
	public void StartTrack()
	{
		ActionTracker.StartTracking ();
		PlayerActionSampler.Instance.ResetSamplingTime ();
	}
	
	public void EndTrack()
	{
		ActionTracker.EndTracking ();
		ActionTracker.OutputTracking (dataFilePath,false,Append);
	}
	
	public void StartPlay()
	{
		ActionReplayer.LoadActions (dataFilePath);
		PlayerActionPlayer.Instance.ResetPlayer ();
		ActionReplayer.PlayFromBeginning ();
	}
	
	public void EndPlay()
	{
		ActionReplayer.EndPlaying ();
	}

	protected virtual void onReplayEnd()
	{

	}
}
