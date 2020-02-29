using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeSceneAuto : MonoBehaviour
{
    public AudioClip newAudio;

   public void ChangeScene(string newScene)
    {
        FindObjectOfType<AudioManager>().OnSceneChange(newAudio);
        SceneManager.LoadScene(newScene);
    }
}
