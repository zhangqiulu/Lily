using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UISettings : MonoBehaviour {

	public PlayerFPS player;

	public void SliderSpeedMove(Slider s)
	{
		player.speedMove = s.value;
	}

	public void SliderSmoothMove(Slider s)
	{
		player.smoothMove = s.value;
	}

	public void SliderSpeedMouse(Slider s)
	{
		player.speedMouse = s.value;
	}

	public void SliderSmoothMouse(Slider s)
	{
		player.smoothMouse = s.value;
	}


	// ----------------- Weapons ------------------------ //



	public void SliderMaxWeaponX(Slider s)
	{
		player.w_maxRotateY = s.value;
	}

	public void SliderMaxWeaponY(Slider s)
	{
		player.w_maxRotateX = s.value;
	}

	public void SliderSpeedWeapon(Slider s)
	{
		player.w_speedWeapon = s.value;
	}

	public void SliderMarginAccuracy(Slider s)
	{
		player.marginAccuracy = s.value;
	}
}
