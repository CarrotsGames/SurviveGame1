using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XboxCtrlrInput;
[RequireComponent(typeof(PlayerPhysics))]

public class PlayerController : MonoBehaviour {

    public float JumpHeight = 4;
    public float TimeToApex = .4f;
    public float AccelerationTimeAirBourne = .2f;
    public float AccelerationTimeGrounded = .1f;
    public float MoveSpeed = 6;

    public Vector2 WallJumpClimb;
    public Vector2 WallJumpOff;
    public Vector2 WallLeap;

    public float WallSlideSpeedMax;


    public float Gravity;
    public float JumpVelocity;
    public bool CanJump;
    Vector3 Velocity;
    PlayerPhysics playerphys;
    private void Start()
    {
        playerphys = GetComponent<PlayerPhysics>();
        Gravity = -(2 * JumpHeight) / Mathf.Pow(TimeToApex, 2);
        JumpVelocity = Mathf.Abs(Gravity) * TimeToApex;
        CanJump = true;
    }

    void Update()
    {
       Vector2 Input = new Vector2(XCI.GetAxis(XboxAxis.LeftStickX), (XCI.GetAxis(XboxAxis.LeftStickY)));
        int WallDirectionX = (playerphys.Collisions.Left) ? -1 : 1;
        float TargetVelocity = Input.x * MoveSpeed;
        Velocity.x = Mathf.SmoothDamp(Velocity.x, TargetVelocity, ref TargetVelocity, (playerphys.Collisions.Below) ? AccelerationTimeGrounded : AccelerationTimeAirBourne);

        bool IsWallSliding = false;
        if((playerphys.Collisions.Left || playerphys.Collisions.Right) && !playerphys.Collisions.Below && Velocity.y < 0)
        {
            IsWallSliding = true;
             if(Velocity.y < -WallSlideSpeedMax)
            {
                Velocity.y = -WallSlideSpeedMax;
            }
        }
        if (playerphys.Collisions.Above || playerphys.Collisions.Below)
        {
            Debug.Log("FloorCollision");

            Velocity.y = 0;
        }
        if (XCI.GetButtonDown(XboxButton.A))
        {
            if (IsWallSliding)
            {
                if(WallDirectionX == Input.x)
                {
                    Velocity.x = -WallDirectionX * WallJumpClimb.x;
                    Velocity.y = WallJumpClimb.y;
                }
                else if(Input.x == 0)
                {
                    Velocity.x = -WallDirectionX * WallJumpOff.x;
                    Velocity.y = WallJumpOff.y;
                }
                else
                {
                    Velocity.x = -WallDirectionX * WallLeap.x;
                    Velocity.y = WallLeap.y;
                }
            }
            if (playerphys.Collisions.Above)
            {
                Velocity.y = JumpVelocity;
            }
        }
      

        Velocity.y += Gravity * Time.deltaTime;
        playerphys.Move(Velocity * Time.deltaTime);
     }

    private void OnCollisionEnter2D(Collision2D other)
    {

        if (other.collider.gameObject.layer == 8)
        {
            Debug.Log("Hithead");
        }
    }
}
