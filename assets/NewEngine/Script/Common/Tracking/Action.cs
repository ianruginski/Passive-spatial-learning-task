using UnityEngine;
using System.Collections;

public abstract class Action{

	public enum ActionTypes
	{
		Movement,
		Look,
		Interact,
		TextInfo,
		Teleport
	}

	#region Memebers
	private float endtime = 0.0f;
	#endregion

	#region Properties
	/// <summary>
	/// get the end time (from the tracking start time) of this game action 
	/// </summary>
	public float EndTime{get{return endtime;}set{endtime = value;}}
	public virtual ActionTypes ActionType{get{return (ActionType)-1;}}
	#endregion

	#region Functions
	/// <summary>
	/// Convert real game data to a string line
	/// </summary>
	public virtual void Record(object info)
	{}

	/// <summary>
	/// Load Data From a String Line
	/// </summary>
	public void Load(string line)
	{
		load (line.Split (','));
	}

	protected virtual void load(string[] strs)
	{
		endtime = float.Parse (strs [1]);
	}

	/// <summary>
	/// get the save string line of this action
	/// </summary>
	public virtual string Save()
	{
		return ((int)ActionType).ToString () + "," + endtime.ToString("0.00") + ",";
	}

	/// <summary>
	/// play this action depens on the time from start
	/// </summary>
	/// <param name="timeFromStart">Time from start.</param>
	public virtual void Play(float endTime)
	{
	}

	public virtual void PlayEnd()
	{

	}

	#endregion
}

public static class ActionFactory
{
	/// <summary>
	/// Create an action from a string line
	/// </summary>
	public static Action Create(string line)
	{
		Action.ActionTypes actionType = (Action.ActionTypes)int.Parse(line.Substring (0, line.IndexOf (',')));
		switch(actionType)
		{
		case Action.ActionTypes.Movement:
			MovementAction mAction = new MovementAction();
			mAction.Load(line);
			return mAction;
			break;
		case Action.ActionTypes.Look:
			LookAction lAction = new LookAction();
			lAction.Load(line);
			return lAction;
			break;
		case Action.ActionTypes.Interact:
			InteractAction iAction = new InteractAction();
			iAction.Load(line);
			return iAction;
			break;
		case Action.ActionTypes.TextInfo:
			TextInfoAction tAction = new TextInfoAction();
			tAction.Load(line);
			return tAction;
			break;
		case Action.ActionTypes.Teleport:
			TeleportAction tlAction = new TeleportAction();
			tlAction.Load(line);
			return tlAction;
			break;
		default:
			return null;
			break;
		}
	}
}

#region Actions

public class MovementAction : Action
{
	Vector3 position;
	public override ActionTypes ActionType {
		get {
			return ActionTypes.Movement;
		}
	}
	protected override void load (string[] strs)
	{
		base.load (strs);
		position = new Vector3 (float.Parse (strs [2]), float.Parse (strs [3]), float.Parse (strs [4]));
	}
	public override string Save ()
	{
		return base.Save () + position.x.ToString("0.00") + "," + position.y.ToString("0.00") + "," + position.z.ToString("0.00");
	}
	public override void Play (float endTime)
	{
		base.Play (endTime);
		PlayerActionPlayer.Instance.PlayNextMovementTarget (position,endTime);
	}

	/// <summary>
	/// Convert real game data to a string line, Info: the position
	/// </summary>
	public override void Record (object info)
	{
		position = (Vector3)info;
	}
}

public class LookAction : Action
{
	Vector3 direction;
	public override ActionTypes ActionType {
		get {
			return ActionTypes.Look;
		}
	}
	protected override void load (string[] strs)
	{
		base.load (strs);
		direction = new Vector3 (float.Parse (strs [2]), float.Parse (strs [3]), float.Parse (strs [4]));
	}
	public override string Save ()
	{
		return base.Save () + direction.x.ToString("0.00") + "," + direction.y.ToString("0.00") + "," + direction.z.ToString("0.00");
	}


	public override void Play (float endTime)
	{
		base.Play (endTime);
		PlayerActionPlayer.Instance.PlayNextLookTarget (direction, endTime);
	}

	/// <summary>
	/// Convert real game data to a string line, Info: the face direction vector
	/// </summary>
	public override void Record (object info)
	{
		direction = (Vector3)info;
	}
}

public class InteractAction : Action
{
	DynamicObject targetObj;	
	public override ActionTypes ActionType {
		get {
			return ActionTypes.Interact;
		}
	}
	protected override void load (string[] strs)
	{
		base.load (strs);
		targetObj = SceneObjectMgr.Instance.FindDynamicObject (strs [2]);
	}
	public override string Save ()
	{
		return base.Save () + targetObj.name;
	}
	public override void PlayEnd ()
	{
		targetObj.Interact ();
		base.PlayEnd ();
	}

	/// <summary>
	/// Convert real game data to a string line , Info: the DynamicObject
	/// </summary>
	public override void Record (object info)
	{
		targetObj = (DynamicObject)info;
	}
}

public class TextInfoAction : Action
{
	string textInfo;
	public override ActionTypes ActionType {
		get {
			return ActionTypes.TextInfo;
		}
	}
	protected override void load (string[] strs)
	{
		base.load (strs);
	}
	public override string Save ()
	{
		return base.Save () + textInfo;
	}
	public override void PlayEnd ()
	{
		base.PlayEnd ();
	}
	
	/// <summary>
	/// Convert real game data to a string line , Info: the DynamicObject
	/// </summary>
	public override void Record (object info)
	{
		textInfo = (string)info;
	}
}

public class TeleportAction : Action
{
	public struct TeleportTarget
	{
		public Vector3 Position;
		public Vector3 DirectionAngle;
	}
	TeleportTarget teleportTarget;
	public override ActionTypes ActionType {
		get {
			return ActionTypes.Teleport;
		}
	}
	protected override void load (string[] strs)
	{
		base.load (strs);
		teleportTarget.Position = new Vector3 (float.Parse (strs [2]), float.Parse (strs [3]), float.Parse (strs [4]));
		teleportTarget.DirectionAngle = new Vector3 (float.Parse (strs [5]), float.Parse (strs [6]), float.Parse (strs [7]));
	}
	public override string Save ()
	{
		return base.Save () +
				teleportTarget.Position.x.ToString("0.00") + "," + 
				teleportTarget.Position.y.ToString("0.00") + "," + 
				teleportTarget.Position.z.ToString("0.00") + "," +
				teleportTarget.DirectionAngle.x.ToString("0.00") + "," + 
				teleportTarget.DirectionAngle.y.ToString("0.00") + "," + 
				teleportTarget.DirectionAngle.z.ToString("0.00");
	}
	public override void PlayEnd ()
	{
		base.PlayEnd ();
		PlayerObject.Instance.Position = teleportTarget.Position;
		PlayerObject.Instance.DirectionAngle = teleportTarget.DirectionAngle;
	}
	
	/// <summary>
	/// Convert real game data to a string line , Info: TeleportTarget
	/// </summary>
	public override void Record (object info)
	{
		teleportTarget = (TeleportTarget)info;
	}
}
#endregion
