using System.Collections.Generic;
using UnityEngine;

public class ObjectiveManager : MonoBehaviour
{
    //spawn customer when intro is done
    public bool introCompleet = false;
    bool pizzaCompleet = false;
    public List<IngredientSO> ingredients = new();

    [SerializeField] CustomerManager customerManager;

    void Start()
    {
        IntroObjectives();  
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.C))
        {
            pizzaCompleet = true;

            print("button pressed");
        }
    }

    void IntroObjectives()
    {
        //Objectives for the intro
        customerManager.CustomerSpawner();
        introCompleet = true;
    }

    void CustomerCompletion()
    {
            //Objectives for making the pizza
            
            //Wait for customer to leave
            //Kill 'em

            //Spawn new customer
    }

    public bool PizzaCompleet
    {
        get { return pizzaCompleet;}
        set { pizzaCompleet = value;}
    }
}
