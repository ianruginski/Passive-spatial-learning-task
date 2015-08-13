using UnityEngine;
using System.Collections;

public class PlayerActionPlayer : MonoBehaviour {

	private class TimedVector3
	{
		#region Memebers
		public Vector3 VecFrom, VecTo, VecCurrent;
		public float Duration;
		private float timer;
		#endregion

		public void Set(Vector3 from, Vector3 to, float duration)
		{
			VecFrom = from;
			VecTo = to;

			VecFrom.x = checkNearestFrom (VecFrom.x, VecTo.x);
			VecFrom.y = checkNearestFrom (VecFrom.y, VecTo.y);
			VecFrom.z = checkNearestFrom (VecFrom.z, VecTo.z);

			VecCurrent = VecFrom;
			Duration = duration;
			timer = 0.0f;
		}

		public void Update(float delta)
		{
			if (timer <= Duration) 
			{
				float r = Duration == 0.0f? 1.0f :  timer / Duration;
				VecCurrent = Vector3.Lerp(VecFrom,VecTo,r);
				timer += delta;
			}
		}

		private static float checkNearestFrom(float from, float to)
		{
			//350 - 10
			if (to + 360 - from < 180) 
				return from - 360;
			//10 - 350
			else if (from + 360 - to < 180)
				return from + 360;
			else
				return from;
		}
	}

	#region Singleton
	private static PlayerActionPlayer instance = null;
	public static PlayerActionPlayer Instance
	{
		get
		{
			if(instance == null)
				Debug.LogError("must be added to scene to init the instance before using!");
			return instance;
		}
	}
	#endregion
	
	#region Public Memebers for Unity
	public PlayerObject PlayerObject;
	#endregion
	
	#region Memebers
	private TimedVector3 vecMove,vecLook;
	private float lastMoveEndTime,lastLookEndTime;
	#endregion
	
	#region Functions
	// Use this for initialization
	void Awake () {
		instance = this;
		vecMove = new TimedVector3 ();
		vecLook = new TimedVector3 ();
		lastMoveEndTime = 0;
		lastLookEndTime = 0;
	}

	void Update()
	{
		if(ActionReplayer.IsPlaying)
		{
			vecMove.Update (Time.deltaTime);
			vecLook.Update (Time.deltaTime);

			PlayerObject.Position = vecMove.VecCurrent;
			PlayerObject.DirectionAngle = vecLook.VecCurrent;
			Debug.Log(vecLook.Duration.ToString() + vecLook.VecFrom.ToString() + vecLook.VecTo.ToString());
		}
	}

	public void ResetPlayer()
	{
		vecMove.Set (PlayerObject.Position, PlayerObject.Position, 0.0f);
		vecLook.Set (PlayerObject.DirectionAngle, PlayerObject.DirectionAngle, 0.0f);
		lastMoveEndTime = 0;
		lastLookEndTime = 0;
	}

	public void PlayNextMovementTarget(Vector3 pos, float endTime)
	{
		Debug.Log(string.Format("Next postion:{0}",pos.ToString()));
		vecMove.Set (vecMove.VecTo, pos, endTime - lastMoveEndTime);
		lastMoveEndTime = endTime;
	}

	public void PlayNextLookTarget(Vector3 dir, float endTime)
	{
		vecLook.Set (vecLook.VecTo, dir, endTime - lastLookEndTime);
		lastLookEndTime = endTime;
	}

	#endregion
}
