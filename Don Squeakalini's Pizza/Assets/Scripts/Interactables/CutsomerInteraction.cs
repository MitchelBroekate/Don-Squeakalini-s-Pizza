using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;

public class CustomerInteraction : MonoBehaviour
{
    //customer talks (pop-up)
    Transform canvas;
    List<GameObject> foodIcons = new();

    CanvasGroup canvasAlpha;

    bool alphaIncrease = false;

    List<IngredientSO> ingredients = new();

    CustomerManager customerManager;

    void Start()
    {
        canvas = transform.GetChild(0).transform;
        customerManager = GameObject.Find("Script Managers").GetComponent<CustomerManager>();

        for(int i = 0; i < canvas.childCount; i++)
        {
            foodIcons.Add(canvas.GetChild(i).gameObject);
        }

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
                canvasAlpha.alpha += 0.3f * Time.deltaTime;
            }
        }
    }

    public void StartFade()
    {
        StartCoroutine(ImageFade());
    }

    IEnumerator ImageFade()
    {
        int imageCount = 0;

        foreach(GameObject foodImage in foodIcons)
        {
            alphaIncrease = true;

            SpriteRenderer currentSprite = foodIcons[imageCount].GetComponent<SpriteRenderer>();
            canvasAlpha = foodIcons[imageCount].GetComponent<CanvasGroup>();

            if(ingredients[imageCount] != null)
            {
                currentSprite.sprite = ingredients[imageCount].ingredientSprite;
            }

            currentSprite.enabled = true;

            yield return new WaitForSeconds(0.9f);

            alphaIncrease = false;
   
            imageCount++;
        }
        
        StopCoroutine(ImageFade());

    }

    //icon fade-in
}
