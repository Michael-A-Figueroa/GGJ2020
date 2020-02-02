using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFocus : MonoBehaviour
{
    public LayerMask layerMask;

    public float maxDistance;

    public GameObject focusHit;
    public GameObject focusCache;
    public bool highlight;

    public Color startColor;
    public Color partHealthLow;
    public Color partHealthMid;
    public Color partHealthFull;
    public int partHealth;

    public Color[] colorChoices;

    // Start is called before the first frame update
    void Start()
    {
        colorChoices[0] = partHealthFull;
        colorChoices[1] = partHealthFull;
        colorChoices[2] = partHealthMid;
        colorChoices[3] = partHealthLow;
    }

    // Update is called once per frame
    void Update()
    {
        // This would cast rays only against colliders in layer 8.
        // But instead we want to collide against everything except layer 8. The ~ operator does this, it inverts a bitmask.
        //layerMask = ~layerMask;

        RaycastHit hit;
        // Does the ray intersect any objects excluding the player layer
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, maxDistance, layerMask))
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.red);
            focusHit = hit.transform.gameObject;

            //Debug.Log("Did Hit: " + hit.collider.tag);
        }
        else if(focusCache != null)
        {
            focusHit = null;
            highlight = false;
            focusCache.GetComponent<Renderer>().material.color = startColor;
        }

        //highlight
        if (focusHit != null && (focusHit.CompareTag("part") || focusHit.CompareTag("tool")) && !highlight)
        {
            focusCache = focusHit;
            startColor = focusHit.GetComponent<Renderer>().material.color;

            if (focusHit.CompareTag("part"))
            {
                partHealth = focusHit.GetComponent<PartBehavior>().partHealth;
                focusHit.GetComponent<Renderer>().material.color = colorChoices[Mathf.Clamp(partHealth, 0, 3)];
            }
            else if (focusHit.CompareTag("tool"))
            {
                //focusHit.GetComponent<Renderer>().material.color =  Color.yellow;
                focusHit.GetComponent<ToolBehavior>().toolRenderer.material.color = Color.yellow;
            }

            highlight = true;
        }
        //un-highlight
        if (focusCache != null && !ReferenceEquals(focusCache, focusHit))
        {
            focusCache.GetComponent<Renderer>().material.color = startColor;

            highlight = false;
        }

        Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * maxDistance, Color.blue);
    }
}
