using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SceneObjectMgr : MonoBehaviour {

	#region Singleton
	private static SceneObjectMgr instance = null;
	public static SceneObjectMgr Instance
	{
		get
		{
			if(instance == null)
				Debug.LogError("must be added to scene to init the instance before using!");
			return instance;
		}
	}
	#endregion

	#region Memebers
	private Dictionary<string,DynamicObject> dynamicObjDict = new Dictionary<string, DynamicObject>();
	#endregion

	#region Functions
	void Awake()
	{
		instance = this;

		//add dynamic objects to dict
		foreach(DynamicObject dObj in DynamicObjects)
		{
			if(dynamicObjDict.ContainsKey(dObj.name))
			{
				Debug.LogError(string.Format("Can not add {0} to the dynamic object dict because it already contain the same key!", dObj.name));
			}
			else
			{
				dynamicObjDict.Add(dObj.name, dObj);
			}
		}
	}

	public DynamicObject FindDynamicObject(string name)
	{
		DynamicObject dObj = null;
		if(!dynamicObjDict.TryGetValue(name,out dObj))
		{
			Debug.Log(string.Format("Cannot find {0} in the dynamic object dict",name));
		}
		return dObj;
	}
	#endregion
	
	#region Public Memebers for Unity
	public DynamicObject[] DynamicObjects;
	public PlayerObject PlayerObject;
	#endregion
}
