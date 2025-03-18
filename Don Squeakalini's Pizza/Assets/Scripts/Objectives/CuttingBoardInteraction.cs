using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

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
    
    RectTransform parentRect;
    
    int currentKeybind = 4;
    int currentMinigameCompletion = 0;
    bool minigameCompleted = false;
    float minigameScore = 1;

    void Start()
    {
        parentRect = emptyParent.GetComponent<RectTransform>();

        foreach (IngredientSO ingredient in objectiveManager.currentObjectiveIngredients)
        {
            if(!ingredientsNeeded.Contains(ingredient))
            {
                ingredientsNeeded.Add(ingredient);
            }
        }

        ingredientsNeeded.Add(IngrdientDough);
    }

    void Update()
    {
        if(currentMinigameCompletion > 4)
        {
            minigameCompleted = true;
        }
    }

    public void OnW(InputAction.CallbackContext context)
    {
        if(context.performed)
        {   
            if(minigameCompleted) return;

            if(currentKeybind == 0)
            {

                currentMinigameCompletion++;
                StartCoroutine(RandomSkillcheckPopUp(1));
                print("Checked The Skill :P");

                Destroy(emptyParent.transform.GetChild(0).gameObject);
            } 
            else if(currentKeybind < 4 && currentKeybind != 0)
            {
                //minus points
                minigameScore *= 0.75f;
                minigameScore = (float)Math.Round(minigameScore, 2);

                print("Wrong order");
                print(minigameScore);

                currentMinigameCompletion++;
                StartCoroutine(RandomSkillcheckPopUp(1));

                Destroy(emptyParent.transform.GetChild(0).gameObject);
            } 
        }
    }

    public void OnA(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            if(minigameCompleted) return;

            if(currentKeybind == 1)
            {
                minigameScore++;
                currentMinigameCompletion++;
                StartCoroutine(RandomSkillcheckPopUp(1));

                print("Checked The Skill :P");

                Destroy(emptyParent.transform.GetChild(0).gameObject);
            } 
            else if(currentKeybind < 4 && currentKeybind != 1)
            {
                minigameScore *= 0.75f;
                minigameScore = (float)Math.Round(minigameScore, 2);

                print("Wrong order");
                print(minigameScore);

                currentMinigameCompletion++;
                StartCoroutine(RandomSkillcheckPopUp(1));

                Destroy(emptyParent.transform.GetChild(0).gameObject);
            } 
        }
    }

    public void OnS(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            if(minigameCompleted) return;

            if(currentKeybind == 2)
            {
                minigameScore++;
                currentMinigameCompletion++;
                StartCoroutine(RandomSkillcheckPopUp(1));

                print("Checked The Skill :P");

                Destroy(emptyParent.transform.GetChild(0).gameObject);
            } 
            else if(currentKeybind < 4 && currentKeybind != 2)
            {
                minigameScore *= 0.75f;
                minigameScore = (float)Math.Round(minigameScore, 2);

                print("Wrong order");
                print(minigameScore);

                currentMinigameCompletion++;
                StartCoroutine(RandomSkillcheckPopUp(1));

                Destroy(emptyParent.transform.GetChild(0).gameObject);
            } 
        }
    }

    public void OnD(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            if(minigameCompleted) return;

            if(currentKeybind == 3)
            {
                minigameScore++;
                currentMinigameCompletion++;
                StartCoroutine(RandomSkillcheckPopUp(1));

                print("Checked The Skill :P");

                Destroy(emptyParent.transform.GetChild(0).gameObject);
            }
            else if(currentKeybind < 4 && currentKeybind != 3)
            {
                minigameScore *= 0.75f;
                minigameScore = (float)Math.Round(minigameScore, 2);

                print("Wrong order");
                print(minigameScore);

                currentMinigameCompletion++;
                StartCoroutine(RandomSkillcheckPopUp(1));

                Destroy(emptyParent.transform.GetChild(0).gameObject);
            } 
        }
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

                //Skillcheck function
                StartCoroutine(RandomSkillcheckPopUp(1));
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
             StartCoroutine(playerInteraction.PopUpText(2, "I forgot to grab an ingredient"));
         }
    }


    IEnumerator RandomSkillcheckPopUp(float waitTime)
    {
        currentKeybind = 4;

        yield return new WaitForSeconds(waitTime);

        currentKeybind = UnityEngine.Random.Range(0, skillcheckTxt.Count);

        float randomX = UnityEngine.Random.Range(-parentRect.rect.width / 2, parentRect.rect.width / 2);
        float randomY = UnityEngine.Random.Range(-parentRect.rect.height / 2, parentRect.rect.height / 2);

        TextMeshProUGUI currentText = Instantiate(skillcheckTxt[currentKeybind], emptyParent.transform);

        currentText.rectTransform.localPosition = new Vector3(randomX, randomY, 0);
    }
}
