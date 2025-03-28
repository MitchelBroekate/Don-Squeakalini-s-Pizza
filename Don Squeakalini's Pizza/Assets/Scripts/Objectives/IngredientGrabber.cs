using Unity.Mathematics;
using UnityEngine;

public class IngredientGrabber : MonoBehaviour
{
    [SerializeField] Transform itemHolder;
    [SerializeField] IngredientSO ingredientToSpawn;
    [SerializeField] ObjectiveManager objectiveManager;
    [SerializeField] PlayerInteraction playerInteraction;
    [SerializeField] IngredientSO sauceSpoon;

    public void SpawnIngredient()
    {
        if(objectiveManager.OrderCompleted)
        {
            if (!objectiveManager.ingredientGrabbed)
            {
                //instantiate, and set itemGrabbed to true to fill 'inventory'
                objectiveManager.ingredientGrabbed = true;

                if(ingredientToSpawn == sauceSpoon)
                {
                    GameObject currentIngredient = Instantiate(ingredientToSpawn.ingredientObject, itemHolder.position, itemHolder.rotation);
                    currentIngredient.transform.parent = itemHolder;

                    currentIngredient.transform.Rotate(0, -100, 0);
                }
                else
                {
                    GameObject currentIngredient = Instantiate(ingredientToSpawn.ingredientObject, itemHolder.position, itemHolder.rotation);
                    currentIngredient.transform.parent = itemHolder;
                }

                objectiveManager.currentGrabbedIngredient = ingredientToSpawn;
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
                    StopAllCoroutines();
                    StartCoroutine(playerInteraction.PopUpText(2, "Got to put this back first"));
                }
            }
        }
        else
        {
            //UI pop-up
            StopAllCoroutines();
            StartCoroutine(playerInteraction.PopUpText(2, "I haven't fulled in the order yet"));
        }

    }
}
