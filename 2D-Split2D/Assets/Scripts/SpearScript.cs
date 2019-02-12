using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpearScript : MonoBehaviour
{


    public Rigidbody2D Rb;


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.gameObject.layer == 8)
        {
            Rb.isKinematic = true;
          //  Rb.simulated = false;
        }
    }
}
