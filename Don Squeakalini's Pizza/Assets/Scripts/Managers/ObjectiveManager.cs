using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

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

    float quotaMoney = 0;
    public int quotaAmount = 1;

    void Start()
    {
        IntroObjectives();  
        CreateQuota();
    }

    void IntroObjectives()
    {
        //Objectives for the intro
        customerManager.CustomerSpawner();
        introCompleet = true;
    }

    public void NewCustomerStates()
    {
        currentObjectiveIngredients.Clear();

        PizzaCompleet = false;
        OrderCompleted = false;
        IngredientToppingsCompleted = false;
        OvenMinigameCompleted = false;
        PizzaGrabbed = false;
        ingredientGrabbed = false;
    }

    void CreateQuota()
    {
        if(quotaAmount < 2)
        {
            //set quota value
            quotaMoney = 100;
        }
        else
        {
            //increase quota value based on finished quota's
            quotaMoney = 100 * quotaAmount / 1.5f;
        }
    }

    public void QuotaCompleted()
    {
        quotaAmount++;

        CreateQuota();
    }

    void CheckMoneyForQuota()
    {
        if(moneyEarned > quotaMoney)
        {

        }
        else
        {

        }
    }

    public void ChangeLayer(GameObject obj, int newLayer)
    {
        obj.layer = newLayer;

        foreach (Transform child in obj.transform)
        {
            ChangeLayer(child.gameObject, newLayer);
        }
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