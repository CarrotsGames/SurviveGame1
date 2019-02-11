using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackScript : MonoBehaviour {

    float Damage = 2;
    Enemy enemy;
    GameObject EnemyGameObject;
	// Use this for initialization
	void Start () {
        EnemyGameObject = GameObject.FindGameObjectWithTag("Enemy");
        enemy = EnemyGameObject.GetComponent<Enemy>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            Destroy(this.EnemyGameObject);
         }
    }

}
