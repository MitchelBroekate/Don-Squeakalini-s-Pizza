using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngredientGrabber : MonoBehaviour
{
    [SerializeField] IngredientSO ingredientToSpawn;
    [SerializeField] ObjectiveManager objectiveManager;

    public void SpawnIngredient()
    {
        if (!objectiveManager.itemGrabbed)
        {
            //spawn ingredient, spawn as player child and positioning

            objectiveManager.itemGrabbed = true;
        }
    }
}
