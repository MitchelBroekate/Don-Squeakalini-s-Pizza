using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerOrderObjective : MonoBehaviour
{
    [SerializeField] List<IngredientSO> chosenIngredients = new();

    ObjectiveManager objectiveManager;

    int correctOrder;

    void Start()
    {
        objectiveManager = GetComponent<ObjectiveManager>();
    }

    public void Saus()
    {
        chosenIngredients.Add(objectiveManager.ingredients[0]);
    }
    public void Cheese()
    {
        chosenIngredients.Add(objectiveManager.ingredients[1]);
    }


    public void Pepperoni()
    {
        chosenIngredients.Add(objectiveManager.ingredients[2]);
    }

    public void Paprika()
    {
        chosenIngredients.Add(objectiveManager.ingredients[3]);
    }

    public void SendOrder()
    {
        if(objectiveManager.currentObjectiveIngredients.Count == chosenIngredients.Count)
        {
            int ingredientCheck = 0;

            foreach(IngredientSO ingredient in chosenIngredients)
            {
                if(objectiveManager.currentObjectiveIngredients[ingredientCheck] != ingredient)
                {
                    correctOrder--;
                }
                else
                {
                    correctOrder++;
                }

                ingredientCheck++;
            }
        }
        else
        {
            int objectiveIngredientAmount = objectiveManager.currentObjectiveIngredients.Count;
            int chosenIngredientsAmount = chosenIngredients.Count;

            if(objectiveIngredientAmount - chosenIngredientsAmount > 0)
            {
                correctOrder -= objectiveIngredientAmount - chosenIngredientsAmount;

                int ingredientCheck = 0;

                foreach(IngredientSO ingredient in chosenIngredients)
                {
                    if(objectiveManager.currentObjectiveIngredients[ingredientCheck] != ingredient)
                    {
                        correctOrder--;
                    }
                    else
                    {
                        correctOrder++;
                    }
                        
                    ingredientCheck++;
                }
            }
            else if(objectiveIngredientAmount - chosenIngredientsAmount < 0)
            {
                correctOrder -= chosenIngredientsAmount - objectiveIngredientAmount;

                int ingredientCheck = 0;

                foreach(var ingredient in objectiveManager.currentObjectiveIngredients)
                {
                    if(objectiveManager.currentObjectiveIngredients[ingredientCheck] != chosenIngredients[ingredientCheck])
                    {
                        correctOrder--;
                    }
                    else
                    {
                        correctOrder++;
                    }
                        
                    ingredientCheck++;
                }
            }


        }

        if(correctOrder < 0)
        {
            //angry customer
        }

        print(correctOrder);
    }
}
