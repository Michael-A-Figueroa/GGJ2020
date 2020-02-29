using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    public float force;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Barrels>())
        {
            other.gameObject.GetComponent<Rigidbody>().AddForce(transform.right * -force);
        }
    }
}
