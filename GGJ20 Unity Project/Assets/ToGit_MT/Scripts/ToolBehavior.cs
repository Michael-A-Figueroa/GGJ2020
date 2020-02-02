using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolBehavior : MonoBehaviour
{
    public int toolType;
    public SphereCollider toolTrigger;

    public AudioSource audioSource;
    public AudioClip pickupClip;
    public AudioClip dropClip;
    public AudioClip useCorrectClip;
    public AudioClip useIncorrectClip;

    public void PlayPickup()
    {
        //audioSource.clip = pickupClip;
        //audioSource.Play();
    }

    public void PlayDrop()
    {
        //audioSource.clip = dropClip;
        //audioSource.Play();
    }

    public void PlayCorrect()
    {
        //audioSource.clip = useCorrectClip;
        //audioSource.Play();
    }

    public void PlayIncorrect()
    {
        //audioSource.clip = useIncorrectClip;
        //audioSource.Play();
    }
}
