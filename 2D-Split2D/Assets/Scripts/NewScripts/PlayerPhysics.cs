using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class PlayerPhysics : MonoBehaviour {

    public LayerMask CollisionMask;
    BoxCollider2D PlayerCollider;
 
    public float Skin = 0.015f;
    GameObject PlayerGameObj;
    NewPlayerController PlayerScript;
     public int HorizontalRayCount = 4;
    public int VerticalRayCount = 4;

    float HorizontalRaySpacing;
    float VerticalRaySpacing;
    public CollisionInfo Collisions;
      RaycastOrigins raycastOrigins;

    void Start()
    {
        PlayerCollider = GetComponent<BoxCollider2D>();
        PlayerGameObj = GameObject.FindGameObjectWithTag("Player");
        PlayerScript = PlayerGameObj.GetComponent<NewPlayerController>();
        CalculateRaySpacing();

    }

    public void Move(Vector3 velocity)
    {
        UpdateRaycastOrigins();
        Collisions.Reset();
        if (velocity.x != 0)
        {
            HorizontalCollisions(ref velocity);
        }
        if (velocity.y != 0)
        {
            VerticalCollisions(ref velocity);
        }

        transform.Translate(velocity);
    }

    void HorizontalCollisions(ref Vector3 velocity)
    {
        float directionX = Mathf.Sign(velocity.x);
        float rayLength = Mathf.Abs(velocity.x) + Skin;

        for (int i = 0; i < HorizontalRayCount; i++)
        {
            Vector2 rayOrigin = (directionX == -1) ? raycastOrigins.bottomLeft : raycastOrigins.bottomRight;
            rayOrigin += Vector2.up * (HorizontalRaySpacing * i);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, CollisionMask);

            Debug.DrawRay(rayOrigin, Vector2.right * directionX * rayLength, Color.red);

            if (hit)
            {
                velocity.x = (hit.distance - Skin) * directionX;
                rayLength = hit.distance;

                Collisions.Left = directionX == -1;
                Collisions.Right = directionX == 1;

            }
        }
    }

    void VerticalCollisions(ref Vector3 velocity)
    {
        float directionY = Mathf.Sign(velocity.y);
        float rayLength = Mathf.Abs(velocity.y) + Skin;

        for (int i = 0; i < VerticalRayCount; i++)
        {
            Vector2 rayOrigin = (directionY == -1) ? raycastOrigins.bottomLeft : raycastOrigins.topLeft;
            rayOrigin += Vector2.right * (VerticalRaySpacing * i + velocity.x);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLength, CollisionMask);

            Debug.DrawRay(rayOrigin, Vector2.up * directionY * rayLength, Color.red);

            if (hit)
            {
                Debug.Log("Grounded");
                PlayerScript.CanJump = true;
                velocity.y = (hit.distance - Skin) * directionY;
                rayLength = hit.distance;


                Collisions.Above = directionY == -1;
                Collisions.Below = directionY == 1;
            }
        }
    }

    void UpdateRaycastOrigins()
    {
        Bounds Bounds = PlayerCollider.bounds;
        Bounds.Expand(Skin * -2);

        raycastOrigins.bottomLeft = new Vector2(Bounds.min.x, Bounds.min.y);
        raycastOrigins.bottomRight = new Vector2(Bounds.max.x, Bounds.min.y);
        raycastOrigins.topLeft = new Vector2(Bounds.min.x, Bounds.max.y);
        raycastOrigins.topRight = new Vector2(Bounds.max.x, Bounds.max.y);
    }

    void CalculateRaySpacing()
    {
        Bounds Bounds = PlayerCollider.bounds;
        Bounds.Expand(Skin * -2);

        HorizontalRayCount = Mathf.Clamp(HorizontalRayCount, 2, int.MaxValue);
        VerticalRayCount = Mathf.Clamp(VerticalRayCount, 2, int.MaxValue);

        HorizontalRaySpacing = Bounds.size.y / (HorizontalRayCount - 1);
        VerticalRaySpacing = Bounds.size.x / (VerticalRayCount - 1);
    }

    struct RaycastOrigins
    {
        public Vector2 topLeft, topRight;
        public Vector2 bottomLeft, bottomRight;
    }

    public struct CollisionInfo
    {
        public bool Above, Below;
        public bool Left, Right;

        public void Reset()
        {
            Above = Below = false;
            Left = Right = false;
        }
    }

}



