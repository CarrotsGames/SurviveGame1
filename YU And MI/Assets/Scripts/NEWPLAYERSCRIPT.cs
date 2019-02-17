using UnityEngine;
using System.Collections;
using XboxCtrlrInput;


[RequireComponent(typeof(NEWPlayterPhsyics))]
public class NEWPLAYERSCRIPT : MonoBehaviour
{

    public float jumpHeight = 4;
    public float timeToJumpApex = .4f;
    float accelerationTimeAirborne = .2f;
    float accelerationTimeGrounded = .1f;
    float moveSpeed = 6;

    public float TimeTillGlide;
    float GlideTimer;
    public Vector2 WallJump;
    bool CanGlide;

    float Apex;

    float gravity;
    float jumpVelocity;
    Vector3 velocity;
    float velocityXSmoothing;

    NEWPlayterPhsyics controller;

    void Start()
    {
        controller = GetComponent<NEWPlayterPhsyics>();
        CanGlide = false;
        Apex = timeToJumpApex;
        GlideTimer = TimeTillGlide;
        gravity = -(2 * jumpHeight) / Mathf.Pow(timeToJumpApex, 2);
        jumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
        print("Gravity: " + gravity + "  Jump Velocity: " + jumpVelocity);
    }

    void Update()
    {
        Vector2 input = new Vector2(XCI.GetAxis(XboxAxis.LeftStickX), (XCI.GetAxis(XboxAxis.LeftStickY)));
        int wallDirX = (controller.collisions.left) ? -1 : 1;

        float targetVelocityX = input.x * moveSpeed;
        velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, (controller.collisions.below) ? accelerationTimeGrounded : accelerationTimeAirborne);

        if ((controller.collisions.left) && !controller.collisions.below && velocity.y < 0)
        {
            if (XCI.GetButtonDown(XboxButton.A))
            {
                velocity.y = 0;
                velocity.x = WallJump.x;
                velocity.y = WallJump.y;
            }

        }
        if ((controller.collisions.right) && !controller.collisions.below && velocity.y < 0)
        {
            if (XCI.GetButtonDown(XboxButton.A))
            {
                velocity.y = 0;
                velocity.x = -WallJump.x;
                velocity.y = WallJump.y;
            }

        }

        if (controller.collisions.above || controller.collisions.below)
        {
            velocity.y = 0;

        }



        if (XCI.GetButtonDown(XboxButton.A))
        {

            if (controller.collisions.below)
            {
                velocity.y = jumpVelocity;
            }
        }

        if (tag == "Player2")
        {
            if (CanGlide)
            {
                TimeTillGlide -= Time.deltaTime;

                if (TimeTillGlide <= 0)
                {
                    timeToJumpApex = 1;
                    gravity = -(2 * jumpHeight) / Mathf.Pow(timeToJumpApex, 2);
                }

                if (TimeTillGlide <= -1)
                {
                    timeToJumpApex = 1;
                    gravity = -(2 * jumpHeight) / Mathf.Pow(timeToJumpApex, 2);
                    velocity.y = -2.45f;

                }
            }
            if (XCI.GetButtonDown(XboxButton.A))
            {
                CanGlide = true;

            }
            if (XCI.GetButtonUp(XboxButton.A))
            {
                TimeTillGlide = GlideTimer;
                timeToJumpApex = Apex;
                gravity = -(2 * jumpHeight) / Mathf.Pow(timeToJumpApex, 2);
                CanGlide = false;


            }



        }
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}