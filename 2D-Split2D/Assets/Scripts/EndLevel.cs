using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndLevel : MonoBehaviour {

	// grabs player gameobject
	private GameObject player;
	//------------------------------------------------------------------------------------------------------------------
	//		Start()
	//  Runs during initalisation
	//
	// Param:
	//				None
	// Return:
	//				Void
	//------------------------------------------------------------------------------------------------------------------
	void Start () {
		player = GameObject.FindGameObjectWithTag ("Player");
	}
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
			player.GetComponent<Controller2D> ().NextScene ();
	}
}
