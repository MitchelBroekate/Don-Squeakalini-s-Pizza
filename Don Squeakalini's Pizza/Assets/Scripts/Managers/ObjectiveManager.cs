using System.Collections.Generic;
using UnityEngine;

public class ObjectiveManager : MonoBehaviour
{
    //spawn customer when intro is done
    
    public bool introCompleet = false;
    bool pizzaCompleet = false;
    public bool itemGrabbed = false; 
    bool orderCompleted = false;
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
}
