using UnityEngine;
using System.Collections;
using System.IO;
using System.Collections.Generic;

public static class ActionReplayer {

	#region static Memebers
	private static List<Action> actions = new List<Action>();
	private static bool isPlaying = false;
	private static bool isPausing = false;
	private static float timeFromStart = 0.0f;
	private static int playIdx = -1;
	#endregion
	
	#region Static Properties
	public static bool IsPlaying
	{
		get
		{
			return isPlaying;
		}
	}
	#endregion

	/// <summary>
	/// Load action from a  action data file
	/// </summary>
	public static void LoadActions(string filepath)
	{
		StreamReader sr = new StreamReader (filepath);
		if(sr != null)
		{
			actions.Clear();
			string line;
			Action action;
			while(!sr.EndOfStream)
			{
				line = sr.ReadLine();
				action = ActionFactory.Create(line);
				if(action != null)
				{
					actions.Add(action);
				}
			}
			sr.Close();
			Debug.Log(string.Format("{0} actions loaded !",actions.Count));
		}
		else
		{
			Debug.LogError(string.Format("Can't open {0}, load actions failed!",filepath));
		}
	}

	/// <summary>
	/// Start to play the actions from a certain start time
	/// </summary>
	/// <param name="startTime">Start time.</param>
	public static void StartPlaying(float startTime)
	{
		///now it always play from 0, improved one will be implemented in next version
		/// 
		if(!isPlaying)
		{
			playIdx = -1;
			isPlaying = true;
			isPausing = false;
			timeFromStart = 0.0f;
			Debug.Log("Start Replay!");
		}
	}

	/// <summary>
	/// Start to play actions from beginning( start time = 0 )
	/// </summary>
	public static void PlayFromBeginning()
	{
		StartPlaying(0);
	}

	public static void PausePlaying()
	{
		if(isPlaying && !isPausing)
		{
			isPausing = true;
		}
	}

	public static void ContinuePlaying()
	{
		if(isPlaying && isPausing)
		{
			isPausing = false;
		}
	}
	
	/// <summary>
	/// end the current play, 
	/// </summary>
	public static void EndPlaying()
	{
		Debug.Log ("Replay ended");
		isPlaying = false;
	}

	public static void Update(float delta)
	{
		if(isPlaying && !isPausing)
		{
			timeFromStart += delta;

			float lastEndTime = playIdx == -1? 0.0f :  actions[playIdx].EndTime;
			if(lastEndTime <= timeFromStart)
			{
				if(playIdx > -1)
					actions[playIdx].PlayEnd();
				playIdx++;
				if(playIdx < actions.Count)
					actions[playIdx].Play(actions[playIdx].EndTime);
				else
					EndPlaying();
			}
		}
	}
}
