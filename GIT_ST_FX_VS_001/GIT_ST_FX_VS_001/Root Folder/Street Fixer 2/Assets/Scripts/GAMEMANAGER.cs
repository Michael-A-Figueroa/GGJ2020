using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GAMEMANAGER : MonoBehaviour
{

    //iF YOU GET WITHIN 30 SECONDS THE LIGHTS START TO BLINK 

    [Header("Time Settings")]
    public float timer = 90;
    public float timerSpeed = 1;
    float curTime;
    public Image timerSlider;
    public Color goodColor = Color.green;
    public Color alertColor = Color.yellow;
    public Color warningColor = Color.red;
    float colorTimerTransition = 0.2f;
    [Header("Vehicle Settings")]
    public GameObject vehiclePrefab;
    public Transform repairPoint;
    public Transform exitPoint;
    float vehicleTransition = 0.1f;
    GameObject selectedVehicle;
    [SerializeField]bool isFixed;
    List<PartSocket> parts;
    [Header("Random Dialogue")]
    public AudioSource npcAudio;
    public AudioClip[] delmorDialogue;
    public AudioClip[] chadDialogue;
    public AudioClip[] bossDialogue;
    public GameObject delmorPortrait;
    public GameObject chadPortrait;
    public GameObject bossPortrait;
    public Vector2 speechRandomizer = new Vector2(5, 10);
    int characterSelect;
    float randomTime;
    int randomAudioClip;
    [Space]
    [Header("Hazard 1 - Black Out")]
    public bool useHazard1;
    public Light[] lightsToShutOff;
    public float lightIntensity;
    [Header("Hazard 2 - Fire")]
    public bool useHazard2;
    public ParticleSystem[] firesToStart;

    bool gameOver;

    void Start()
    {
        #region Default
        curTime = timer; //Set Timer
        randomTime = 0;
        #endregion
    }

    void Update()
    {
        if (gameOver)
            return;

        #region Timer Control - Complete
        curTime -= Time.deltaTime * timerSpeed; //Time Reduction

        timerSlider.fillAmount = curTime / timer;

        //Set Timer Slider Color
        if(curTime <= timer / 3) timerSlider.color = Color.Lerp(timerSlider.color, warningColor, colorTimerTransition);
        else if (curTime <= timer / 2 && curTime > timer / 3) timerSlider.color = Color.Lerp(timerSlider.color, alertColor, colorTimerTransition);
        else if (curTime <= timer / 1 && curTime > timer / 2) timerSlider.color = Color.Lerp(timerSlider.color, goodColor, colorTimerTransition);

        if(curTime <= 0)
        {
            GameOver();
        }
        #endregion
        #region Vehicle Control - InComplete
        //Spawn Vehicle
        if (selectedVehicle == null) SpawnVehicle();

        //Control Vehicle Movement
        if(selectedVehicle != null)
        {
            if(!isFixed && selectedVehicle.transform.position != repairPoint.position) selectedVehicle.transform.position = Vector3.Lerp(selectedVehicle.transform.position, repairPoint.position, vehicleTransition);
            if (isFixed)
            {
                if (selectedVehicle.transform.position != exitPoint.position) selectedVehicle.transform.position = Vector3.Lerp(selectedVehicle.transform.position, exitPoint.position, vehicleTransition);
                if(selectedVehicle.transform.position == exitPoint.position)
                {
                    AddTime(timer, 0.1f);
                    parts = null;
                    Destroy(selectedVehicle);
                    selectedVehicle = null;
                    isFixed = false;
                }
            }

            //Inspect Vehicle isFixed State
            
        }
        #endregion
        #region Dialogue Control
        if (!npcAudio.isPlaying && randomTime == 0)
        {
            delmorPortrait.SetActive(false);
            chadPortrait.SetActive(false);
            bossPortrait.SetActive(false);
            characterSelect = Random.Range(0, 99);
            randomAudioClip = Random.Range(0, delmorDialogue.Length);
            randomTime = Random.Range(speechRandomizer.x, speechRandomizer.y);

            StartCoroutine(DialogueWaitTime());
        }
        #endregion
    }

    #region Timer Functions - Complete
    public void AddTime(float timeToAdd, float timerSpeedIncrease)
    {
        timerSpeed += timerSpeedIncrease;

        if(curTime + timeToAdd > timer) { curTime = timer; }
        else { timer += timeToAdd; }
    }
    public void RemoveTime(float amount)
    {
        if(curTime - amount < 0) { GameOver(); }
        else { curTime -= amount; }
    }
    #endregion
    #region Vehicle Functions - Complete
    void SpawnVehicle()
    {
        selectedVehicle = Instantiate(vehiclePrefab) as GameObject;
        selectedVehicle.transform.position = exitPoint.position;
        selectedVehicle.transform.rotation = exitPoint.rotation;
        parts = new List<PartSocket>(selectedVehicle.GetComponentsInChildren<PartSocket>());
    }
    
    public void AddPartToFix(PartSocket newPart)
    {
        parts.Add(newPart);
        CheckFixedParts();
    }

    public void CheckFixedParts()
    {
        for (int i = 0; i < parts.Count; i++)
        {
            if (parts[i].isFixed)
                parts.Remove(parts[i]);

            if (parts.Count == 0)
            {
                isFixed = true;         
            }
        }
    }
    #endregion
    #region Dialogue Functions - Complete
    IEnumerator DialogueWaitTime()
    {
        yield return new WaitForSeconds(randomTime);
          
        if (characterSelect <= 33) //Play Delmor
        {
            delmorPortrait.SetActive(true);
            npcAudio.clip = delmorDialogue[randomAudioClip];
            npcAudio.Play();
        }
        else if (characterSelect > 33 && characterSelect <= 66) //Play Chad
        {
            chadPortrait.SetActive(true);
            npcAudio.clip = chadDialogue[randomAudioClip];
            npcAudio.Play();
        }
        else if (characterSelect > 66) //Play Boss
        {
            bossPortrait.SetActive(true);
            npcAudio.clip = bossDialogue[randomAudioClip];
            npcAudio.Play();
        }

        StopCoroutine(DialogueWaitTime());
        randomTime = 0;
    }
    #endregion

    void Hazard1()
    {
        for (int i = 0; i < lightsToShutOff.Length; i++)
        {
            lightsToShutOff[i].intensity = lightIntensity;
        }

        curTime = timer;
        timerSpeed -= 0.2f;
        useHazard1 = false;
    }
    void Hazard2()
    {
        for (int i = 0; i < firesToStart.Length; i++)
        {
            firesToStart[i].Play();
        }

        curTime = timer;
        timerSpeed -= 0.2f;
        useHazard2 = false;
    }

    public void GameOver()
    {
        if (useHazard1)
        {
            Hazard1();
            return;
        }
        else if (useHazard2)
        {
            Hazard2();
            return;
        }
            

        gameOver = true;

        //Add Pause Here for Animation
        //Activate Player Animation
        //Change Scene

        GetComponent<SceneControl>().LoadScene(GetComponent<SceneControl>().newScene);
    }
}
