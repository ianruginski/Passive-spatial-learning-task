using UnityEngine;
using System.Collections;

public class scr_ItemFound : MonoBehaviour {
	public scr_RecordPosition playerScript;
	public int objectID;
	private bool inside;
	public static int TotalItemsFound;
	bool ItemRecorded;
	// Use this for initialization
	void Awake(){
		TotalItemsFound=0;
	}
	void Start () {
		inside=false;
		ItemRecorded=false;
	}
	
	// Update is called once per frame
	void Update () {

	}
	void OnTriggerEnter(Collider i_Body)
	{
		if(i_Body.gameObject.tag=="Player" && !ItemRecorded)
		{
			if(objectID!=6)
			{
				playerScript.displayGUI=true;
				inside=true;
			}
			if(objectID==6 && TotalItemsFound==5)
			{
				playerScript.finalGUI=true;
				inside=true;
			}
		}
	}

	void OnTriggerExit(Collider i_Body)
	{
		if(i_Body.gameObject.tag=="Player")
		{
			playerScript.displayGUI=false;
			playerScript.finalGUI=false;
			inside=false;
		}
	}
}
