using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CustomerOrderObjective : MonoBehaviour
{
    ObjectiveManager objectiveManager;

    int correctOrder = 0;
    float moneyMultiplier = 1;

    bool addedCheese = false, addedSaus = false, addedPaprika = false, addedPepperoni = false;

    void Start()
    {
        objectiveManager = GetComponent<ObjectiveManager>();
    }

    public void Saus()
    {
        if(!addedSaus)
        {
            addedSaus = true;
            if(objectiveManager.currentObjectiveIngredients.Contains(objectiveManager.ingredients[0]))
            {
                correctOrder++;
            }
            else
            {
                correctOrder--;
            }
        }
    }
    public void Cheese()
    {
        if(!addedCheese)
        {
            addedCheese = true;
            if(objectiveManager.currentObjectiveIngredients.Contains(objectiveManager.ingredients[1]))
            {
                correctOrder++;
            }
            else
            {
                correctOrder--;
            }
        }
    }


    public void Pepperoni()
    {
        if(!addedPepperoni)
        {
            addedPepperoni = true;
            if(objectiveManager.currentObjectiveIngredients.Contains(objectiveManager.ingredients[2]))
            {
                correctOrder++;
            }
            else
            {
                correctOrder--;
            }
        }
    }

    public void Paprika()
    {
        if(!addedPaprika)
        {
            addedPaprika = true;
            if(objectiveManager.currentObjectiveIngredients.Contains(objectiveManager.ingredients[3]))
            {
                correctOrder++;
            }
            else
            {
                correctOrder--;
            }
        }
    }

    public void SendOrder()
    {

        if(correctOrder == objectiveManager.currentObjectiveIngredients.Count)
        {
            objectiveManager.OrderCompletion();

            print("Order Compleet");

            //correct order sfx
        }
        else
        {
            addedCheese = false;
            addedPaprika = false;
            addedPepperoni = false;
            addedSaus = false;

            correctOrder = 0;

            moneyMultiplier *= 0.75f;
            moneyMultiplier = (float)Math.Round(moneyMultiplier, 2);

            print("Wrong order");
            print(moneyMultiplier);

            //failed order sfx
        }
    }
}
