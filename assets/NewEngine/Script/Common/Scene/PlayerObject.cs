using UnityEngine;
using System.Collections;

public class PlayerObject : MonoBehaviour {



	//Singleton
	private static PlayerObject s_instance;
	public static PlayerObject Instance{ get { return s_instance; } }

	public GameObject CameraObj;
	public MonoBehaviour CharacterMotor, MouseLooking, ControllerLooking;
	
	// Use this for initialization
	void Awake () {
		s_instance = this;
		EnableLooking (true);
		EnableMovement (true);
	}

	public Vector3 Position
	{
		get{return this.transform.position;}
		set{this.transform.position = value;}
	}

	public Vector3 DirectionAngle
	{
		get{ return this.transform.localEulerAngles;}
		set{ this.transform.localEulerAngles = value;}
	}

	public Vector3 Direction
	{
		get{return this.transform.forward;}
	}

	public void TeleportTo(Vector3 pos,Vector3 dir)
	{
		PlayerObject.Instance.Position = pos;
		PlayerObject.Instance.DirectionAngle = dir;

		TeleportAction action = new TeleportAction();
		TeleportAction.TeleportTarget teleportTarget;
		teleportTarget.Position = pos;
		teleportTarget.DirectionAngle= dir;
		action.Record(teleportTarget);
		ActionTracker.TrackAction(action);
	}

	public void EnableLooking(bool enable)
	{
		if(!enabled)
		{
			MouseLooking.enabled = false;
			ControllerLooking.enabled = false;
		}
		else
		{
			if(PlayerInput.CurrentControlMode == PlayerInput.ControlMode.KeyboardAndMouse)
			{
				MouseLooking.enabled = true;
				ControllerLooking.enabled = false;
			}
			else if(PlayerInput.CurrentControlMode == PlayerInput.ControlMode.XBoxController)
			{
				MouseLooking.enabled = false;
				ControllerLooking.enabled = true;
			}
		}
	}

	public void EnableMovement(bool enable)
	{
		CharacterMotor.enabled = enable;
	}
}
