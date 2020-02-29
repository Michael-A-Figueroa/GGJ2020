using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneControl : MonoBehaviour
{
    [Header("UI")]
    public GameObject loadScreen;
    public Text progressText;
    public Slider slider;

    [Space]
    [Header("Only Called by Script")]
    [Tooltip("By Script Call ONLY")]
    public string newScene;
    [Header("Audio")]
    public AudioClip newBackgroundMusic;

    AsyncOperation operation;

    public void LoadScene(string sceneName)
    {
        UpdateProgressUI(0);
        loadScreen.SetActive(true);
        FindObjectOfType<AudioManager>().OnSceneChange(newBackgroundMusic);

        StartCoroutine(BeginLoad(sceneName));
    }

    IEnumerator BeginLoad(string sceneName)
    {
        operation = SceneManager.LoadSceneAsync(sceneName);

        while (!operation.isDone)
        {
            UpdateProgressUI(operation.progress);
            yield return null;
        }

        UpdateProgressUI(operation.progress);
        operation = null;
        loadScreen.SetActive(false);
    }

    void UpdateProgressUI(float progress)
    {
        slider.value = progress;
        progressText.text = (int)(progress * 100) + "%";
    }


    public void ExitGame()
    {
        Application.Quit();
    }
}
