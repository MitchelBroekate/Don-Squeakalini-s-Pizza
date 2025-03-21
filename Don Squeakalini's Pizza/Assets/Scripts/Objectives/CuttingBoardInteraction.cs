using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CuttingBoardInteraction : MonoBehaviour
{
    [SerializeField] List<IngredientSO> ingredientsNeeded = new();

    [SerializeField] ObjectiveManager objectiveManager;
    [SerializeField] PlayerInteraction playerInteraction;

    [SerializeField] GameObject player;
    [SerializeField] GameObject cuttingCam;
    
    [SerializeField] Transform itemHolder;

    [SerializeField] IngredientSO IngrdientDough;
    IngredientSO currentIngredient;

    bool doughRollingCompleted = false;
    bool ingredientsAdded = false;

    [SerializeField] Canvas canvas;
    [SerializeField] GameObject emptyParent;
    [SerializeField] List<GameObject> skillcheckObjects = new();
    [SerializeField] GameObject ingredientTxt;
    
    RectTransform parentRect;
    
    int currentKeybind = 4;
    int currentMinigameCompletion = 0;
    bool minigameCompleted = false;
    float minigameScore = 5;
    

    void Start()
    {
        parentRect = emptyParent.GetComponent<RectTransform>();

        ingredientsNeeded.Add(IngrdientDough);

        ingredientTxt.SetActive(false);
    }

    void Update()
    {
        if(currentMinigameCompletion > 4)
        {
            minigameCompleted = true;

            StartCoroutine(CompleteMinigame());
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

                if(currentMinigameCompletion <5)
                {
                    StartCoroutine(RandomSkillcheckPopUp(1));
                }

                Destroy(emptyParent.transform.GetChild(0).gameObject);
            } 
            else if(currentKeybind < 4 && currentKeybind != 0)
            {
                minigameScore *= 0.75f;
                minigameScore = (float)Math.Round(minigameScore, 2);

                currentMinigameCompletion++;

                if(currentMinigameCompletion <5)
                {
                    StartCoroutine(RandomSkillcheckPopUp(1));
                }

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

                if(currentMinigameCompletion <5)
                {
                    StartCoroutine(RandomSkillcheckPopUp(1));
                }

                Destroy(emptyParent.transform.GetChild(0).gameObject);
            } 
            else if(currentKeybind < 4 && currentKeybind != 1)
            {
                minigameScore *= 0.75f;
                minigameScore = (float)Math.Round(minigameScore, 2);

                currentMinigameCompletion++;

                if(currentMinigameCompletion <5)
                {
                    StartCoroutine(RandomSkillcheckPopUp(1));
                }

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

                if(currentMinigameCompletion <5)
                {
                    StartCoroutine(RandomSkillcheckPopUp(1));
                }

                Destroy(emptyParent.transform.GetChild(0).gameObject);
            } 
            else if(currentKeybind < 4 && currentKeybind != 2)
            {
                minigameScore *= 0.75f;
                minigameScore = (float)Math.Round(minigameScore, 2);

                currentMinigameCompletion++;

                if(currentMinigameCompletion <5)
                {
                    StartCoroutine(RandomSkillcheckPopUp(1));
                }

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

                if(currentMinigameCompletion <5)
                {
                    StartCoroutine(RandomSkillcheckPopUp(1));
                }

                Destroy(emptyParent.transform.GetChild(0).gameObject);
            }
            else if(currentKeybind < 4 && currentKeybind != 3)
            {
                minigameScore *= 0.75f;
                minigameScore = (float)Math.Round(minigameScore, 2);

                currentMinigameCompletion++;

                if(currentMinigameCompletion <5)
                {
                    StartCoroutine(RandomSkillcheckPopUp(1));
                }

                Destroy(emptyParent.transform.GetChild(0).gameObject);
            } 
        }
    }

    public void StartMinigame()
    {
        if(objectiveManager.IngredientToppingsCompleted == true) return;

        //if ingredient is in inventory
         if(objectiveManager.ingredientGrabbed)
         { 
            if(!ingredientsAdded)
            {
                foreach(IngredientSO ingredient in objectiveManager.currentObjectiveIngredients)
                {
                    if(!ingredientsNeeded.Contains(ingredient))
                    {
                        ingredientsNeeded.Add(ingredient);
                    }
                }

                ingredientsAdded = true;
            }


            currentIngredient = objectiveManager.currentGrabbedIngredient;
            
            if(!doughRollingCompleted && currentIngredient != IngrdientDough)
            {
                //pop up (Dough Needed)
                StopAllCoroutines();
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

                if(currentIngredient == IngrdientDough)
                {
                    doughRollingCompleted = true;
                }

                ingredientsNeeded.Remove(currentIngredient);

                //Skillcheck function
                StartCoroutine(RandomSkillcheckPopUp(1));
            }
            else
            {
                //pop up (wrong ingredient grabbed)
                StopAllCoroutines();
                StartCoroutine(playerInteraction.PopUpText(2, "The customer didn't want this on his pizza"));
                return;
            }
         }
         else
         {
             //pop up (no ingredient grabbed)
             StopAllCoroutines();
             StartCoroutine(playerInteraction.PopUpText(2, "I forgot to grab an ingredient"));
         }
    }

    void WaveMoneyMultiplier()
    {
        //will multiply the money depending on how far you are in the game
    }

    IEnumerator RandomSkillcheckPopUp(float waitTime)
    {
        currentKeybind = 4;

        yield return new WaitForSeconds(waitTime);

        currentKeybind = UnityEngine.Random.Range(0, skillcheckObjects.Count);

        float randomX = UnityEngine.Random.Range(-parentRect.rect.width / 2, parentRect.rect.width / 2);
        float randomY = UnityEngine.Random.Range(-parentRect.rect.height / 2, parentRect.rect.height / 2);

        GameObject currentskillcheck = Instantiate(skillcheckObjects[currentKeybind], emptyParent.transform);

        currentskillcheck.transform.localPosition = new Vector3(randomX, randomY, 0);
    }

    IEnumerator CompleteMinigame()
    {
        ingredientTxt.SetActive(true);

        yield return new WaitForSeconds(2);

        ingredientTxt.SetActive(false);

        player.SetActive(true);
        cuttingCam.SetActive(false);

        //remember score 
        WaveMoneyMultiplier();
        objectiveManager.MoneyToAdd(minigameScore);

        if(ingredientsNeeded.Count <= 0)
        {
            objectiveManager.IngredientToppingsCompleted = true;
        }

        minigameScore = 5;
        currentMinigameCompletion = 0;
        ingredientsAdded = false;
        minigameCompleted = false;
    }
}
