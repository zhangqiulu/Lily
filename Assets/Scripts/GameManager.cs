using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	public PlayerFPS player;

	public GameObject canvasSetting;
	
	public bool isSetting;
	
	void Update()
	{
		if (Input.GetKeyDown (KeyCode.Escape))
			isSetting = !isSetting;
		
		if(isSetting)
		{
			player.enabled = false;
			canvasSetting.gameObject.SetActive(true);
			Cursor.visible = true;
			Cursor.lockState = CursorLockMode.None;
		}
		
		if(!isSetting)
		{
			player.enabled = true;
			canvasSetting.gameObject.SetActive(false);
			Cursor.visible = false;
			Cursor.lockState = CursorLockMode.Locked;
		}
	}
}
