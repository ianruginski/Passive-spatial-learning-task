using UnityEngine;
using System.Collections;

public class IEMainMenu : MonoBehaviour {

	private enum MenuState
	{
		MainMenu, TesterInput, RecodingMenu
	}

	private MenuState currentMenuState;

	void Start()
	{
		currentMenuState = MenuState.MainMenu;
	}

	void OnGUI()
	{
		switch(currentMenuState)
		{
		case MenuState.MainMenu:
			onGUIMainMenu();
			break;
		case MenuState.TesterInput:
			onGUITesterInput();
			break;
		case MenuState.RecodingMenu:
			onGUIRecodingMenu();
			break;
		default:
			break;
		}
	}

	#region MainMenu

	private void onGUIMainMenu()
	{
		Debug.Log ("aa");

		if(GUIHelper.Button(Screen.width * 0.5f,Screen.height * 0.5f,"Record"))
		{
			currentMenuState = MenuState.TesterInput;
		}

		if(GUIHelper.Button(Screen.width * 0.5f,Screen.height * 0.5f + 100,"Play"))
		{
			currentMenuState = MenuState.RecodingMenu;
		}
	}

	#endregion

	#region Tester Input

	private string pNum = "";
	private string gender = "";
	private string age = "";

	private void onGUITesterInput()
	{
		float offsetX = Screen.width * 0.1f;
		float offsetY = Screen.height * 0.1f;
		GUI.Label(new Rect (offsetX, offsetY, 200, 30), "PNumber");
		GUI.Label(new Rect (offsetX, offsetY + 40, 200, 30), "Gender");
		GUI.Label(new Rect (offsetX, offsetY + 80, 200, 30), "Age");
		
		pNum = GUI.TextField (new Rect (offsetX + 200, offsetY, 200, 30), pNum);
		gender = GUI.TextField (new Rect (offsetX + 200, offsetY + 40, 200, 30), gender);
		age = GUI.TextField (new Rect (offsetX + 200, offsetY + 80, 200, 30), age);
		
		if(GUIHelper.Button(offsetX + 100,offsetY + 130,"OK"))
		{
			//load next level
			IEExperiment.dataFilePath = "test.dat";
			IEExperiment.PlayerInfo = string.Format("PNumber:{0},Gender:{1},Age:{2}",pNum,gender,age);
			IEExperiment.SceneMode = SceneBase.SceneModeEnum.Record;
			
			Application.LoadLevel("KEExperiment");
		}
	}

	#endregion

	#region Recoding Menu

	private void onGUIRecodingMenu()
	{

		float offsetX = Screen.width * 0.5f;
		float offsetY = Screen.height * 0.5f;
		for(int i =0;i<6;i++)
		{
			if(GUIHelper.Button(offsetX + 100,offsetY + 130,"OK"))
			{
				//load next level
				IEExperiment.dataFilePath = "test.dat";
				IEExperiment.PlayerInfo = string.Format("PNumber:{0},Gender:{1},Age:{2}",pNum,gender,age);
				IEExperiment.SceneMode = SceneBase.SceneModeEnum.Record;
				
				Application.LoadLevel("KEExperiment");
			}
		}
	}

	#endregion
}
