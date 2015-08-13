using UnityEngine;
using System.Collections;

public class AimCross : MonoBehaviour {

	public Texture Image;
	public float Size = 0.05f;

	void OnGUI()
	{
		GUI.DrawTexture(new Rect(Screen.width * 0.5f - Size * Screen.height, Screen.height * (0.5f - Size),Screen.height * Size, Screen.height * Size),Image);
	}
}
