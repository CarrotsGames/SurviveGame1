using UnityEngine;
using System.Collections;

public class HelpUpScript : MonoBehaviour
{
    float a;
    private void OnTriggerStay2D(Collider2D collision)
    {
         
           if (collision.gameObject.tag == "Player")
            {
                Debug.Log("SupFren2");
            }
            if (collision.gameObject.tag == "Player2")
            {
                Debug.Log("SupFren1");
            }
         
    }
}
