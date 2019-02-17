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
    private void OnTriggerStay2D(Collider2D collision)
    {

        if (collision.gameObject.tag == "Player2")
        {
            Debug.Log("SupFren1");
            if (XCI.GetButtonDown(XboxButton.B))
            {
                Vector2 offset = new Vector2(-2, 1) * Time.deltaTime;
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
                Vector2 offset = new Vector2(-2, 1) * Time.deltaTime;
                collision.gameObject.transform.position = (Vector2)transform.position + offset * transform.localScale.x;

            }
        }

    }

}

