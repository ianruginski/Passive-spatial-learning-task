using UnityEngine;
using System.Collections;

public class PlayerInput{

	public enum ControlMode
	{
		XBoxController,
		KeyboardAndMouse
	}

	//public static ControlMode CurrentControlMode = ControlMode.KeyboardAndMouse;
	public static ControlMode CurrentControlMode = ControlMode.XBoxController;

	public static bool IsInteractiveKeyDown()
	{
		if(CurrentControlMode == ControlMode.KeyboardAndMouse)
		{
			return Input.GetMouseButtonDown(0); // left mouse btn
		}
		else if(CurrentControlMode == ControlMode.XBoxController)
		{
			return Input.GetButtonDown("Controller A");
		}
		else
			return false;
	}

	public static bool IsAssistKeyDown()
	{
		if(CurrentControlMode == ControlMode.KeyboardAndMouse)
		{
			return Input.GetMouseButtonDown(1); // left mouse btn
		}
		else if(CurrentControlMode == ControlMode.XBoxController)
		{
			return Input.GetButtonDown("Controller B");
		}
		else
			return false;
	}
}
