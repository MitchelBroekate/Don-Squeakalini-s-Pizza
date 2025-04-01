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

    bool hasWon = false;

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

        heatSlider.onValueChanged.AddListener(CheckheatSliderPosition);
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

        if(Input.GetButtonDown("Fire1"))
        {
            SetNewTargetZone();
        }
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

        SetNewTargetZone();
        
    }

    public void OnOvenHeatIncrease(InputAction.CallbackContext context)
    {
        ovenHeatGain = context.performed;
    }

    public GameObject targetZonePrefab;
    public RectTransform heatSliderParent;
    public TMP_Text  feedbackText;

    private GameObject targetZoneInstance;
    private float targetMin;
    private float targetMax;
    public float zoneSize = 30f;

    void SetNewTargetZone()
    {
        if (targetZoneInstance != null)
        {
            Destroy(targetZoneInstance);
        }

        targetMin = Random.Range(heatSlider.minValue, heatSlider.maxValue - zoneSize);
        targetMax = targetMin + zoneSize;

        targetZoneInstance = Instantiate(targetZonePrefab, heatSliderParent);
        RectTransform rectTransform = targetZoneInstance.GetComponent<RectTransform>();

        float heatSliderHeight = heatSlider.GetComponent<RectTransform>().rect.height;
        float normalizedMin = targetMin / heatSlider.maxValue;
        float normalizedMax = targetMax / heatSlider.maxValue;

        rectTransform.anchorMin = new Vector2(0.5f, normalizedMin);
        rectTransform.anchorMax = new Vector2(0.5f, normalizedMax);
        rectTransform.anchoredPosition = Vector2.zero;
    }

    void CheckheatSliderPosition(float value)
    {
        if (value >= targetMin && value <= targetMax)
        {
            hasWon = true;
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
    }

    public void RestartGame()
    {
        SetNewTargetZone();
    }

    public void ResetOvenStates()
    {
        hasWon = false;
    }
}
