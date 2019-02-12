using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XboxCtrlrInput;
public class Player : MonoBehaviour {

    public float timeToJumpApex = .4f;                                      
    // how long the player can attack for
    public float swordTime = 1.0f;                                           
    //gravity of the player 
    float gravity;
    // how high the player can jump
    public float jumpHeight = 4;                                             
    // how fast the player can jump
    float jumpVelocity;    
    // How long the player can be airborn
    float accelerationTimeAirborne = .2f;        
    // how long until the player can touch the ground
    float accelerationTimeGrounded = .1f;                                    
    //movement speed
    public float moveSpeed;													
    public float wallSlideSpeedCapacity = 3;
    public float wallStickTime = 0.25f;
    float wallUnstickTime; 


    //refrences the player Gameobject 
    GameObject PlayerGameobj;
    GameObject PlayerGameobj2;

    // grabs the start point of the level
    GameObject levelStart;
    // grabs the end point of the level
    public GameObject LevelEnd;
    //creates gameobject slot
    public GameObject sword;       
    //creates gameobject slot
    public GameObject sword2;

    public Vector3 StartOffset;
    // Allows player to jump off and climb the same wall 
    public Vector2 wallJumpClimb;
    // allows player to let go of the wall at any time 
    //public Vector2 wallJumpOff;
    // Gives player the momentum of a wall jump to reach other wall
    public Vector2 wallJumpAction;
    // the velocity of the players x,y,z
    Vector3 velocity;
    public bool canJump;
    // prevents the players movement from being jaggedy 
    float velocityXSmoothing;
    //creates Orbitangle
    private float OrbitAngle;                                              
    // references players rigidbody
    private Rigidbody2D rb;                                                 
    // resets players position in the world 
    public Vector3 ResetPosition;    
    // Refrences the raycast controller 
    private RaycastController player;
    public bool canPlayerMove;
    //refrences the Controller2D script
    Controller2D controller;                                                
                                                                       
    void Start() {
        rb = GetComponent<Rigidbody2D>();
        controller = GetComponent<Controller2D>();
        player = GetComponent<RaycastController>();
        PlayerGameobj = GameObject.FindGameObjectWithTag("Player");
        PlayerGameobj2 = GameObject.FindGameObjectWithTag("Player2");
        canPlayerMove = true;
        levelStart = GameObject.FindGameObjectWithTag("LevelStart");

        ResetPosition = levelStart.transform.position + PlayerGameobj.GetComponent<Controller2D>().StartOffset;
        PlayerGameobj.transform.position = levelStart.transform.position + StartOffset;
        ResetPosition = levelStart.transform.position + PlayerGameobj2.GetComponent<Controller2D>().StartOffset;
        PlayerGameobj2.transform.position = levelStart.transform.position + StartOffset;
        gravity = -(2 * jumpHeight) / Mathf.Pow(timeToJumpApex, 2);
        jumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
        canJump = true;
        print("Gravity: " + gravity + " Jump Velocity: " + jumpVelocity);
    }
    //------------------------------------------------------------------------------------------------------------------
    //		Update()
    // Runs every frame
    //
    // Parma:
    //				None
    // Return:
    //				Void
    //------------------------------------------------------------------------------------------------------------------
    private void Update()
    {
        if (canPlayerMove)
        {
            Movement();
        }
    }
    void Movement()
    {
        Vector2 input = new Vector2(XCI.GetAxis(XboxAxis.LeftStickX), XCI.GetAxis(XboxAxis.LeftStickY));
        int wallDirectionX = (controller.collisions.left) ? -1 : 1;

        float targetVelocityX = input.x * moveSpeed;
        velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, (controller.collisions.below) ? accelerationTimeGrounded : accelerationTimeAirborne);

        bool isWallSliding = false;
        // if the player is colliding with the wall in the air
        if ((controller.collisions.left || controller.collisions.right) && !controller.collisions.below && velocity.y <= 0)
        {
            isWallSliding = true;
            // Makes player slide down the wall 
            if (velocity.y < -wallSlideSpeedCapacity)
            {
                velocity.y = -wallSlideSpeedCapacity;
            }
            // Counts down how long the player can slide down the wall 
            if (wallUnstickTime > 0)
            {
                velocityXSmoothing = 0;
                velocity.x = 0;
                if (input.x != wallDirectionX && input.x != 0 && controller.maxClimbAngle == 90) 
                {
                    wallUnstickTime -= Time.deltaTime;
                    Debug.Log("WallStick");
                }
                // Resets unstick time
                else
                {
                    wallUnstickTime = wallStickTime;
                }
            }
            // Resets unstick time
            else
            {
                wallUnstickTime = wallStickTime;
            }

        }

            if (controller.collisions.above || controller.collisions.below)
            {
                velocity.y = 0;
            }

            if (XCI.GetButtonDown(XboxButton.A) && canJump)
            {
                if (isWallSliding)
                {
                    if (wallDirectionX == input.x  )
                    {
                        velocity.x = -wallDirectionX * wallJumpClimb.x;
                        velocity.y = wallJumpClimb.y;

                    }
                    else if (input.x == 0)
                    {
                    //  velocity.x = -wallDirectionX * wallJumpOff.x;
                    //     velocity.y = wallJumpOff.y;
                    Debug.Log("AH");
                    velocity.x = -wallDirectionX * wallJumpAction.x;
                    velocity.y = wallJumpAction.y;
                }
                    else
                    {
                        velocity.x = -wallDirectionX * wallJumpAction.x;
                        velocity.y = wallJumpAction.y;
                    }
                }
                if (controller.collisions.below)
                {
                    velocity.y = jumpVelocity;
                }
            }

            if (XCI.GetButtonDown(XboxButton.LeftBumper))
            {
                sword.SetActive(true);
                Invoke("hideSword", swordTime);
            }

            //if (XCI.GetButtonDown(XboxButton.RightBumper))
            //{
            //    sword2.SetActive(true);
            //    Invoke("hideSword", swordTime);
            //}

            velocity.y += gravity * Time.deltaTime;
            controller.Move(velocity * Time.deltaTime);
        }
     
    
	//------------------------------------------------------------------------------------------------------------------
	//		ResetPlayer()
	// Reset the player position
	//
	// Parma:
	//				None
	// Return:
	//				Void
	//------------------------------------------------------------------------------------------------------------------

	public void ResetPlayer () {

		//.rb.angularVelocity = Vector2.zero;
		rb.velocity = Vector2.zero;
		transform.position = ResetPosition;
		OrbitAngle = 0.01f;
	}
    //------------------------------------------------------------------------------------------------------------------
    //		hideSword()
    // Turns off the player sword after use
    //
    // Parma:
    //				None
    // Return:
    //				Void
    //------------------------------------------------------------------------------------------------------------------
 
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player2")
        {
            Debug.Log("Player");
        }
       
    }

    private void hideSword (){
		sword.SetActive (false);
		sword2.SetActive (false);
	}
}
