using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneControl : MonoBehaviour
{
    public bool changeScene;

    public float timedDelay;
    public float reductionSpeed;
    [Space]
    public string sceneName;

    // Update is called once per frame
    void Update()
    {
        if (!changeScene)
            return;

        timedDelay -= Time.deltaTime * reductionSpeed;

        if (timedDelay <= 0)
            SceneManager.LoadScene(sceneName);

    }

    public void ChangeScene(string sceneToLoad)
    {
        SceneManager.LoadScene(sceneToLoad);
    }
}
