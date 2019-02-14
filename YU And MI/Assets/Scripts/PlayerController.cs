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
    public bool GLIDING = false;

    // public Vector2 WallJumpClimb;
    // public Vector2 WallJumpOff;
    // public Vector2 WallLeap;
    public Vector2 WallJump;

    public float WallSlideSpeedMax;


    public float Gravity;
    public float JumpVelocity;
    public bool CanJump;
    bool CanGlide;
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

        //  bool IsWallSliding = false;

        //IsWallSliding = true;
        // if(Velocity.y < -WallSlideSpeedMax)
        //{
        //    Velocity.y = -WallSlideSpeedMax;
        //}

        if (playerphys.Collisions.Above || playerphys.Collisions.Below)
        {

            Velocity.y = 0;
        }
        if ((playerphys.Collisions.Left) && !playerphys.Collisions.Below && Velocity.y < 0)
        {
            Debug.Log("LeftWall");
            if (XCI.GetButtonDown(XboxButton.A))
            {
                Velocity.y = 0;
                Velocity.x = WallJump.x;
                Velocity.y = WallJump.y;
            }

        }
        if (playerphys.Collisions.Right && !playerphys.Collisions.Below && Velocity.y < 0)
        {
            Debug.Log("RightWall");

            if (XCI.GetButtonDown(XboxButton.A))
            {
                Velocity.y = 0;

                Velocity.x = -WallJump.x;
                Velocity.y = WallJump.y;
            }
        }
        if (XCI.GetButton(XboxButton.A))
        {
      
            if (playerphys.Collisions.Above)
            {
                Velocity.y = JumpVelocity;
            }
        }
        if (tag == "Player2")
        {
            if (XCI.GetButtonDown(XboxButton.A) && !CanGlide)
            {
                CanGlide = true;
            }
            if(CanGlide && XCI.GetButtonDown(XboxButton.A))
            {

                if(XCI.GetButtonUp(XboxButton.A))
                GLIDING = true;
            }
            if (XCI.GetButtonDown(XboxButton.A) && GLIDING)
            {
                Debug.Log("Glide");
            }
        }
         
        if (tag == "Player")
        {
            CanGlide = false;
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
