using UnityEngine;
using UnityEditor;
using XboxCtrlrInput;
public class PlayerPhysics : MonoBehaviour
{
    public LayerMask CollisionMask;
    BoxCollider2D PlayerCollider;

    public float MaxClimbAngle = 80;
    public float MaxDescendAngle = 80;

    public float Skin = 0.015f;
    GameObject PlayerGameObj;
    GameObject PlayerGameObj2;

    PlayerController PlayerScript;
    PlayerController PlayerScript2;

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
        PlayerGameObj2 = GameObject.FindGameObjectWithTag("Player2");
        PlayerScript2 = PlayerGameObj.GetComponent<PlayerController>();
        CalculateRaySpacing();
        Collisions.FaceDir = 1; 
    }

    public void Move(Vector3 velocity)
    {
        UpdateRaycastOrigins();
        Collisions.Reset();
        Collisions.VelocityOld = velocity;
 
        if (velocity.x != 0)
        {
            Collisions.FaceDir = (int)Mathf.Sign(velocity.x);
        }

        if (velocity.y < 0)
        {
            DescenedSlope(ref velocity);
        }

        HorizontalCollisions(ref velocity);
        if (velocity.y != 0)
        {
            VerticalCollisions(ref velocity);
        }

        transform.Translate(velocity);

      
    }

    void HorizontalCollisions(ref Vector3 velocity)
    {
        float directionX = Collisions.FaceDir;
        float rayLength = Mathf.Abs(velocity.x) + Skin;

        if (Mathf.Abs(velocity.x) < Skin)
        {
            rayLength = 2 * Skin;
        }

        for (int i = 0; i < HorizontalRayCount; i++)
        {
            Vector2 rayOrigin = (directionX == -1) ? raycastOrigins.bottomLeft : raycastOrigins.bottomRight;
            rayOrigin += Vector2.up * (HorizontalRayCount * i);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, CollisionMask);

            Debug.DrawRay(rayOrigin, Vector2.right * directionX * rayLength, Color.red);

            if (hit)
            {

                if (hit.distance == 0)
                {
                    continue;
                }

                float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);

                if (i == 0 && slopeAngle <= MaxClimbAngle)
                {
                    if (Collisions.DescendSlope)
                    {
                        Collisions.DescendSlope = false;
                        velocity = Collisions.VelocityOld;
                    }
                    float distanceToSlopeStart = 0;
                    if (slopeAngle != Collisions.SlopeAngleOld)
                    {
                        distanceToSlopeStart = hit.distance - Skin;
                        velocity.x -= distanceToSlopeStart * directionX;
                    }
                    ClimbSlope(ref velocity, slopeAngle);
                    velocity.x += distanceToSlopeStart * directionX;
                }

                if (!Collisions.ClimbingSlope || slopeAngle > MaxClimbAngle)
                {
                    velocity.x = (hit.distance - Skin) * directionX;
                    rayLength = hit.distance;

                    if (Collisions.ClimbingSlope)
                    {
                        velocity.y = Mathf.Tan(Collisions.SlopeAngle * Mathf.Deg2Rad) * Mathf.Abs(velocity.x);
                    }

                    Collisions.Left = directionX == -1;
                    Collisions.Right = directionX == 1;
                }
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
              //  Debug.Log("Grounded");
                PlayerScript.CanJump = true;
                Velocity.y = (hit.distance - Skin) * directionY;
                rayLength = hit.distance;
                if(Collisions.ClimbingSlope)
                {
                    Velocity.x = Velocity.y / Mathf.Tan(Collisions.SlopeAngle * Mathf.Deg2Rad) * Mathf.Sign(Velocity.x);
                }
              // if (Collisions.SlopeAngle > 60 && Collisions.SlopeAngle < 180 || Collisions.SlopeAngle < 0 && Collisions.SlopeAngle < -180)
              // {
              //     if (tag == "Player")
              //     {
              //         PlayerScript.CanJump = false;
              //     }
              //     else if(tag == "Player2")
              //     {
              //         PlayerScript2.CanJump = false;
              //
              //     }
              // }
              // else
              // {
              //     PlayerScript.CanJump = true;
              //
              // }
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

    void DescenedSlope(ref Vector3 Velocity)
    {
        float DirectionX = Mathf.Sign(Velocity.x);
                            // If directionX        Do this                 else do this
        Vector2 RayOrigin = (DirectionX == -1) ? raycastOrigins.bottomRight : raycastOrigins.bottomLeft;
        RaycastHit2D Hit = Physics2D.Raycast(RayOrigin, -Vector2.up, Mathf.Infinity, CollisionMask);

        if(Hit)
        {
            float SlopeAngle = Vector2.Angle(Hit.normal, Vector2.up);
            if(SlopeAngle != 0 && SlopeAngle <= MaxDescendAngle)
            {
                if(Mathf.Sign(Hit.normal.x) == DirectionX)
                {
                    if(Hit.distance - Skin <= Mathf.Tan(SlopeAngle * Mathf.Deg2Rad) * Mathf.Abs(Velocity.x))
                    {
                        float MoveDistance = Mathf.Abs(Velocity.x);
                        float DescenedVelocityY = Mathf.Sin(SlopeAngle * Mathf.Deg2Rad) * MoveDistance;

                        Velocity.x = Mathf.Cos(SlopeAngle * Mathf.Deg2Rad) * MoveDistance * Mathf.Sign(Velocity.x);
                        Velocity.y -= DescenedVelocityY;

                        Collisions.SlopeAngle = SlopeAngle;
                        Collisions.DescendSlope = true;
                        Collisions.Below = true;
                    }
                }
            }
        }
    }
    public struct CollisionInfo
    {
        public bool Above, Below;
        public bool Left, Right;

        public bool ClimbingSlope;
        public bool DescendSlope;

        public float SlopeAngle, SlopeAngleOld;
        public Vector3 VelocityOld;
        public int FaceDir;
        public void Reset()
        {
            Above = Below = false;
            Left = Right = false;
            ClimbingSlope = false;
            DescendSlope = false;
            SlopeAngleOld = SlopeAngle;
            SlopeAngle = 0;
        }
    }

}

