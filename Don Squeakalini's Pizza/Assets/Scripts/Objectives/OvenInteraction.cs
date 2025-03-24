using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class OvenInteraction : MonoBehaviour
{
    [SerializeField] ObjectiveManager objectiveManager;
    [SerializeField] PlayerInteraction playerInteraction;
    [SerializeField] Transform playerPizzaHolder;

    GameObject ovenPizzaHolder;

    int ovenInteractionState = 0;

    void Start()
    {
        ovenPizzaHolder = transform.GetChild(0).gameObject;
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

                    ovenInteractionState++;
                    break;

                case 2:
                    //close oven and start minigame
                    StartCoroutine(StartMinigame());
                    break;

                default:
                    Debug.LogWarning("Int out of bounds. OvenInteraction/StartOvenMinigame");
                    break;
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



        
    }
}
