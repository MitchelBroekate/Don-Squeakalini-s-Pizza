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

    float heatGainSpeed = 50;
    float heatLossSpeed = 100;

    [SerializeField] bool ovenHeatGain = false;


    [Header("Minigame Zone Variables")]
    [SerializeField] GameObject zoneVisualPrefab;
    [SerializeField] float requiredHoldTime = 6f;
    
    float minZoneValue = 25f;
    float maxZoneValue = 45f;
    float currentHoldTime = 0f;
    bool isInZone = false;
    bool hasWon = false;
    RectTransform zoneVisual;

    void Start()
    {
        ovenPizzaHolder = transform.GetChild(0).gameObject;
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

        SliderZoneCheck();

        if(Input.GetButtonDown("Fire1"))
        {
            CreateAndSetupZoneVisual();
        }
    }

    public void OvenInteract()
    {
        if(objectiveManager.PizzaGrabbed)
        {
            switch(ovenInteractionState)
            {
                case 0:
                    //open oven
                    ovenInteractionState++;
                    break;

                case 1:
                    //place pizza
                    playerPizzaHolder.GetChild(0).transform.parent = ovenPizzaHolder.transform;
                    ovenPizzaHolder.transform.GetChild(0).transform.position = transform.GetChild(0).transform.position;
                    ovenPizzaHolder.transform.GetChild(0).transform.rotation = transform.GetChild(0).transform.rotation;
                    objectiveManager.PizzaGrabbed = false;

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
            ovenPizzaHolder.transform.GetChild(0).transform.position = playerPizzaHolder.position;
            ovenPizzaHolder.transform.GetChild(0).transform.rotation = playerPizzaHolder.rotation;
        }
        else if(ovenInteractionState == 2)
        {
            StartCoroutine(StartMinigame());

            //switch camera
            ovenCamera.SetActive(true);
            pizzariaController.LockPlayer(true);
            for(int i = 0; i < player.transform.childCount; i++)
            {
                player.transform.GetChild(i).gameObject.SetActive(false);
            }
        }
        else
        {
            StartCoroutine(playerInteraction.PopUpText(2, "I need to finish the pizza first"));
        }
    }

    IEnumerator StartMinigame()
    {
        //close oven

        yield return new WaitForSeconds(2);

        CreateAndSetupZoneVisual();
        
    }

    public void OnOvenHeatIncrease(InputAction.CallbackContext context)
    {
        ovenHeatGain = context.performed;
    }

    void SliderZoneCheck()
    {
        if (hasWon) return;

        isInZone = heatSlider.value >= minZoneValue && heatSlider.value <= maxZoneValue;

        if (isInZone)
        {
            currentHoldTime += Time.deltaTime;
            
            if (currentHoldTime >= requiredHoldTime)
            {
                OnWin();
            }
        }
        else
        {
            currentHoldTime = 0f;
        }

        Debug.Log($"Value: {heatSlider.value:F1}, In Zone: {isInZone}, Hold Time: {currentHoldTime:F2}");
    }

    private void CreateAndSetupZoneVisual()
    {
        GameObject zoneInstance = Instantiate(zoneVisualPrefab, heatSlider.transform);
        zoneInstance.transform.parent = heatSlider.transform;
        zoneVisual = zoneInstance.GetComponent<RectTransform>();

        zoneVisual.SetAsLastSibling();

        SetRandomZone();
    }

    private void SetupZoneVisual()
    {
        float parentWidth = zoneVisual.rect.width;
        float parentHeight = zoneVisual.rect.height;

        zoneVisual.GetComponent<RectTransform>().anchoredPosition = new Vector2(newX, 0);
    }

    private void OnWin()
    {
        Debug.Log("You Win!");
        hasWon = true;

        ovenInteractionState = 0;

        pizzariaController.LockPlayer(false);
        for(int i = 0; i < player.transform.childCount; i++)
        {
            player.transform.GetChild(i).gameObject.SetActive(true);
        }
        ovenCamera.SetActive(false);

        //OnDestroy();
    }

    public void SetRandomZone(float minRange = 40f)
    {
        minZoneValue = Random.Range(0f, 100f - minRange);
        maxZoneValue = minZoneValue + minRange;
        currentHoldTime = 0f;
        hasWon = false;
        SetupZoneVisual();
    }

    // Clean up when destroyed
    void OnDestroy()
    {
        if (zoneVisual != null)
        {
            Destroy(zoneVisual.gameObject);
        }
    }
}
