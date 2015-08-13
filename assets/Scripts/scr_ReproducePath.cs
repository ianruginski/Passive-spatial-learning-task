using UnityEngine;
using System.Collections;

public class scr_ReproducePath : MonoBehaviour {
	Vector3 CurrentVertex;
	public Camera TopCamera;
	public string DataPath;

	//Line Renderer
	private LineRenderer RendererComponent;
	// Variables used to draw a smooth line
	private RaycastHit PreviousHit;
	private RaycastHit CurrentHit;

	public GameObject LevelObjects;
	//Strings to store object names
	private string[] Objects;
	public GUIStyle Style;

	private Vector3[] ObjectPositions;
	private Vector3[] MousePositions;
	private int ObjectNumber;
	private int VertexCount;
	private Vector3[] VertexBuffer;
	// Use this for initialization
	void Start () {
		RendererComponent=(LineRenderer)GetComponent(typeof(LineRenderer));
		Objects=new string[3]{"ObjectName","ObjectName","ObjectName"};
		ObjectPositions=new Vector3[3];
		MousePositions = new Vector3[3];
		ObjectNumber = 0;
		LevelObjects.SetActive (false);
		VertexCount=0;
		VertexBuffer=new Vector3[2000];
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyUp(KeyCode.KeypadPlus))
		{
			if(ObjectNumber<ObjectPositions.Length)
			{
				//Clear the data points collected

				//System.IO.File.WriteAllText (DataPath,"");

				Ray ray = TopCamera.ScreenPointToRay (Input.mousePosition);	
				CurrentHit=new RaycastHit();
				if (Physics.Raycast (ray,out CurrentHit, 1005.0f)) 
				{
					
				}
				ObjectPositions[ObjectNumber]=CurrentHit.point;
				MousePositions[ObjectNumber]=new Vector3(Input.mousePosition.x,Screen.height-Input.mousePosition.y,0.0f);
				ObjectNumber++;
			}
		}
		if(Input.GetKeyUp(KeyCode.Backspace))
		{
			ResetBoxes();
			System.IO.File.WriteAllText (DataPath,"");
		}

		if(Input.GetKeyUp(KeyCode.Escape))
		{
			System.IO.File.WriteAllText(DataPath,"");
			if(ObjectNumber>=3)
			{
				for(int i=0;i<Objects.Length;i++)
					System.IO.File.AppendAllText (DataPath,"Object Name:"+Objects[i]+"    "+"Position:"+ObjectPositions[i] +"\r\n");
			}
			Application.Quit();
		}

	}
	void OnGUI()
	{

		if(ObjectNumber>=1)
			Objects[0] = GUI.TextField (new Rect (MousePositions[0].x,MousePositions[0].y, 100, 20), Objects[0], 25);
		if(ObjectNumber>=2)
			Objects[1] = GUI.TextField (new Rect (MousePositions[1].x,MousePositions[1].y,100, 20), Objects[1], 25);
		if(ObjectNumber>=3)
		{
			Objects[2] = GUI.TextField (new Rect (MousePositions[2].x,MousePositions[2].y, 100, 20), Objects[2], 25);
			GUI.Label(new Rect(Screen.width/2-500,Screen.height*7/8,1000,20),"Press Backspace To Re-record locations,\n Or Press Escape To Finish Experiment",Style);
		}
		else
		{
			GUI.Label(new Rect(Screen.width/2-500,Screen.height*7/8,1000,20),"Press + To Record Locations",Style);

		}
	}

	void ResetBoxes()
	{
		Objects=new string[3]{"ObjectName","ObjectName","ObjectName"};
		ObjectNumber = 0;
	}
}