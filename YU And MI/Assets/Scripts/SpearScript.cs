using UnityEngine;
using System.Collections;
[RequireComponent(typeof(Rigidbody2D))]

public class SpearScript : MonoBehaviour
{


    public Rigidbody2D Rb;


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.gameObject.layer == 9)
        {
            Rb.isKinematic = true;
            //  Rb.simulated = false;
        }
    }
}
