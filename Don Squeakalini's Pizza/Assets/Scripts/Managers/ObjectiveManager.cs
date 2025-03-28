using System.Collections.Generic;
using UnityEngine;

public class ObjectiveManager : MonoBehaviour
{
    //spawn customer when intro is done
    
    public bool introCompleet = false;
    public bool ingredientGrabbed = false; 
    public bool PizzaGrabbed = false;
    bool pizzaCompleet = false;
    bool orderCompleted = false;
    bool ingredientToppingsCompleted = false;
    bool ovenMinigameCompleted = false;
    bool boxDestroyer = false;

    float moneyEarned = 0;

    [SerializeField] float moneyToAdd = 0;

    public IngredientSO currentGrabbedIngredient;

    public List<IngredientSO> ingredients = new();
    public List<IngredientSO> currentObjectiveIngredients = new();

    [SerializeField] CustomerManager customerManager;

    void Start()
    {
        IntroObjectives();  
    }

    void IntroObjectives()
    {
        //Objectives for the intro
        customerManager.CustomerSpawner();
        introCompleet = true;
    }

    public void OrderCompletion()
    {
        //Start making pizza
        orderCompleted = true;
    }

    public bool PizzaCompleet
    {
        get { return pizzaCompleet;}
        set { pizzaCompleet = value;}
    }

    public bool OrderCompleted
    {
        get { return orderCompleted;}
        set { orderCompleted = value;}
    }

    public bool IngredientToppingsCompleted
    {
        get { return ingredientToppingsCompleted;}
        set { ingredientToppingsCompleted = value;}
    }

    public bool OvenMinigameCompleted
    {
        get { return ovenMinigameCompleted;}
        set { ovenMinigameCompleted = value;}
    }

    public bool BoxDestroyer
    {
        get { return boxDestroyer;}
        set { boxDestroyer = value;}
    }

    public float MoneyToAdd(float value)
    {
        moneyToAdd += value;
        return moneyToAdd;
    }
}