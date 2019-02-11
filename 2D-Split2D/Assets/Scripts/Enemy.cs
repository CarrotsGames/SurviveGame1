using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour {

	public float knockBackPower = 0.5f;									//public varible for knockbackpower
	public float detectionRange = 6;									//public  varible for detectionRange
	public GameObject player;											//public Gameobject which is the player 	
	private NavMeshAgent navAgent;										//grabs navmesh

	public float health = 2;											//creates health varibles
	//------------------------------------------------------------------------------------------------------------------
	//		TakeDamage()
	// Calculates the death of the enemy
	//
	// Param:
	//				float damage
	// Return:
	//				Void
	//------------------------------------------------------------------------------------------------------------------
	public void TakeDamage (float damage) {
		health = health - damage;
		if (health >= 0) {
			GetComponent<Rigidbody2D> ().AddForce (transform.forward * -1 * knockBackPower, ForceMode2D.Impulse);
		}
		if (health <= 0) {
 			Destroy (this.gameObject);
		}
	}

	//------------------------------------------------------------------------------------------------------------------
	//		Start()
	// Runs during initialisation
	//
	// Param:
	//				None
	// Return:
	//				Void
	//------------------------------------------------------------------------------------------------------------------
	void Start () {
		player = GameObject.FindGameObjectWithTag ("Player");
		navAgent = GetComponent<NavMeshAgent> ();
		
	}
	
	// Update is called once per frame
	void Update () {

		if (Vector2.Distance (transform.position, player.transform.position) < detectionRange) {
		
			navAgent.destination = player.transform.position;										//follows the players position
		}
		
	}
}
