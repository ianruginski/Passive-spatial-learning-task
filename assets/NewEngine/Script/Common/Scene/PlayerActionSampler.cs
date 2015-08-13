using UnityEngine;
using System.Collections;

public class PlayerActionSampler : MonoBehaviour {

	#region Singleton
	private static PlayerActionSampler instance = null;
	public static PlayerActionSampler Instance
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
	public float SampleRate = 30.0f;
	public float SampleDiff = 0.5f;
	#endregion

	#region Memebers
	private float timeCounter = 0.0f;
	private Vector3 lastSamplePos;
	private Vector3 lastSampleDir;
	private bool firstTimeSample;
	#endregion

	#region Functions
	// Use this for initialization
	void Awake () {
		instance = this;
	}
	
	// Update is called once per frame
	void Update () {
		if (ActionTracker.IsTracking)
			updateSampling (Time.deltaTime);
	}

	private void updateSampling(float delta)
	{
		timeCounter += delta;
		float interval = 1.0f / SampleRate;
		if(timeCounter > interval)
		{
			timeCounter -= interval;

			//tracks movement and direction

			if(firstTimeSample || Vector3.Distance(lastSamplePos,PlayerObject.Position) >= SampleDiff)
			{
				MovementAction mAction = new MovementAction();
				mAction.Record(PlayerObject.Position);
				ActionTracker.TrackAction(mAction);
				lastSamplePos = PlayerObject.Position;
			}

			if(firstTimeSample || Vector3.Distance(lastSampleDir,PlayerObject.DirectionAngle) >= SampleDiff)
			{
				LookAction lAction = new LookAction();
				lAction.Record(PlayerObject.DirectionAngle);
				ActionTracker.TrackAction(lAction);
				lastSampleDir = PlayerObject.DirectionAngle;
			}

			if(firstTimeSample)
				firstTimeSample = false;

		}
	}

	public void ResetSamplingTime()
	{
		timeCounter = 1.0f / SampleRate;
		firstTimeSample = true;
	}

	#endregion
}
