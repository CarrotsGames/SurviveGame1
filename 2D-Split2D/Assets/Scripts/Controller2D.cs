using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
[RequireComponent (typeof ( BoxCollider2D))]

public class Controller2D : RaycastController {
 	public LayerMask collisionMask;

	//Climbing angle
	public float maxClimbAngle = 60;
    public float maxDescendAngle = 75;
    Player PlayerScript;
    Player PlayerScript2;

    GameObject PlayerGameobj;
    GameObject PlayerGameobj2;

    public CollisionInfo collisions;
    public override void Start()
    {
        base.Start();
        PlayerGameobj = GameObject.FindGameObjectWithTag("Player");
        PlayerGameobj2 = GameObject.FindGameObjectWithTag("Player2");

        PlayerScript = PlayerGameobj.GetComponent<Player>();
        PlayerScript2 = PlayerGameobj.GetComponent<Player>();

        collisions.faceDirection = 1;
    }

    //grabs "UpdateRayCastOrigins" for the raycast bounds.
    //grabs "Collision.Reset" for the left,right,above,below,slope resets.
    //if velocity along the x position = 0 then grab "HorizontalCollision" ref will pass the same varibles onto the method.
    //if velocity along the y position = 0 then grab "Vertical Collisions" ref will pass the same verivles onto the method. 
    //------------------------------------------------------------------------------------------------------------------
    //		Move()
    // Moves the player around 
    //
    // Param:
    //				Vector3 velocity
    // Return:
    //				Void
    //------------------------------------------------------------------------------------------------------------------
    public void Move(Vector3 velocity)
    {
		UpdateRaycastOrigins ();
		collisions.Reset ();
        collisions.velocityOld = velocity;
        if(velocity.x != 0)
        {
            collisions.faceDirection = (int)Mathf.Sign(velocity.x);
        }
        if(velocity.y < 0)
        {
            DescenedSlope(ref velocity);
        }

        HorizontalCollisions(ref velocity);

        if (velocity.y != 0)
        {
			VerticalCollisions (ref velocity);
		}
		transform.Translate (velocity);
			
	}
	//------------------------------------------------------------------------------------------------------------------
	//		HorizontalCollisions()
	// creates raycasting collisions detection horizontally 
	//
	// Param:
	//				ref Vector3 velocity
	// Return:
	//				Void
	//------------------------------------------------------------------------------------------------------------------
	void HorizontalCollisions (ref Vector3 velocity) {
        float directionX = collisions.faceDirection;
		float rayLength = Mathf.Abs (velocity.x) + skinWidth;

        if (Mathf.Abs(velocity.x) < skinWidth)
        {
            rayLength = 2 * skinWidth;

        }

		for (int i = 0; i < horizonalRayCount; i ++) {
            Vector2 rayOrigin = (directionX == -1) ? raycastOrigins.bottomLeft : raycastOrigins.bottomRight;
			rayOrigin += Vector2.up * (horizontalRaySpacing * i);
			RaycastHit2D hit = Physics2D.Raycast (rayOrigin, Vector2.right * directionX, rayLength, collisionMask);

			Debug.DrawRay(rayOrigin, Vector2.right * directionX * rayLength,Color.red);

			if (hit) {

                float slopeAngle = Vector2.Angle (hit.normal, Vector2.up);

 				if (i == 0 && slopeAngle <= maxClimbAngle )
                {
                     if (collisions.descendingSlope)
                    {
                        collisions.descendingSlope = false;
                        velocity = collisions.velocityOld;
                    }

                    float distanceToSlopeStart = 0;
					if (slopeAngle != collisions.slopeAngleOld)
                    {
                        distanceToSlopeStart = hit.distance - skinWidth;
                        velocity.x -= distanceToSlopeStart * directionX;
                    }
					ClimbSlope (ref velocity, slopeAngle);
					velocity.x += distanceToSlopeStart * directionX;
 
                }

                if (!collisions.climbingSlope || slopeAngle > maxClimbAngle) {
					velocity.x = (hit.distance - skinWidth) * directionX;
					rayLength = hit.distance;
                     if (collisions.climbingSlope)
                    {
						velocity.y = Mathf.Tan (collisions.slopeAngle * Mathf.Deg2Rad) * Mathf.Abs (velocity.x);

					}
                    if(slopeAngle < 90)
                    {
                        if (tag == "Player")
                        {
                            PlayerScript.canJump = false;
                        }
                        else if(tag == "Player2")
                        {
                            PlayerScript2.canJump = false;

                        }
                    }

                    collisions.left = directionX == -1;
                    collisions.right = directionX == 1;
                }


            }
        }
	}
	//------------------------------------------------------------------------------------------------------------------
	//		VerticalCollisions()
	// creates raycasting collisions detection Vertically 
	//
	// Param:
	//				ref Vector3 velocity
	// Return:
	//				Void
	//------------------------------------------------------------------------------------------------------------------
	void VerticalCollisions (ref Vector3 velocity)
    {
		float directionY = Mathf.Sign (velocity.y);
		float rayLength = Mathf.Abs (velocity.y) + skinWidth;
        // Cosntantly checks raycast on Y axis 
		for (int i = 0; i < verticalRayCount; i ++)
        {
			Vector2 rayOrigin = (directionY == -1)?raycastOrigins.bottomLeft : raycastOrigins.topLeft;
			rayOrigin += Vector2.right * (verticalRaySpacing * i + velocity.x);
			RaycastHit2D hit = Physics2D.Raycast (rayOrigin, Vector2.up * directionY, rayLength, collisionMask);

			Debug.DrawRay(rayOrigin, Vector2.up * directionY * rayLength,Color.red);
            // CHECKS IF PLAYER IS GROUNDED/Colliding with anything on its lower hitbox
            // IF THE PLAYER IS WALKING ON FLAT GROUND
			if (hit)
            {
                velocity.y = (hit.distance - skinWidth) * directionY;
				rayLength = hit.distance;
                PlayerScript.canJump = true;
 
                // checks if theres a slope
                if (collisions.climbingSlope)
                {
                    velocity.x = velocity.y / Mathf.Tan(collisions.slopeAngle * Mathf.Deg2Rad) * Mathf.Sign(velocity.x);
                }

                collisions.below = directionY == -1;
				collisions.above = directionY == 1;
			}
	    }
        // IF THE PLAYER IS WALKING UP A SLOPE
        if(collisions.climbingSlope)
        {
            float DirectionX = Mathf.Sign(velocity.x);
            rayLength = Mathf.Abs(velocity.x) + skinWidth;
            Vector2 RayOrigin = ((DirectionX == -1)?raycastOrigins.bottomLeft:raycastOrigins.bottomRight) + Vector2.up * velocity.y;
            RaycastHit2D hit = Physics2D.Raycast(RayOrigin, Vector2.right * DirectionX, rayLength, collisionMask);
            if(hit)
            {
 
                float SlopeAngle = Vector2.Angle(hit.normal, Vector2.up);
                if(SlopeAngle != collisions.slopeAngle)
                {
                    velocity.x = (hit.distance - skinWidth) * DirectionX;
                    collisions.slopeAngle = SlopeAngle;
                }
            }
        }
	}
	//------------------------------------------------------------------------------------------------------------------
	//		ClimbSlope()
	// raycasts up to 80 Degree slopes so it is smooth 
	//
	// Param:
	//				ref Vector3 velocity and float slopeAngle
	// Return:
	//				Void
	//------------------------------------------------------------------------------------------------------------------
	void ClimbSlope(ref Vector3 velocity, float slopeAngle)
    {
		float moveDistance = Mathf.Abs (velocity.x);
		float climbVelocityY = Mathf.Sin (slopeAngle * Mathf.Deg2Rad) * moveDistance;

		if (velocity.y <= climbVelocityY)
        {
			velocity.y = climbVelocityY;
			velocity.x = Mathf.Cos (slopeAngle * Mathf.Deg2Rad) * moveDistance * Mathf.Sign (velocity.x);
			collisions.below = true;
			collisions.climbingSlope = true;
			collisions.slopeAngle = slopeAngle;
		}
	}

