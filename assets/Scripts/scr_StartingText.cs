using UnityEngine;
using System.Collections;

public class scr_StartingText : MonoBehaviour {
	public string Number;
	public string Gender;
	public string Age;
	public string PlayBackSpeed;
	public int TextWidth;
	public int TextHeight;
	public int TrialNumber;
	public bool Record;
	public bool Trial;
	public bool TopView;
	public bool FirstPerson;
	public bool EnableRocks;
	private bool expStarted;
	// Use this for initialization
	void Start () {
		Number="Number";
		Gender="Gender";
		Age="Age";
		PlayBackSpeed="PlayBackSpeed";
		TextWidth=400;
		TextHeight=40;
		TrialNumber=1;
		expStarted=false;
		Record=false;
		Trial=false;
		EnableRocks=false;
	}
	
	// Update is called once per frame
	void Awake()
	{
		DontDestroyOnLoad(transform.gameObject);
	}
	void Update () {
	
	}
	void OnGUI()
	{
		if(!expStarted)
		{
			Number = GUI.TextField (new Rect (Screen.width/2-TextWidth/2,Screen.height/2-4*TextHeight, TextWidth, TextHeight), Number, 25);
			Gender = GUI.TextField (new Rect (Screen.width/2-TextWidth/2,Screen.height/2-2*TextHeight, TextWidth, TextHeight), Gender, 25);
			Age = GUI.TextField (new Rect (Screen.width/2-TextWidth/2,Screen.height/2, TextWidth, TextHeight), Age, 25);
			PlayBackSpeed = GUI.TextField (new Rect (Screen.width/2-TextWidth/2,Screen.height/2+2*TextHeight, TextWidth, TextHeight), PlayBackSpeed, 25);
			Record = GUI.Toggle(new Rect(Screen.width/2-TextWidth/2,Screen.height/2+4*TextHeight, TextWidth/2, TextHeight/2), Record, "Record");
			Trial = GUI.Toggle(new Rect(Screen.width/2,Screen.height/2+4*TextHeight, TextWidth/2, TextHeight/2), Trial, "Trial");
			EnableRocks = GUI.Toggle(new Rect(Screen.width/2-TextWidth/2,Screen.height/2+5*TextHeight, TextWidth/2, TextHeight/2), EnableRocks, "EnableRocks");
			if(GUI.Button(new Rect (Screen.width/2-TextWidth/2,Screen.height/2+6*TextHeight, TextWidth, TextHeight),"Begin Experiment"))
			{
				expStarted=true;
				int Temp=0;
				if(!int.TryParse(PlayBackSpeed,out Temp))
				{
					PlayBackSpeed="2";
				}

				if(EnableRocks)
			   		Application.LoadLevel (1);
				else
					Application.LoadLevel(2);
			}
		}
	}
}
