using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class OvenInteraction : MonoBehaviour
{
    [SerializeField] ObjectiveManager objectiveManager;
    [SerializeField] PlayerInteraction playerInteraction;
    [SerializeField] PizzariaController pizzariaController;

    [SerializeField] Transform playerPizzaHolder;

    [SerializeField] GameObject player;
    [SerializeField] GameObject ovenCamera;

    GameObject ovenPizzaHolder;

    int ovenInteractionState = 0;

    [SerializeField] Slider heatSlider;

    float heatGainSpeed = 50;
    float heatLossSpeed = 100;

    [SerializeField] bool ovenHeatGain = false;
    bool startheatDetection = false;

    [SerializeField] float heatGainTime;
    [SerializeField] float heatGainTimeLimit;

    bool heatLock = false;
    bool hasWon = false;


    float moneyToEarn = 10;
    float moneyMultiplier = 1;
    bool perfectExecution = true;

    public AudioSource ovenOpenClose;
    public AudioSource ovenOn;

    public AudioClip ovenOpen;
    public AudioClip ovenClose;
    public AudioClip ovenHum;
    public AudioClip ovenPing;


    void Start()
    {
        ovenPizzaHolder = transform.GetChild(0).gameObject;

        heatSlider.minValue = 0;
        heatSlider.maxValue = 100;
    }

    void Update()
    {
        if(ovenHeatGain)
        {
            heatSlider.value += heatGainSpeed * Time.deltaTime;
        }
        else
        {
            heatSlider.value -= heatLossSpeed * Time.deltaTime;
        }

        heatRangeDetection();
    }

    public void OvenInteract()
    {
        if(objectiveManager.PizzaCompleet)
        {
            StartCoroutine(playerInteraction.PopUpText(2, "I already baked the pizza"));
            return;
        }

        if(objectiveManager.PizzaGrabbed)
        {
            switch(ovenInteractionState)
            {
                case 0:
                    //open oven
                    ovenInteractionState++;
                    ovenOpenClose.clip = ovenOpen;
                    ovenOpenClose.Play();
                    break;
                  
                case 1:
                    //place pizza
                    playerPizzaHolder.GetChild(0).transform.parent = ovenPizzaHolder.transform;
                    ovenPizzaHolder.transform.GetChild(0).transform.position = transform.GetChild(0).transform.position;
                    ovenPizzaHolder.transform.GetChild(0).transform.rotation = transform.GetChild(0).transform.rotation;
                    objectiveManager.PizzaGrabbed = false;
                    objectiveManager.ChangeLayer(ovenPizzaHolder.transform.GetChild(0).gameObject,0);

                    ovenInteractionState++;
                    break;

                default:
                    Debug.LogWarning("Int out of bounds. OvenInteraction/StartOvenMinigame");
                    break;
            }            
        }
        else if(hasWon)
        {
            ovenPizzaHolder.transform.GetChild(0).parent = playerPizzaHolder;
            playerPizzaHolder.GetChild(0).transform.position = playerPizzaHolder.position;
            playerPizzaHolder.GetChild(0).transform.rotation = playerPizzaHolder.rotation;

            objectiveManager.OvenMinigameCompleted = true;
            objectiveManager.PizzaGrabbed = true;

            objectiveManager.ChangeLayer(playerPizzaHolder.GetChild(0).gameObject,9);
        }
        else if(ovenInteractionState == 2)
        {
            StartCoroutine(StartMinigame());

            //switch camera
            ovenCamera.SetActive(true);
            pizzariaController.LockPlayer(true);
            ovenOn.clip = ovenHum;
            ovenOn.Play();

            for(int i = 0; i < player.transform.childCount; i++)
            {
                player.transform.GetChild(i).gameObject.SetActive(false);
            }
        }
        else
        {
            StartCoroutine(playerInteraction.PopUpText(2, "First I need to add toppings to the pizza"));
        }
    }

    IEnumerator StartMinigame()
    {
        //close oven
        ovenOpenClose.clip = ovenClose;
        ovenOpenClose.Play();
        ovenOn.clip = ovenHum;
        ovenOn.Play();
        yield return new WaitForSeconds(2);

        startheatDetection = true;
    }

    public void OnOvenHeatIncrease(InputAction.CallbackContext context)
    {
        ovenHeatGain = context.performed;
    }

    void heatRangeDetection()
    {
        if(startheatDetection)
        {
            while(heatSlider.value > 150 && heatSlider.value < 180)
            {
                heatLock = true;

                if(heatGainTime < heatGainTimeLimit)
                {
                    heatGainTime += Time.deltaTime;
                }
                else
                {
                    if(perfectExecution)
                    {
                        moneyMultiplier *= 1.5f;
                        moneyMultiplier = (float)Math.Round(moneyMultiplier, 2);
                    }
                    moneyToEarn *= moneyMultiplier;

                    objectiveManager.MoneyToAdd(moneyToEarn);

                    hasWon = true;
                    startheatDetection = false;
                    ovenInteractionState = 0;

                    pizzariaController.LockPlayer(false);
                    for(int i = 0; i < player.transform.childCount; i++)
                    {
                        player.transform.GetChild(i).gameObject.SetActive(true);
                    }

                    ovenCamera.SetActive(false);

                    ovenOn.clip = ovenPing;
                    ovenOn.Play();

                    ovenOpenClose.clip = ovenOpen;
                    ovenOpenClose.Play();
                }

                if(heatLock && heatSlider.value < 150 || heatGainTime > 180)
                {
                    moneyMultiplier *= 0.75f;
                    moneyMultiplier = (float)Math.Round(moneyMultiplier, 2);
                    
                    perfectExecution = false;
                }
            }
        }
    }

    public void ResetOvenStates()
    {
        hasWon = false;
    }
}
