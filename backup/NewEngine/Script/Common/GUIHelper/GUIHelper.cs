using UnityEngine;
using System.Collections;

public static class GUIHelper {

	public static int FontSize = 30;

	public static void SetFontSize()
	{
		GUI.skin.label.fontSize = FontSize;
		GUI.skin.button.fontSize = FontSize;
		GUI.skin.textArea.fontSize = FontSize;
		GUI.skin.textField.fontSize = FontSize;
	}

	public static bool Button(float cx, float cy,string text, float width = 100)
	{
		//if(PlayerInput.CurrentControlMode == PlayerInput.ControlMode.XBoxController) 
		//	text += " (A)";
		GUI.skin.button.fontSize = FontSize;

		if(GUI.Button(new Rect(cx - 50, cy -15 , width,50),text)) //|| 
		//   (PlayerInput.CurrentControlMode == PlayerInput.ControlMode.XBoxController && PlayerInput.IsInteractiveKeyDown()))
		{
			return true;
		}
		else
			return false;
	}

	public static void TextInfo(string info, bool center)
	{
		GUI.skin.textArea.fontSize = FontSize;



		//backBG mode
		if(center)
		{
			GUI.TextArea (new Rect (Screen.width * 0.2f, Screen.height * 0.3f, Screen.width * 0.6f, Screen.height * 0.4f), info);
		}
		else
		{
			GUI.TextArea (new Rect (Screen.width * 0.2f, Screen.height * 0.75f, Screen.width * 0.6f, Screen.height * 0.2f), info);
		}
	}

	public static bool TextBox(string info)
	{
		TextInfo (info,false);
		return Button (Screen.width * 0.5f, Screen.width * 0.6f, "OK");
	}

	public static void LableAlpha(string text, float alpha)
	{
		GUIStyle labelCenter = GUI.skin.GetStyle ("Label");
		labelCenter.alignment = TextAnchor.MiddleCenter;

		Color guiColor = GUI.color;
		float currentAlpha = guiColor.a;

		guiColor.a = alpha;
		GUI.color = guiColor;
		GUI.Label (new Rect (0, Screen.height * 0.4f, Screen.width * 1.0f, Screen.height * 0.2f), text,labelCenter);

		guiColor.a = currentAlpha;
		GUI.color = guiColor;
	}
}
