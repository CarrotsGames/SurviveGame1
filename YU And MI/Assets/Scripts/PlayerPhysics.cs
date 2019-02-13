using UnityEngine;
using UnityEditor;
using XboxCtrlrInput;
public class PlayerPhysics : MonoBehaviour
{
    public LayerMask CollisionMask;
    BoxCollider2D PlayerCollider;

    float MaxClimbAngle = 80;
 
    public float Skin = 0.015f;
    GameObject PlayerGameObj;
    PlayerController PlayerScript;
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
        PlayerScript = PlayerGameObj.GetComponent<PlayerController>();
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

    void HorizontalCollisions(ref Vector3 Velocity)
    {
        float directionX = Mathf.Sign(Velocity.x);
        float rayLength = Mathf.Abs(Velocity.x) + Skin;

        for (int i = 0; i < HorizontalRayCount; i++)
        {
            Vector2 rayOrigin = (directionX == -1) ? raycastOrigins.bottomLeft : raycastOrigins.bottomRight;
            rayOrigin += Vector2.up * (HorizontalRaySpacing * i);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, CollisionMask);

            Debug.DrawRay(rayOrigin, Vector2.right * directionX * rayLength, Color.red);

            if (hit)
            {
                float SlopeAngle = Vector2.Angle(hit.normal, Vector2.up);
                if(i == 0 && SlopeAngle <= MaxClimbAngle)
                {
                    float DistanceToSlopeStart = 0;
                    if(SlopeAngle != Collisions.SlopeAngleOld)
                    {
                        DistanceToSlopeStart = hit.distance - Skin;
                        Velocity.x -= DistanceToSlopeStart * directionX;
                    }
                    ClimbSlope(ref Velocity, SlopeAngle);
                    Velocity.x += DistanceToSlopeStart * directionX;
                }
                // If the slope angle is equal to the max climb angle
                if(!Collisions.ClimbingSlope || SlopeAngle > MaxClimbAngle)
                {
                    Velocity.x = (hit.distance - Skin) * directionX;
                    rayLength = hit.distance;

                    // Stopcs climbing 
                    if(Collisions.ClimbingSlope)
                    {
                        Velocity.y = Mathf.Tan(Collisions.SlopeAngle * Mathf.Deg2Rad) * Mathf.Abs(Velocity.x);
                    }
                }
          
                Collisions.Left = directionX == -1;
                Collisions.Right = directionX == 1;

            }
        }
    }

    void VerticalCollisions(ref Vector3 Velocity)
    {
        float directionY = Mathf.Sign(Velocity.y);
        float rayLength = Mathf.Abs(Velocity.y) + Skin;

        for (int i = 0; i < VerticalRayCount; i++)
        {
            Vector2 rayOrigin = (directionY == -1) ? raycastOrigins.bottomLeft : raycastOrigins.topLeft;
            rayOrigin += Vector2.right * (VerticalRaySpacing * i + Velocity.x);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLength, CollisionMask);

            Debug.DrawRay(rayOrigin, Vector2.up * directionY * rayLength, Color.red);

            if (hit)
            {
                Debug.Log("Grounded");
                PlayerScript.CanJump = true;
                Velocity.y = (hit.distance - Skin) * directionY;
                rayLength = hit.distance;
                if(Collisions.ClimbingSlope)
                {
                    Velocity.x = Velocity.y / Mathf.Tan(Collisions.SlopeAngle * Mathf.Deg2Rad) * Mathf.Sign(Velocity.x);
                }

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
    void ClimbSlope(ref Vector3 velocity, float SlopeAngle)
    {
        float moveDistance = Mathf.Abs(velocity.x);
        float climbVelocityY = Mathf.Sin(SlopeAngle * Mathf.Deg2Rad) * moveDistance;

        if (velocity.y <= climbVelocityY)
        {
            velocity.y = climbVelocityY;
            velocity.x = Mathf.Cos(SlopeAngle * Mathf.Deg2Rad) * moveDistance * Mathf.Sign(velocity.x);
            Collisions.Below = true;
            Collisions.ClimbingSlope = true;
            Collisions.SlopeAngle = SlopeAngle;
        }
    }

    public struct CollisionInfo
    {
        public bool Above, Below;
        public bool Left, Right;

        public bool ClimbingSlope;
        public float SlopeAngle, SlopeAngleOld;

        public void Reset()
        {
            Above = Below = false;
            Left = Right = false;
            ClimbingSlope = false;
            SlopeAngleOld = SlopeAngle;
            SlopeAngle = 0;
        }
    }

}

