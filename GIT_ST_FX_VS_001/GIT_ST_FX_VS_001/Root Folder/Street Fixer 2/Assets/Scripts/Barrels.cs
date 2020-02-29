using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrels : MonoBehaviour
{
    public ParticleSystem explosion;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<PartSocket>())
        {
            //Cause Explosion
            Instantiate(explosion, transform.position, transform.rotation);
            //Add Damage to Vehicle
            collision.gameObject.GetComponent<PartSocket>().Damage();
            //Delay
            //Destroy Actor
            Destroy(gameObject);
        }

        //If Sword Hits it Destroy Barrel Anyway without causing damage
    }

    void DestroyBarrel()
    {
        Destroy(gameObject);
    }
}
