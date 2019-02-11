using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformController : RaycastController {
    public LayerMask PlayerMask;
 public Vector3 move;
	// Use this for initialization
	public override void  Start () {
        base.Start();
	}
	
	// Update is called once per frame
	void Update ()
    {
        UpdateRaycastOrigins();
        Vector3 velocity = move * Time.deltaTime;
        MovePassengers(velocity);
        transform.Translate(velocity);
	}
    void MovePassengers(Vector3 Velocity)
    {

        HashSet<Transform> movedPassengers = new HashSet<Transform>();
        float directionX = Mathf.Sign(Velocity.x);
        float directionY = Mathf.Sign(Velocity.y);

        // Vertical platform
        if (Velocity.y != 0)
        {
            float rayLength = Mathf.Abs(Velocity.y) + skinWidth;
            // Cosntantly checks raycast on Y axis 
            for (int i = 0; i < verticalRayCount; i++)
            {
                Vector2 rayOrigin = (directionY == -1) ? raycastOrigins.bottomLeft : raycastOrigins.topLeft;
                rayOrigin += Vector2.right * (verticalRaySpacing * i);
                RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLength, PlayerMask);

                if (hit)
                {
                    if (!movedPassengers.Contains(hit.transform))
                    {
                        movedPassengers.Add(hit.transform);
                        float forceX = (directionY == 1) ? Velocity.x : 0;
                        float forceY = Velocity.y - (hit.distance - skinWidth) * directionY;
                        hit.transform.Translate(new Vector3(forceX, forceY));
                    }
                }
            }
        }

        if (Velocity.x != 0)
        {
            float rayLength = Mathf.Abs(Velocity.x) + skinWidth;

            for (int i = 0; i < horizonalRayCount; i++)
            {
                Vector2 rayOrigin = (directionX == -1) ? raycastOrigins.bottomLeft : raycastOrigins.bottomRight;
                rayOrigin += Vector2.up * (horizontalRaySpacing * i);
                RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, PlayerMask);
                if (hit)
                {
                    if (!movedPassengers.Contains(hit.transform))
                    {
                        movedPassengers.Add(hit.transform);
                        float forceX = Velocity.x - (hit.distance - skinWidth) * directionX;
                        float forceY = 0;
                        hit.transform.Translate(new Vector3(forceX, forceY));
                    }
                }
            }
        }
    }
}
