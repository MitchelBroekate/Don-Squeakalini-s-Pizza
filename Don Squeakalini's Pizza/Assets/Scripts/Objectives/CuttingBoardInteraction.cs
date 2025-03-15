using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuttingBoardInteraction : MonoBehaviour
{
    [SerializeField] ObjectiveManager objectiveManager;
    [SerializeField] PlayerInteraction playerInteraction;

    [SerializeField] GameObject player;
    [SerializeField] GameObject cuttingCam;
    
    [SerializeField] Transform itemHolder;

    [SerializeField] IngredientSO IngrdientDough;
    IngredientSO currentIngredient;
    bool doughRollingCompleted = false;

    

    public void StartMinigame()
    {
        //if ingredient is in inventory
         if(objectiveManager.ingredientGrabbed)
         { 
            currentIngredient = objectiveManager.currentGrabbedIngredient;
            
            if(!doughRollingCompleted && currentIngredient != IngrdientDough)
            {
                //pop up (Dough Needed)
                StartCoroutine(playerInteraction.PopUpText(2, "First comes the dough"));
                return;
            }
            //Check Which ingredient is grabbed
            

            objectiveManager.ingredientGrabbed = false;
            Destroy(itemHolder.GetChild(0).gameObject);
            
            //camera switch
            cuttingCam.SetActive(true);
            player.SetActive(false);

            //Skillcheck button randomizer
            
            //Skillcheck spawn location

            //Skillcheck button pop-up
         }
         else
         {
             //pop up (no ingredient grabbed)
         }
    }
}
