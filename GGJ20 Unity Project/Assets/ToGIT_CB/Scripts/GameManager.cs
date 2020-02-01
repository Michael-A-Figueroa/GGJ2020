using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    #region Variables
    [Header("Character Settings")]
    public int cash;
    public int monthlyBills;

    [Header("Time Settings")]
    public float taskCompletionTime = 10;
    float curTime;
    public float timeReductionRate = 1f;
    public float timeReductionIncreaseRate = 0.1f;
    [Space]
    [Header("Car Settings")]
    public GameObject carPrefab;
    [Tooltip("Point to which the car needs to be repaired at")]
    public Transform repairPoint;
    [Tooltip("Point which the car will go to after vehicle is repaired")]
    public Transform exitPoint;
    [Tooltip("Time it takes to lerp between Repair Point and Exit Point")]
    public float exitTransitionTime = 5f;
    public float entryTransitionTime = 5f;
    [Space]
    [Header("Bill Collector Settings")]
    public int daysTellPayment = 30; //Day the Bill Collector Shows up
    int curDays = 1; //Current Days
    public float hoursInDay = 24; //Maximum time in a Day
    float curHour; //Current Time for Day
    public float hoursReductionRate = 5; //Reduction of Time per second for Day
    public AudioClip[] collectorPhrases;
    [Header("CHAD Settings")]
    public GameObject chadImage;
    public AudioClip[] chadPhrases;
    public Vector2 chadRandomMinMax = new Vector2(5, 10);

    [Header("GUI Settings")]
    public Image timerSlider;
    public Slider billCollectorSlider;
    public Text cashCounterText;

    [Header("Audio Settings")]
    public AudioSource NPCAudio;
    public AudioSource PlayerAudio;
    float npcAudioLength;

    [Header("Hazard 1")]
    public Light[] Lights;
    public float lightIntensity;

    [Header("Hazard 3")]
    public GameObject catPrefab;
    public Transform catSpawnPoint;

    [Header("End Game")]
    public Camera deathCam;
    public GameObject HUD;
    Animator playerAnim;



    //Shop Attributes
    bool carInLot, timesUp, carCompleted, stopTime;
    bool hazard1, hazard2, hazard3;
    GameObject curCar;
    float randomTime; //For CHAD's Audio
    #endregion

    void Start()
    {
        #region Default
        chadImage.SetActive(false);
        curTime = taskCompletionTime;
        billCollectorSlider.minValue = 0;
        billCollectorSlider.maxValue = daysTellPayment;
        SpawnCar();

        playerAnim = GameObject.FindGameObjectWithTag("Player").GetComponent<Animator>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        #endregion
    }

    void Update()
    {
        if (stopTime)
            return;

        #region Car/Garage Controls
            #region Time Reduction - Completed
            if (curTime > 0 && carInLot) curTime -= timeReductionRate * Time.deltaTime; //Reduce Time
            else if (curTime <= 0 && carInLot) timesUp = true; //Increase Difficulty
            #endregion
            #region Car Transitions - Completed
            if (curCar != null)
            {
                if (!carInLot) //Move Car to Repair Point
                {
                    curCar.transform.position = Vector3.Lerp(curCar.transform.position, repairPoint.position, entryTransitionTime * Time.deltaTime);
                    if (curCar.transform.position == repairPoint.position) carInLot = true;
                }
                else if (carInLot) //Move Car To Exit Point
                {
                    if (timesUp || carCompleted)
                    {
                        curTime = 0;
                        curCar.transform.position = Vector3.Lerp(curCar.transform.position, exitPoint.position, exitTransitionTime * Time.deltaTime);
                        if (curCar.transform.position == exitPoint.position) { Destroy(curCar); curCar = null; UpgradeTime(); carInLot = false; }
                    }
                }
            }
            #endregion
            #region Update GUI Timer - Completed
            timerSlider.fillAmount = curTime / taskCompletionTime;
            #endregion
        #endregion
        #region Bill Collector Controls - Completed
        if (curHour > 0) curHour -= hoursReductionRate * Time.deltaTime;
        else if(curHour <= 0)
        {
            curDays += 1;
            curHour = hoursInDay;

            if (curDays > daysTellPayment)
            {
                //Disable Chads Audio Counter
                NPCAudio.Stop();
                StopCoroutine(ChadWaitTime());

                //Play Collectors Audio
                int randomBankerPhrase = Random.Range(0, collectorPhrases.Length);
                NPCAudio.clip = collectorPhrases[randomBankerPhrase];
                NPCAudio.Play();


                //Run Check to see if Player has enough Money
                if (cash < monthlyBills)
                {
                    cash = 0;

                    //Run Hazard if Fails
                    //3 Warnings follwed by hazard then end Game
                    if (!hazard1)
                    {
                        hazard1 = true;
                        curDays = 1; //Reset Days
                        //Play Audio: Bill Collector
                        //Activate Hazard 1 = Electricity
                        Hazard1();
                    }
                    else if (!hazard2)
                    {
                        hazard2 = true;
                        curDays = 1; //Reset Days
                        //Play Audio: Bill Collector
                        Hazard2();
                    }
                    else if (!hazard3)
                    {
                        hazard3 = true;
                        curDays = 1; //Reset Days
                        Hazard2();
                    }
                    else
                    {
                        //End Game
                        stopTime = true;
                        GetComponent<SceneControl>().changeScene = true;
                        Camera.main.gameObject.SetActive(false);
                        deathCam.gameObject.SetActive(true);
                        FindObjectOfType<PlayerMovement>().enabled = false;
                        playerAnim.SetFloat("moveX", 0);
                        playerAnim.SetFloat("moveY", 0);
                        playerAnim.applyRootMotion = false;
                        playerAnim.SetTrigger("Lose");
                        HUD.SetActive(false);
                        deathCam.GetComponent<Animator>().SetTrigger("Activate");
                    }
                }
                else
                {
                    cash -= monthlyBills; //Remove Cash
                    curDays = 1; //Reset Days
                }               
            }

        }

            #region Update GUI Timer - Completed
            billCollectorSlider.value = curDays;
        #endregion
        #endregion
        #region CHAD Controls - Completed
        if (carInLot)
        {
            if(!timesUp && !carCompleted && randomTime == 0 && !NPCAudio.isPlaying && curDays < daysTellPayment - 5)
            {
                randomTime = Random.Range(chadRandomMinMax.x, chadRandomMinMax.y);
                StartCoroutine(ChadWaitTime());
            }
        }

        if (!NPCAudio.isPlaying)
        {
            chadImage.SetActive(false);
            NPCAudio.clip = null;
        }
        #endregion
        #region Other
        cashCounterText.text = cash.ToString();
        #endregion
    }

    #region Functions
    void UpgradeTime()
    {
        //This Runs if Time Runs out or Car is Completed
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////
        #region Check for Vehicle Completion - INCOMPLETE
        //Run Check to see if all parts were completed (INCOMPLETE)
        //      *If TRUE give Full Monetary Value
        //      *If FALSE give Monetary Value divided by all the Parts ([Full Price / Total PCS] * Completed PCS)
        #endregion
        #region Reduction Rate Increase - Completed
        timeReductionRate += timeReductionIncreaseRate;
        #endregion
        #region Remove/Replace Car in Lot - Completed
        //Remove Car from Lot
        //Bring a new Car into Lot
        SpawnCar();
        #endregion
        #region Reset Time - Completed
        curTime = taskCompletionTime;
        #endregion
        #region Stop Time Temporarily to Spawn new Car
        carInLot = false;
        carCompleted = false;
        timesUp = false;
        #endregion
    }
    void SpawnCar()
    {
        if (curCar != null)
            return;

        curCar = Instantiate(carPrefab) as GameObject;
        curCar.transform.position = exitPoint.position;
        curCar.transform.rotation = exitPoint.rotation;
    }
    IEnumerator ChadWaitTime()
    {
        yield return new WaitForSeconds(randomTime);

        //Set Active Image: Chad
        chadImage.SetActive(true);
        //Play Random Audio: Chad
        int randomAudioClip = Random.Range(0, chadPhrases.Length);
        NPCAudio.clip = chadPhrases[randomAudioClip];
        NPCAudio.Play();

        StopCoroutine(ChadWaitTime());

        randomTime = 0; //Resets Counter
    }
    void Hazard1()
    {
        for (int i = 0; i < Lights.Length; i++)
        {
            Lights[i].intensity = lightIntensity;
            Lights[i].GetComponentInChildren<ParticleSystem>().Play();
        }
    }
    void Hazard2()
    {
        GameObject spawn = Instantiate(catPrefab) as GameObject;
        spawn.transform.position = catSpawnPoint.position;
        spawn.transform.rotation = catSpawnPoint.rotation;
    }
    void Hazard3()
    {
        
    }
    #endregion
}
