using UnityEngine;
using System.Collections;

public class Task {

	public delegate bool TaskProcessDeleagete(Task task);

	public string TaskText = "Task Text";

	public TaskManager TaskMgr = null;

	public event TaskProcessDeleagete OnTaskStart;
	public event TaskProcessDeleagete OnTaskProgressCheck; // return true means the task is completed
	public event TaskProcessDeleagete OnTaskEnd;

	public void TaskStart()
	{
		if(OnTaskStart != null)
			OnTaskStart.Invoke (this);
	}

	public void TaskUpdate()
	{
		bool completed = false;
		if (OnTaskProgressCheck != null)
			completed = OnTaskProgressCheck.Invoke (this);
		if(completed && TaskMgr != null)
			TaskMgr.NextTask ();
	}

	public void TaskEnd()
	{
		if(OnTaskEnd != null)
			OnTaskEnd.Invoke(this);
	}

}