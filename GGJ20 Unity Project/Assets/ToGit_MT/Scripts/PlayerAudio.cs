using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudio : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip[] audioClips;

    public float clipWaitTime = 5;

    public bool playClips;

    public void PlayClipLoop()
    {
        playClips = true;
        Invoke("RandomAudioClip", clipWaitTime);
    }

    public void StopClipLoop()
    {
        playClips = false;
    }

    void RandomAudioClip()
    {
        if (playClips)
        {
            audioSource.clip = audioClips[Random.Range(0, audioClips.Length)];
            audioSource.Play();
            PlayClipLoop();
        }
    }
}
