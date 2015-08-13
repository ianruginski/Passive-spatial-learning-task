using UnityEngine;
using System.Collections;

public class IETesterInfo : MonoBehaviour {

	private string pNum = "";
	private string gender = "";
	private string age = "";

	private bool beforeVideo = false;

	public bool IanVersion = true;

	void OnGUI()
	{
		if(!beforeVideo)
		{

			GUIHelper.SetFontSize();
			float offsetX = Screen.width * 0.5f - 200;
			float offsetY = Screen.height * 0.5f - 100;

			if(!IanVersion)
			{


				GUI.Label(new Rect (offsetX, offsetY, 200, 50), "PNumber");
				GUI.Label(new Rect (offsetX, offsetY + 60, 200, 50), "Gender");
				GUI.Label(new Rect (offsetX, offsetY + 120, 200, 50), "Age");
				
				pNum = GUI.TextField (new Rect (offsetX + 200, offsetY, 200, 50), pNum);
				gender = GUI.TextField (new Rect (offsetX + 200, offsetY + 60, 200, 50), gender);
				age = GUI.TextField (new Rect (offsetX + 200, offsetY + 120, 200, 50), age);

				if(GUIHelper.Button(offsetX + 150,offsetY + 200,"Start"))
				{
					beforeVideo = true;
					Camera.main.gameObject.SetActive(false);
				}

			}
			else
			{
				if(GUIHelper.Button(offsetX + 300,offsetY + 260,"Ian_Record",250))
				{
					//load next level
					IEExperiment.dataFilePath = "Ian_Replay.dat"; 
					IEExperiment.PlayerInfo = string.Format("PNumber:{0},Gender:{1},Age:{2}",pNum,gender,age);
					IEExperiment.SceneMode = SceneBase.SceneModeEnum.Record;
					IEExperiment.CurrentExpState = IEExperiment.ExperiementState.Designer_Recoding;
					
					Application.LoadLevel("IEExperiment");
				}

				if(GUIHelper.Button(offsetX + 300,offsetY + 320,"Ian_Replay",250))
				{
					//load next level
					IEExperiment.dataFilePath = "Ian_Replay.dat"; // SAVE_PNumber_month_day_year_hour_minutes
					IEExperiment.SceneMode = SceneBase.SceneModeEnum.Replay;
					IEExperiment.CurrentExpState = IEExperiment.ExperiementState.Designer_Replaying;
					
					Application.LoadLevel("IEExperiment");
				}

				if(GUIHelper.Button(offsetX + 10,offsetY + 260,"Ian_Record2",250))
				{
					//load next level
					IEExperiment.dataFilePath = "Ian_Replay2.dat"; 
					IEExperiment.PlayerInfo = string.Format("PNumber:{0},Gender:{1},Age:{2}",pNum,gender,age);
					IEExperiment.SceneMode = SceneBase.SceneModeEnum.Record;
					IEExperiment.CurrentExpState = IEExperiment.ExperiementState.Designer_Recoding;
					
					Application.LoadLevel("IEExperiment");
				}
				
				if(GUIHelper.Button(offsetX + 10,offsetY + 320,"Ian_Replay2",250))
				{
					//load next level
					IEExperiment.dataFilePath = "Ian_Replay2.dat"; // SAVE_PNumber_month_day_year_hour_minutes
					IEExperiment.SceneMode = SceneBase.SceneModeEnum.Replay;
					IEExperiment.CurrentExpState = IEExperiment.ExperiementState.Designer_Replaying;
					
					Application.LoadLevel("IEExperiment");
				}
			}
		}
		else
		{
			GUIHelper.TextInfo("You will now be shown a video of a route, as you were traveling through a virtual environment. Please try to learn the environment to the best of your ability, since your knowledge of the environment will be assessed later in the experiment. Please pay attention to the locations of the major named landmarks encountered in the environment. Please press “A” on the controller to begin the video.",true);
		}
	}

	void Update()
	{
		if(beforeVideo && PlayerInput.IsInteractiveKeyDown())
		{
			//load next level
			IEExperiment.dataFilePath = string.Format("Ian_Replay.dat");
			IEExperiment.PlayerInfo = string.Format("PNumber:{0},Gender:{1},Age:{2}",pNum,gender,age);
			IEExperiment.PNum = pNum;
			IEExperiment.SceneMode = SceneBase.SceneModeEnum.Replay;
			IEExperiment.CurrentExpState = IEExperiment.ExperiementState.Player_Replaying;
			
			Application.LoadLevel("IEExperiment");
		}
	}
}
