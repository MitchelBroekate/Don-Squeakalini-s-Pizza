using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class CustomerOrderObjective : MonoBehaviour
{
    ObjectiveManager objectiveManager;

    int correctOrder = 0;
    float moneyMultiplier = 1;
    float moneyToEarn = 5;
    bool perfectExecution = true;

    bool addedCheese = false, addedSaus = false, addedPaprika = false, addedPepperoni = false;

    public AudioSource notification;
    public AudioClip negativeNotif;
    public AudioClip positiveNotif;

    void Start()
    {
        objectiveManager = GetComponent<ObjectiveManager>();
    }

    public void Saus()
    {
        if(!addedSaus && !objectiveManager.OrderCompleted)
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
        if(!addedCheese && !objectiveManager.OrderCompleted)
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
        if(!addedPepperoni && !objectiveManager.OrderCompleted)
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
        if(!addedPaprika && !objectiveManager.OrderCompleted)
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
        if(objectiveManager.OrderCompleted) return;

        if(correctOrder == objectiveManager.currentObjectiveIngredients.Count)
        {
            objectiveManager.OrderCompleted = true;

            addedCheese = false;
            addedPaprika = false;
            addedPepperoni = false;
            addedSaus = false;

            correctOrder = 0;

            if(perfectExecution)
            {
                moneyMultiplier = 1.5f;
                moneyMultiplier = (float)Math.Round(moneyMultiplier, 2);
                moneyToEarn *= moneyMultiplier;
            }

            print(moneyToEarn + "Order");

            objectiveManager.MoneyToAdd(moneyToEarn);

            moneyMultiplier = 1;
            moneyToEarn = 5;

            notification.clip = positiveNotif;
            notification.Play();

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
            moneyToEarn *= moneyMultiplier;
            
            perfectExecution = false;

            print("Wrong order" + " " + moneyMultiplier);

            notification.clip = negativeNotif;
            notification.Play();

        }
    }
}
