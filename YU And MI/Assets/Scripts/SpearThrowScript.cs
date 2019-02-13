using UnityEngine;
using System.Collections;
using XboxCtrlrInput;
public class SpearThrowScript : MonoBehaviour
{

    PlayerController PlayerScript;

    GameObject PlayerGameObj;

    public GameObject Spear;
    public Vector2 Velocity;
    bool canShoot = true;
    public float coolDown = 1;
    public Vector2 offset = new Vector2(0.4f, 0);
    // Use this for initialization
    void Start()
    {
        PlayerGameObj = GameObject.FindGameObjectWithTag("Player");
        PlayerScript = PlayerGameObj.GetComponent<PlayerController>();


    }

    // Update is called once per frame
    void Update()
    {
        if (!canShoot)
        {
            coolDown -= Time.deltaTime;

            if (coolDown <= 0)
            {
                canShoot = true;
                coolDown = 1;
            }
        }
        if (XCI.GetAxis(XboxAxis.LeftStickX) >= 0.25f)
        {
            Vector3 TargetAngle = transform.eulerAngles = 0 * Vector3.up;
            transform.eulerAngles = Vector3.Lerp(transform.eulerAngles, TargetAngle, 3);

        }
        if (XCI.GetAxis(XboxAxis.LeftStickX) <= -0.25f)
        {

            Vector3 TargetAngle = transform.eulerAngles = 180f * Vector3.up;
            transform.eulerAngles = Vector3.Lerp(transform.eulerAngles, TargetAngle, 3);

        }
        if (XCI.GetButton(XboxButton.RightBumper) && canShoot)
        {

            if (transform.rotation.y <= 0)
            {
                GameObject Go = (GameObject)Instantiate(Spear, (Vector2)transform.position + offset * transform.localScale.x, Quaternion.identity);
                Go.GetComponent<Rigidbody2D>().velocity = new Vector2(Velocity.x * transform.localScale.x, Velocity.y);
                canShoot = false;
            }

            else if (transform.rotation.y >= 0)
            {
                GameObject Go = (GameObject)Instantiate(Spear, (Vector2)transform.position - offset * transform.localScale.x, Quaternion.identity);
                Go.GetComponent<Rigidbody2D>().velocity = new Vector2(-Velocity.x * transform.localScale.x, Velocity.y);
                canShoot = false;
            }

        }


    }


}