    void DescenedSlope(ref Vector3 velocity )
    {
        float directionX = Mathf.Sign(velocity.x);
        // Calculates players bottom right and left coordinates in the world 
        Vector2 rayOrigin = (directionX == -1) ? raycastOrigins.bottomRight : raycastOrigins.bottomLeft;
        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, -Vector2.up, Mathf.Infinity, collisionMask);
        
        if(hit)
        {
            float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);
            // if players collides with slope
            if(slopeAngle != 0 && slopeAngle <= maxDescendAngle)
            {
                 if(Mathf.Sign(hit.normal.x) == directionX)
                {
                    if(hit.distance - skinWidth <= Mathf.Tan(slopeAngle * Mathf.Deg2Rad) * Mathf.Abs(velocity.x))
                    {
                        float moveDistance = Mathf.Abs(velocity.x);
                        float descendVelocityY = Mathf.Sin(slopeAngle * Mathf.Deg2Rad) * moveDistance;
                        velocity.x = Mathf.Cos(slopeAngle * Mathf.Deg2Rad) * moveDistance * Mathf.Sign(velocity.x);
                        velocity.y -= descendVelocityY;
                        collisions.slopeAngle = slopeAngle;
                        collisions.descendingSlope = true;
                        collisions.below = true;
                    }
                }
            }
        }
    }
 
	//------------------------------------------------------------------------------------------------------------------
	//		CollisionInfo()
	// calculates Collision info
	//
	// Param:
	//				None
	// Return:
	//				struct
	//------------------------------------------------------------------------------------------------------------------
	
	//------------------------------------------------------------------------------------------------------------------
	//		NextScene()
	// Loads the next scene
	//
	// Param:
	//				ref Vector3 velocity (whaaa?)
	// Return:
	//				Void
	//------------------------------------------------------------------------------------------------------------------
	public void NextScene()
    {
        if (SceneManager.GetActiveScene().buildIndex == SceneManager.sceneCount)
        {
            SceneManager.LoadScene(0);
        }
        else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
	}

}
