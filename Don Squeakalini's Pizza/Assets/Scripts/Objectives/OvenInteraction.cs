using System;
using System.Collections;
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
    [SerializeField] Slider timerSlider;

    [SerializeField] float heatGainSpeed;
    [SerializeField] float heatLossSpeed;

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

    [SerializeField] Material pizzaBaked;
    [SerializeField] Material cheeseMolten;

    [SerializeField] Transform ovenDoor;

    [SerializeField] bool rotateOpenDoor = false;
    [SerializeField] bool rotateCloseDoor = false;

    void Start()
    {
        ovenPizzaHolder = transform.GetChild(0).gameObject;
    }

    void FixedUpdate()
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

    void Update()
    {
        if(rotateOpenDoor)
        {
            float rotationSpeed = 180f;
            Quaternion targetRotation = Quaternion.Euler(90, 180, 0);

            ovenDoor.rotation = Quaternion.RotateTowards(ovenDoor.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
        if(rotateCloseDoor)
        {
            float rotationSpeed = 180f;
            Quaternion targetRotation = Quaternion.Euler(0, 180, 0);

            ovenDoor.rotation = Quaternion.RotateTowards(ovenDoor.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }

    public void OvenInteract()
    {
        if(objectiveManager.OvenMinigameCompleted)
        {
            StartCoroutine(playerInteraction.PopUpText(2, "I already baked the pizza"));
            return;
        }

        if(objectiveManager.PizzaGrabbed && !hasWon)
        {
            switch(ovenInteractionState)
            {
                case 0:
                    //open oven
                    ovenInteractionState++;
                    ovenOpenClose.clip = ovenOpen;
                    ovenOpenClose.Play();
                    rotateOpenDoor = true;
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
        else if(hasWon && !objectiveManager.PizzaGrabbed)
        {
            StartCoroutine(GrabPizza());
        }
        else if(ovenInteractionState == 2 && !hasWon)
        {
            rotateOpenDoor = false;
            StartCoroutine(StartMinigame());
        }
        else
        {
            StartCoroutine(playerInteraction.PopUpText(2, "First I need to add toppings to the pizza"));
        }
    }

    IEnumerator StartMinigame()
    {
        //close oven
        rotateCloseDoor = true;
        ovenOpenClose.clip = ovenClose;
        ovenOpenClose.Play();
        ovenOn.clip = ovenHum;
        ovenOn.Play();
        
        yield return new WaitForSeconds(1);

        rotateCloseDoor = false;

        ovenCamera.SetActive(true);
        pizzariaController.LockPlayer(true);
        ovenOn.clip = ovenHum;
        ovenOn.Play();

        for(int i = 0; i < player.transform.childCount; i++)
        {
            player.transform.GetChild(i).gameObject.SetActive(false);
        }

        startheatDetection = true;
    }

    IEnumerator GrabPizza()
    {
        rotateOpenDoor = true;

        ovenPizzaHolder.transform.GetChild(0).parent = playerPizzaHolder;
        playerPizzaHolder.GetChild(0).transform.position = playerPizzaHolder.position;
        playerPizzaHolder.GetChild(0).transform.rotation = playerPizzaHolder.rotation;

        objectiveManager.OvenMinigameCompleted = true;
        objectiveManager.PizzaGrabbed = true;

        objectiveManager.ChangeLayer(playerPizzaHolder.GetChild(0).gameObject,9);

        rotateOpenDoor = false;
        rotateCloseDoor = true;

        yield return new WaitForSeconds(0.5f);

        rotateCloseDoor = false;
    }

    public void OnOvenHeatIncrease(InputAction.CallbackContext context)
    {
        ovenHeatGain = context.performed;
    }

    void heatRangeDetection()
    {
        if(startheatDetection)
        {
            if(heatSlider.value > 155 && heatSlider.value < 175)
            {
                heatLock = true;

                if(heatGainTime < heatGainTimeLimit)
                {
                    heatGainTime += Time.deltaTime;
                    timerSlider.value = heatGainTime;
                }
                else
                {
                    if(perfectExecution)
                    {
                        moneyMultiplier *= 1.5f;
                        moneyMultiplier = (float)Math.Round(moneyMultiplier, 2);
                    }
                    ovenPizzaHolder.transform.GetChild(0).GetComponent<Renderer>().material = pizzaBaked;
                    ovenPizzaHolder.transform.GetChild(0).GetChild(0).GetComponent<Renderer>().material = cheeseMolten;

                    moneyToEarn *= moneyMultiplier;
                    print(moneyToEarn + "Oven");

                    objectiveManager.MoneyToAdd(moneyToEarn);

                    moneyMultiplier = 1;
                    moneyToEarn = 10;
                    heatGainTime = 0;
                    heatSlider.value = 0;
                    timerSlider.value = 0;

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
                    ovenOn.loop = false;
                    ovenOn.Play();

                    ovenOpenClose.clip = ovenOpen;
                    ovenOpenClose.Play();
                }
            }

            if(heatLock && heatSlider.value < 155 || heatGainTime > 175)
            {
                moneyMultiplier *= 0.75f;
                moneyMultiplier = (float)Math.Round(moneyMultiplier, 2);
                
                perfectExecution = false;
                heatLock = false;
            }

        }
    }

    public void ResetOvenStates()
    {
        hasWon = false;
    }
}
