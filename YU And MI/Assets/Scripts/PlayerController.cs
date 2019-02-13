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

    public float Gravity;
    public float JumpVelocity;
    public bool CanJump;
    Vector3 Veloctiy;
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
      
       if (XCI.GetButtonDown(XboxButton.A) && CanJump)
       {
           Veloctiy.y = JumpVelocity;
           CanJump = false;
           Debug.Log("Jump");
       }
        float TargetVelocity = Input.x * MoveSpeed;
        Veloctiy.x = Mathf.SmoothDamp(Veloctiy.x, TargetVelocity, ref TargetVelocity, (playerphys.Collisions.Below) ? AccelerationTimeGrounded : AccelerationTimeAirBourne);

        Veloctiy.y += Gravity * Time.deltaTime;
        playerphys.Move(Veloctiy * Time.deltaTime);
        Debug.Log(Veloctiy.y);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {

        if (other.collider.gameObject.layer == 8)
        {
            Debug.Log("Hithead");
        }
    }
}
