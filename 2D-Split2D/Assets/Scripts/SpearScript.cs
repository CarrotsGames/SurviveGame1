using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XboxCtrlrInput;

public class SpearScript : MonoBehaviour {

    Player PlayerScript;
 
    GameObject PlayerGameObj;
    
    public GameObject Spear;
    public Vector2 Velocity;
    bool canShoot = true;
    public float coolDown = 1;
    public Vector2 offset = new Vector2 (0.4f,0);
    // Use this for initialization
    void Start ()
    {
        PlayerGameObj = GameObject.FindGameObjectWithTag("Player");
        PlayerScript = PlayerGameObj.GetComponent<Player>();

   
    }
	
	// Update is called once per frame
	void Update ()
    {
        if(!canShoot)
        {
            coolDown -= Time.deltaTime;

            if(coolDown <= 0)
            {
                canShoot = true;
                coolDown = 1;
            }
        }
         
        if (XCI.GetButton(XboxButton.RightBumper))
        {
            Debug.Log("RB DOWN");

             if (XCI.GetAxis(XboxAxis.LeftStickX) >= 0.25 && canShoot)
            {
 
               GameObject Go = (GameObject)Instantiate(Spear,(Vector2)transform.position + offset * transform.localScale.x, Quaternion.identity);
               Go.GetComponent<Rigidbody2D>().velocity = new Vector2(Velocity.x * transform.localScale.x, Velocity.y);
               canShoot = false;
            
            }
            if (XCI.GetAxis(XboxAxis.LeftStickX) <= -0.25 && canShoot)
            {
                GameObject Go = (GameObject)Instantiate(Spear, (Vector2)transform.position - offset * transform.localScale.x, Quaternion.identity);
                Go.GetComponent<Rigidbody2D>().velocity = new Vector2(Velocity.x * -transform.localScale.x, Velocity.y);
                canShoot = false;
            };
        }
        else if (XCI.GetButtonUp(XboxButton.RightBumper))

        {
            PlayerScript.canPlayerMove = true;
        }
         
    }
   


}
