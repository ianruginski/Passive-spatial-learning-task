using UnityEngine;
using System.Collections.Generic;

public class TaskManager {

	private Queue<Task> taskQueue = new Queue<Task>();
	private Task currentTask = null;

	public Task CurrentTask
	{
		get{return currentTask;}
	}

	public void PushTask(Task task)
	{
		task.TaskMgr = this;
		taskQueue.Enqueue (task);
	}

	public void ClearTasks()
	{
		currentTask = null;
		taskQueue.Clear ();
	}

	/// <summary>
	/// return false means there is no tasks in the task queue 
	/// </summary>
	public bool NextTask()
	{
		if(currentTask != null)
			currentTask.TaskEnd();
		if(taskQueue.Count > 0)
		{
			currentTask = taskQueue.Dequeue();
			currentTask.TaskStart();
			return true;
		}
		else
		{
			currentTask = null;
			return false;
		}

	}

	public void UpdateCurrentTask()
	{
		if(currentTask != null)
			currentTask.TaskUpdate();
	}

}
