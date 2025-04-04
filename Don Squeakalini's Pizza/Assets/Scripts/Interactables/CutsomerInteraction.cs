using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerInteraction : MonoBehaviour
{
    //customer talks (pop-up)
    Transform canvas;
    CanvasGroup canvasAlpha;
    bool alphaIncrease = false;

    [SerializeField] List<IngredientSO> ingredients = new();

    Transform pictureParent;
    Transform pictureBackground;

    CustomerManager customerManager;
    ObjectiveManager objectiveManager;
    CustomerWalkBehavior customerWalkBehavior;
    CustomerWaitTime customerWaitTime;

    [SerializeField] int imageCount;

    bool canFade = true;
    bool pizzaRecieved = false;

    float totalMoneyToGive = 0;

    Animator animator;
    [SerializeField] GameObject handBox;
    [SerializeField] GameObject groundBox;

    void Start()
    {
        canvas = transform.GetChild(0).transform;
        customerManager = GameObject.Find("Script Managers").GetComponent<CustomerManager>();
        objectiveManager = GameObject.Find("Script Managers").GetComponent<ObjectiveManager>();
        customerWaitTime = GetComponent<CustomerWaitTime>();
        customerWalkBehavior = GetComponent<CustomerWalkBehavior>();
        pictureParent = canvas.GetChild(0);
        pictureBackground = canvas.GetChild(1);
        animator = GetComponent<Animator>();

        for(int i = 0; i < customerManager.ingredientsToAdd.Count; i++)
        {
            ingredients.Add(customerManager.ingredientsToAdd[i]);
        }
    }

    void Update()
    {
        if(alphaIncrease)
        {
            if(canvasAlpha.alpha < 1)
            {
                canvasAlpha.alpha += 0.03f * Time.deltaTime;
            }
        }
    }

    public void StartFade()
    {
        if(canFade)
        {
            StartCoroutine(ImageFade());
            canFade = false;
        }
    }

    IEnumerator ImageFade()
    {
        pictureBackground.gameObject.SetActive(true);

        yield return new WaitForSeconds(0.9f);

        for(int i = 0; i < pictureParent.childCount; i++) 
        {
            alphaIncrease = true;

            SpriteRenderer currentSprite = pictureParent.GetChild(i).GetComponent<SpriteRenderer>();
            canvasAlpha = pictureParent.GetChild(i).GetComponent<CanvasGroup>();

            if(imageCount < ingredients.Count)
            {
                currentSprite.sprite = ingredients[i].ingredientSprite;
            }

            currentSprite.enabled = true;

            yield return new WaitForSeconds(0.9f);

            alphaIncrease = false;

            imageCount++;
        }

        StartCoroutine(customerWaitTime.WaitTime());
    }

    public void GivePizza()
    {
        if(objectiveManager.PizzaCompleet)
        {
            StartCoroutine(pizzaRecieve());
        }
    }

    public bool PizzaRecieved
    {
        get { return pizzaRecieved;}
    }

    public float TotalMoneyToGive
    {
        get { return totalMoneyToGive;}
        set { totalMoneyToGive = value;}
    }

    IEnumerator pizzaRecieve()
    {
        //handover
        animator.SetBool("HandOverThePizza", true);        
        objectiveManager.BoxDestroyer = true;

        //spawn box in hand
        handBox.SetActive(true);

        //handover wait
        yield return new WaitForSeconds(2);

        //handover emotion
        if(objectiveManager.ReadMoneyAdd < 15)
        {
            //angery
            animator.SetBool("Wrong", true);
            
            yield return new WaitForSeconds(0.10f);

            handBox.SetActive(false);
            groundBox.SetActive(true);

            yield return new WaitForSeconds(5);

            groundBox.SetActive(false);
        }
        else
        {
            //not angery
            animator.SetBool("Correct", true);
            yield return new WaitForSeconds(1.20f);
        }


        //walk away

        pizzaRecieved = true;
        customerWalkBehavior.currentCheckpoint++;
        customerWalkBehavior.customerWait = false;
        customerWalkBehavior.rb.isKinematic = false;
        customerWalkBehavior.lookSwitch = false;


        transform.GetChild(0).gameObject.SetActive(false);

        gameObject.layer = LayerMask.NameToLayer("Customer");



        objectiveManager.AddmoneyToQuota();

        objectiveManager.MoneyToAddZero();

        objectiveManager.CustomerCompletion();
    }
}
