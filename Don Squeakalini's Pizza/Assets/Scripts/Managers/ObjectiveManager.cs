using System.Collections.Generic;
using UnityEngine;

public class ObjectiveManager : MonoBehaviour
{
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

    float moneyCuttingDough = 0;
    float moneyCuttingSauce = 0;
    float moneyCuttingPaprika = 0;
    float moneyCuttingPepperoni = 0;
    float moneyCuttingCheese = 0;
    int ingredientSplit = 0;

    public IngredientSO currentGrabbedIngredient;

    public List<IngredientSO> ingredients = new();
    public List<IngredientSO> currentObjectiveIngredients = new();

    [SerializeField] CustomerManager customerManager;
    [SerializeField] PizzariaController pizzariaController;

    float quotaMoney = 0;
    public int quotasCompleted = 1;
    int customerAmount = 0;
    public int customersCompleted = 0;

    [SerializeField] GameObject quotaScreen;
    [SerializeField] GameObject loseScreen;

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
        if(quotasCompleted < 2)
        {
            //set quota value
            quotaMoney = 100;
            customerAmount = 4;
        }
        else
        {
            //increase quota value based on finished quota's
            quotaMoney = 100 * quotasCompleted / 1.5f;
            customerAmount = 4 * quotasCompleted / 2;
        }
    }

    public void QuotaCompleted()
    {
        quotasCompleted++;

        CreateQuota();
    }

    public void CustomerCompletion()
    {
        if(customersCompleted >= customerAmount)
        {
            //activate Quota screen
            quotaScreen.SetActive(true);

            pizzariaController.LockPlayer(true);
        }
        else
        {
            customersCompleted++;
        }
    }

    void CheckMoneyForQuota()
    {
        if(moneyEarned > quotaMoney)
        {
            //disable quota screen and resume game
            quotaScreen.SetActive(false);
        }
        else
        {
            //activate lose screen
            quotaScreen.SetActive(false);
            loseScreen.SetActive(true);
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

    public void AddIngredientMoney()
    {
        float ingredientMoney = moneyCuttingPepperoni + moneyCuttingSauce + moneyCuttingPaprika + moneyCuttingSauce;
        ingredientMoney /= ingredientSplit;

        MoneyToAdd(ingredientMoney);
    }

    public float DoughMoney(float value)
    {
        moneyCuttingDough += value;
        return moneyCuttingDough;
    }

    public float SauceMoney(float value)
    {
        moneyCuttingSauce += value;
        return moneyCuttingSauce;
    }

    public float PaprikaMoney(float value)
    {
        moneyCuttingPaprika += value;
        return moneyCuttingPaprika;
    }

    public float PepperoniMoney(float value)
    {
        moneyCuttingPepperoni += value;
        return moneyCuttingPepperoni;
    }

    public float CheeseMoney(float value)
    {
        moneyCuttingCheese += value;
        return moneyCuttingCheese;
    }

    public int SplitValue
    {
        get{return ingredientSplit;}
        set{ingredientSplit += value;}
    }
}