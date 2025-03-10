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

    [SerializeField] int imageCount;

    bool canFade = true;
    bool pizzaRecieved = false;

    void Start()
    {
        canvas = transform.GetChild(0).transform;
        customerManager = GameObject.Find("Script Managers").GetComponent<CustomerManager>();
        objectiveManager = GameObject.Find("Script Managers").GetComponent<ObjectiveManager>();
        customerWalkBehavior = GetComponent<CustomerWalkBehavior>();
        pictureParent = canvas.GetChild(0);
        pictureBackground = canvas.GetChild(1);

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

        StopCoroutine(ImageFade());
    }

    public void GivePizza()
    {
        if(objectiveManager.PizzaCompleet)
        {
            pizzaRecieved = true;
            customerWalkBehavior.customerWait = false;
            customerWalkBehavior.currentCheckpoint++;

            transform.GetChild(0).gameObject.SetActive(false);

            customerWalkBehavior.rb.isKinematic = false;
            gameObject.layer = LayerMask.NameToLayer("Customer");

            print("Pizza given");

            objectiveManager.PizzaCompleet = false;
        }
    }

    public bool PizzaRecieved
    {
        get { return pizzaRecieved;}
    }
}
