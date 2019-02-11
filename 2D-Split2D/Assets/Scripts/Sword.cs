using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XboxCtrlrInput;

public class Sword : MonoBehaviour {


	public float damage = 0 ;									// creates damage varibles
	//------------------------------------------------------------------------------------------------------------------
	//		OnTriggerEnter2D()
	// We the player enters into the collider of a trigger
	//
	// Param:
	//				Collider2D other
	// Return:
	//				Void
	//------------------------------------------------------------------------------------------------------------------
	void OnTriggertEnter(Collider2D other) {
		if (other.tag == "Enemy")
			other.GetComponent<Enemy> ().TakeDamage (damage);
	}

}
