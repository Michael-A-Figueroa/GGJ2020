using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public float maxVolume = 1;
    AudioSource aud;
    private static AudioManager _instance;
    bool fadeOut, fadeIn;
    float curVolume;
    AudioClip newClip;

    private void Awake()
    {
        if (!_instance)
            _instance = this;
        else
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }
    private void Start()
    {
        aud = GetComponent<AudioSource>();
        curVolume = aud.volume;
    }

    private void Update()
    {
        curVolume = Mathf.Clamp(curVolume, 0, maxVolume);
        aud.volume = curVolume;

        if (fadeOut)
        {
            curVolume -= Time.deltaTime * 2;

            if (curVolume <= 0) { fadeIn = true; fadeOut = false; }
        }
        if (fadeIn)
        {
            aud.Stop();
            aud.clip = newClip;
            aud.Play();

            curVolume += Time.deltaTime * 2;

            if (curVolume >= maxVolume) fadeIn = false;
        }



    }

    public void OnSceneChange(AudioClip clip)
    {
        newClip = clip;
        fadeOut = true;
    }
}
