using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolBehavior : MonoBehaviour
{
    public int toolType;
    public SphereCollider toolTrigger;
    public Renderer toolRenderer;

    public AudioSource audioSource;
    public AudioClip pickupClip;
    public AudioClip dropClip;
    public AudioClip useCorrectClip;
    public AudioClip useIncorrectClip;

    private void Start()
    {
        toolTrigger = GetComponent<SphereCollider>();
        toolRenderer = GetComponent<Renderer>();
        audioSource = GetComponent<AudioSource>();
    }

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
