using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathZone : MonoBehaviour {
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
		if (other.tag == "Player") {
			other.gameObject.GetComponent<Player> ().ResetPlayer ();
			Debug.Log (other.gameObject.GetComponent<Player> ());
		}
	}
}
