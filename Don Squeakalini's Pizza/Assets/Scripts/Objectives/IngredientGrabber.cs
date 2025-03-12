using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngredientGrabber : MonoBehaviour
{
    [SerializeField] Transform itemHolder;
    [SerializeField] IngredientSO ingredientToSpawn;
    [SerializeField] ObjectiveManager objectiveManager;

    public void SpawnIngredient()
    {
        if(objectiveManager.OrderCompleted)
        {
            if (!objectiveManager.itemGrabbed)
            {
                //instantiate, and set itemGrabbed to true to fill 'inventory'
                objectiveManager.itemGrabbed = true;

                GameObject currentIngredient = Instantiate(ingredientToSpawn.ingredientObject, itemHolder.position, Quaternion.identity);
                currentIngredient.transform.parent = itemHolder;

                print("Ingredient Grabbed");
            }
            else
            {
                print("Already carrying an item");
                //UI pop-up
            }
        }
        else
        {
            print("Order not completed");
            //UI pop-up
        }

    }
}
