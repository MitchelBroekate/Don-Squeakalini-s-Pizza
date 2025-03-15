using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngredientGrabber : MonoBehaviour
{
    [SerializeField] Transform itemHolder;
    [SerializeField] IngredientSO ingredientToSpawn;
    [SerializeField] ObjectiveManager objectiveManager;
    [SerializeField] PlayerInteraction playerInteraction;

    public void SpawnIngredient()
    {
        if(objectiveManager.OrderCompleted)
        {
            if (!objectiveManager.ingredientGrabbed)
            {
                //instantiate, and set itemGrabbed to true to fill 'inventory'
                objectiveManager.ingredientGrabbed = true;

                GameObject currentIngredient = Instantiate(ingredientToSpawn.ingredientObject, itemHolder.position, Quaternion.identity);
                currentIngredient.transform.parent = itemHolder;

                objectiveManager.currentGrabbedIngredient = ingredientToSpawn;

                print("Ingredient Grabbed");
            }
            else
            {
                if(objectiveManager.currentGrabbedIngredient == ingredientToSpawn)
                {
                    objectiveManager.currentGrabbedIngredient = null;
                    objectiveManager.ingredientGrabbed = false;

                    Destroy(itemHolder.GetChild(0).gameObject);
                }
                else
                {
                    //UI pop-up
                    StartCoroutine(playerInteraction.PopUpText(2, "Got to put this back first"));
                }
            }
        }
        else
        {
            //UI pop-up
            StartCoroutine(playerInteraction.PopUpText(2, "I haven't fulled in the order yet"));
        }

    }
}
