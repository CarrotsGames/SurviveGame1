using UnityEngine;
using System.Collections;
using XboxCtrlrInput;

public class HelpUpScript : MonoBehaviour
{
    GameObject PlayerGameObj1;
    GameObject PlayerGameObj2;


    void start()
    {
        PlayerGameObj1 = GameObject.FindGameObjectWithTag("Player");
        PlayerGameObj2 = GameObject.FindGameObjectWithTag("Player2");

    }
    private void Update()
    {
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

    }
    private void OnTriggerStay2D(Collider2D collision)
    {

        if (collision.gameObject.tag == "Player2")
        {
            Debug.Log("SupFren1");
            if (XCI.GetButtonDown(XboxButton.B))
            {
                Vector2 offset = new Vector2(-2, 2) * Time.deltaTime;
                collision.gameObject.transform.position = (Vector2)transform.position + offset * transform.localScale.x;

            }
        }
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("SupFren1");
            if (XCI.GetButtonDown(XboxButton.B))
            {
                Vector2 offset = new Vector2(-2, 2) * Time.deltaTime;
                collision.gameObject.transform.position = (Vector2)transform.position + offset * transform.localScale.x;

            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.tag == "Player2")
        {
            Debug.Log("SupFren1");
            if (XCI.GetButtonDown(XboxButton.B))
            {
                Vector2 offset = new Vector2(-2, 2) * Time.deltaTime;
                collision.gameObject.transform.position = (Vector2)transform.position + offset * transform.localScale.x;

            }
        }
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("SupFren1");
            if (XCI.GetButtonDown(XboxButton.B))
            {
                Vector2 offset = new Vector2(-2, 2) * Time.deltaTime;
                collision.gameObject.transform.position = (Vector2)transform.position + offset * transform.localScale.x;

            }
        }

    }

}

