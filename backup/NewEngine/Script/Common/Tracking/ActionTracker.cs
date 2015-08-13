using UnityEngine;
using System.Collections;
using System.IO;
using System.Collections.Generic;

public static class ActionTracker {

	#region Static Memebers
	private static Queue<Action> actions = new Queue<Action>();
	private static bool isTrackingPuased = false;
	private static bool isTrackingStarted = false;
	private static float timeFromStart = 0.0f;
	#endregion

	#region Static Properties
	public static bool IsTracking
	{
		get
		{
			return isTrackingStarted;
		}
	}
	#endregion

	#region Tracking Functionsa
	/// <summary>
	/// clear the action queue and start the tracking process
	/// </summary>
	public static void StartTracking()
	{
		if(!isTrackingStarted)
		{
			actions.Clear();
			isTrackingPuased = false;
			isTrackingStarted = true;
			timeFromStart = 0.0f;
			Debug.Log("Tracking process stated!");
		}
		else
			Debug.LogError("can't start tracking because a started tracking process is existed.");
	}

	/// <summary>
	/// Puase the tracking process, which will not clear the tracking data and can be awaked again by Contirnue Tracking 
	/// </summary>a
	public static void PauseTracking()
	{
		if(isTrackingStarted && isTrackingPuased)
		{
			isTrackingPuased = true;
			Debug.Log("Tracking paused!");
		}
		else
			Debug.LogError("Pause failed, it needs a running tracking process to pause!");
	}

	/// <summary>
	/// Continue the paused tracking process 
	/// </summary>
	public static void ContinueTracking()
	{
		if(isTrackingStarted && !isTrackingPuased)
		{
			isTrackingPuased = false;
			Debug.Log("Tracking continued!");
		}
		else
			Debug.LogError("Can't continue because it needs a paused tracking process.");
	}

	/// <summary>
	/// End the tracking process
	/// </summary>
	public static void EndTracking()
	{
		if(isTrackingStarted)
		{
			isTrackingStarted = false;
			Debug.Log("Tracking ended.");
		}
		else
			Debug.LogError("Can't end a process because there isn't a stated tracking process!");

	}

	/// <summary>
	/// output tracked actions as a file, option to clean the exist aciton data
	/// </summary>
	public static void OutputTracking(string filepath, bool cleandata)
	{
		StreamWriter sw = new StreamWriter (filepath, false);
		int counter = 0;
		if(sw != null)
		{
			if(cleandata)
			{
				while(actions.Count > 0)
				{
					sw.WriteLine(actions.Dequeue().Save());
					counter++;
				}
			}
			else
			{
				foreach(Action action in actions)
				{
					sw.WriteLine(action.Save());
					counter++;
				}
			}

			sw.Close();
			Debug.Log(string.Format("{0} actions outputted!",counter));
		}
		else
		{
			Debug.LogError(string.Format("create/open {0} failed, stop output action data!",filepath));
		}
	}

	/// <summary>
	/// Add an action into the tracking list
	/// </summary>
	public static void TrackAction(Action action)
	{
		if(isTrackingStarted && !isTrackingPuased)
		{
			//update end time
			action.EndTime = timeFromStart;
			actions.Enqueue (action);
			Debug.Log(string.Format("New Tracking Action ({0}) Added on {1}s!",action.ToString(),timeFromStart));
		}
	}

	public static void Update(float delta)
	{
		if(isTrackingStarted && !isTrackingPuased)
		{
			timeFromStart += delta;
		}
	}


	#endregion

}
