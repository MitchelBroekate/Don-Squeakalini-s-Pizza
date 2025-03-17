using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using TMPro;
using UnityEngine;

public class CuttingBoardInteraction : MonoBehaviour
{
    List<IngredientSO> ingredientsNeeded = new();

    [SerializeField] ObjectiveManager objectiveManager;
    [SerializeField] PlayerInteraction playerInteraction;

    [SerializeField] GameObject player;
    [SerializeField] GameObject cuttingCam;
    
    [SerializeField] Transform itemHolder;

    [SerializeField] IngredientSO IngrdientDough;
    IngredientSO currentIngredient;

    bool doughRollingCompleted = false;

    [SerializeField] Canvas canvas;
    [SerializeField] GameObject emptyParent;
    [SerializeField] List<TextMeshProUGUI>  skillcheckTxt = new();

    int currentKeybind = 5;
    int currentMinigameScore = 0;

    void Start()
    {
        foreach (IngredientSO ingredient in objectiveManager.currentObjectiveIngredients)
        {
            if(!ingredientsNeeded.Contains(ingredient))
            {
                ingredientsNeeded.Add(ingredient);
            }
        }

        ingredientsNeeded.Add(IngrdientDough);
    }

    

    public void StartMinigame()
    {
        //if ingredient is in inventory
         if(objectiveManager.ingredientGrabbed)
         { 
            currentIngredient = objectiveManager.currentGrabbedIngredient;
            
            if(!doughRollingCompleted && currentIngredient != IngrdientDough)
            {
                //pop up (Dough Needed)
                StartCoroutine(playerInteraction.PopUpText(2, "First comes the dough"));
                return;
            }

            if(ingredientsNeeded.Contains(currentIngredient))
            {
                objectiveManager.ingredientGrabbed = false;
                Destroy(itemHolder.GetChild(0).gameObject);
                
                //camera switch
                cuttingCam.SetActive(true);
                player.SetActive(false);

                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;

                //Skillcheck spawn location
                //Skillcheck button randomizer
                //Skillcheck button pop-up

                RandomSkillcheckPopUp();
            }
            else
            {
                //pop up (wrong ingredient grabbed)
                StartCoroutine(playerInteraction.PopUpText(2, "The customer didn't want this on his pizza"));
                return;
            }
         }
         else
         {
             //pop up (no ingredient grabbed)
             StartCoroutine(playerInteraction.PopUpText(2, "I forgot the ingredient"));
         }
    }

    void RandomSkillcheckPopUp()
    {
        currentKeybind = Random.Range(0, skillcheckTxt.Count);

        RectTransform parentRect = emptyParent.GetComponent<RectTransform>();

        float randomX = Random.Range(-parentRect.rect.width / 2, parentRect.rect.width / 2);
        float randomY = Random.Range(-parentRect.rect.height / 2, parentRect.rect.height / 2);

        TextMeshProUGUI newText = Instantiate(skillcheckTxt[currentKeybind], emptyParent.transform);

        newText.rectTransform.localPosition = new Vector3(randomX, randomY, 0);
    }

    IEnumerator skillcheckWaitTime()
    {
        yield return new WaitForSeconds(0);
    }
}
