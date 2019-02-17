using UnityEngine;
using System.Collections;
[RequireComponent(typeof(Rigidbody2D))]

public class SpearScript : MonoBehaviour
{
    GameObject PlayerObj;
    SpearThrowScript ThrowSpear;
    public Rigidbody2D Rb;

    private void Start()
    {
        PlayerObj = GameObject.FindGameObjectWithTag("Player");
        ThrowSpear = PlayerObj.transform.GetChild(0).GetComponent<SpearThrowScript>();
    }
    private void Update()
    {
        if (ThrowSpear.HowManySpears == 2)
        {
            Destroy(gameObject);
            ThrowSpear.HowManySpears = 1;
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.gameObject.layer == 9)
        {
            Rb.isKinematic = true;

        }
    }
}
