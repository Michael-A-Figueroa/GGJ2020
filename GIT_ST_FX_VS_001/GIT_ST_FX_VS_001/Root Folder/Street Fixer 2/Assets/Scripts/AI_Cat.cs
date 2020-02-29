using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AI_Cat : MonoBehaviour
{
    public float wanderTimer;
    float timer;
    NavMeshAgent nm;

    private void Start()
    {
        nm = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        timer += Time.deltaTime;

        if (timer >= wanderTimer)
        {
            Vector3 newPos = RandomNavSphere(transform.position, 500, -1);
            nm.SetDestination(newPos);
            timer = 0;
        }
    }

    public static Vector3 RandomNavSphere(Vector3 origin, float distance, int layermask)
    {
        Vector3 randomDirection = Random.insideUnitSphere * distance;

        randomDirection += origin;

        NavMeshHit navHit;

        NavMesh.SamplePosition(randomDirection, out navHit, distance, layermask);

        return navHit.position;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<PartSocket>())
        {
            collision.gameObject.GetComponent<PartSocket>().Damage();
        }

        if (collision.gameObject.GetComponent<Barrels>())
        {
            collision.gameObject.GetComponent<Rigidbody>().AddForce(transform.forward * 500);
        }
    }
}
