using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerToggle : MonoBehaviour {

	public enum ToggleType{ turnOn, turnOff, toggle } 						//finds toggle types

	public GameObject toggleObject;											//creates public gameobject slot named toggleObject
	public ToggleType toggleType;											//creates public toggletype slot named toggle types
	//------------------------------------------------------------------------------------------------------------------
	//		OnTriggerEnter2D()
	// We the player enters into the collider of a trigger
	//
	// Param:
	//				Collider2D other
	// Return:
	//				Void
	//------------------------------------------------------------------------------------------------------------------
	void OnTriggerEnter2D (Collider2D other)
	{
		if (other.tag == "Player")
		{
			if (toggleType == ToggleType.turnOn)
				toggleObject.SetActive (true);
			else if (toggleType == ToggleType.turnOff)
				toggleObject.SetActive (false);
			else if (toggleType == ToggleType.toggle)
				toggleObject.SetActive (!toggleObject.activeSelf);
		}
	}
}
