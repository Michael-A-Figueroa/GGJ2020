using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpControl : MonoBehaviour
{
    public float rayLength = 6;
    public LayerMask interactLayers;
    Tool equippedItem;
    public Transform hand;
    public Animator anim;
    RaycastHit hit;

    private void Update()
    {
        if(Input.GetButtonDown("Fire1") && equippedItem != null)
        {
            anim.SetTrigger("useTool");

            RaycastHit hit;
            if(Physics.Raycast(transform.position, transform.forward, out hit, rayLength, interactLayers))
            {
                //Attack and or fix
                if (hit.collider.GetComponent<PartSocket>() == null)
                    return;

                AudioSource au = equippedItem.GetComponent<AudioSource>();

                #region Audio
                if (hit.collider.GetComponent<PartSocket>().part == equippedItem.repairType)
                {                  
                    au.clip = equippedItem.repairSound;
                    au.Play();
                }
                else if (hit.collider.GetComponent<PartSocket>().part != equippedItem.repairType)
                {
                    au.clip = equippedItem.damageSound;
                    au.Play();
                }
                #endregion

                Fix(hit.collider.GetComponent<PartSocket>());
            }
        }

        if (Input.GetButtonDown("Fire2"))
        {
            if (!equippedItem && Physics.Raycast(transform.position, transform.forward, out hit, rayLength, interactLayers))
            {
                if (hit.collider.GetComponent<Tool>()) PickUp(hit.collider.GetComponent<Tool>());
            }
            else
            {
                Drop();
            }
        }
    }

    void PickUp(Tool item)
    {
        if(equippedItem == null)
        {
            item.GetComponent<Collider>().enabled = false;
            item.GetComponent<Rigidbody>().useGravity = false;
            item.GetComponent<Rigidbody>().isKinematic = true;

            item.transform.position = hand.position;
            item.transform.rotation = hand.rotation;
            item.transform.parent = hand.transform;
            equippedItem = item;
        }
        else
        {
            Drop();

            item.GetComponent<Collider>().enabled = false;
            item.GetComponent<Rigidbody>().useGravity = false;
            item.GetComponent<Rigidbody>().isKinematic = true;
            item.transform.position = hand.position;
            item.transform.rotation = hand.rotation;
            item.transform.parent = hand.transform;
            equippedItem = item;
        }
    }
    void Drop()
    {
        if (equippedItem == null)
            return;

        equippedItem.GetComponent<Rigidbody>().useGravity = true;
        equippedItem.GetComponent<Rigidbody>().isKinematic = false;
        equippedItem.GetComponent<Collider>().enabled = true;
        equippedItem.transform.parent = null;
        equippedItem = null;
    }
    void Fix(PartSocket target)
    {
        if(target.part == equippedItem.repairType)
            target.Repair();
    }
}
