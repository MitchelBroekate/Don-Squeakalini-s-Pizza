using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
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

    [SerializeField] TMP_Text quotaText;
    [SerializeField] TMP_Text moneyText;

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
            //set value
            quotaMoney = 100;
            customerAmount = 4;

            //quota amount UI update
            quotaText.text = quotaMoney.ToString();
        }
        else
        {
            //increase value based on finished quota's
            quotaMoney = 100 * quotasCompleted / 1.5f;
            quotaMoney = (float)Math.Round(quotaMoney);

            customerAmount = 4 * quotasCompleted / 2;

            //quota amount UI update
            quotaText.text = quotaMoney.ToString();
        }
    }

    public void QuotaCompleted()
    {
        QuotaScreenDisable();
    }

    public void CustomerCompletion()
    {
        if(customersCompleted >= customerAmount)
        {
            //activate Quota screen
            QuotaScreenActivate();
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
            quotaScreen.SetActive(true);

            pizzariaController.LockPlayer(true);

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            QuotaCompleted();
        }
        else
        {
            //activate lose screen
            quotaScreen.SetActive(false);
            loseScreen.SetActive(true);

            pizzariaController.LockPlayer(true);

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
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

    public void AddmoneyToQuota()
    {
        moneyEarned += moneyToAdd;
        moneyEarned = (float)Math.Round(moneyEarned, 2);
        //UI quota update
        moneyText.text = moneyEarned.ToString();
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

    public void SplitValue()
    {
        ingredientSplit++;
    }

    public int IngredientSplit
    {
        get { return ingredientSplit;}
    }

    IEnumerator QuotaScreenActivate()
    {
        yield return new WaitForSeconds(5);

        CheckMoneyForQuota();
    }

    IEnumerator QuotaScreenDisable()
    {
        yield return new WaitForSeconds(5);
        
        quotasCompleted++;

        CreateQuota();

        CheckMoneyForQuota();

        quotaScreen.SetActive(false);

        pizzariaController.LockPlayer(false);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

    }
}