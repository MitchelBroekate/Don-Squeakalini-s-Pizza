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
    [SerializeField] PizzariaController pizzariaController;

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
    
    GameObject pizzaBuild;
    bool instantiatePizzaOnce = true;

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

        if(pizzaBuild != null)
        {
            if(pizzaBuild.GetComponent<PizzaInteraction>().grabbedPizza)
            {
                gameObject.layer = LayerMask.NameToLayer("Interactable");
            }
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

        if(objectiveManager.IngredientToppingsCompleted == true)
        {    
            StartCoroutine(playerInteraction.PopUpText(2, "The Pizza is already completed"));
            return;
        }

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
                pizzariaController.LockPlayer(true);
                cuttingCam.SetActive(true);

                for(int i = 0; i < player.transform.childCount; i++)
                {
                    player.transform.GetChild(i).gameObject.SetActive(false);
                }

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

    void PizzaBuilder()
    {
        switch(currentIngredient.ingredientID)
        {
            case 0:
                //Dough
                if(instantiatePizzaOnce)
                {
                    pizzaBuild = Instantiate(currentIngredient.finalObject);

                    //pizzaBuild.transform.parent = transform.GetChild(0);
                    pizzaBuild.transform.position = transform.GetChild(0).position;

                    pizzaBuild.GetComponent<PizzaInteraction>().objectiveManager = objectiveManager;
                    pizzaBuild.GetComponent<PizzaInteraction>().itemHolder = itemHolder;

                    instantiatePizzaOnce = false;
                }

                break;

            case 1:
                //Cheese
                pizzaBuild.transform.GetChild(0).gameObject.SetActive(true);
                break;

            case 2:
                //Paprika
                pizzaBuild.transform.GetChild(1).gameObject.SetActive(true);
                break;

            case 3:
                //Pepperoni
                pizzaBuild.transform.GetChild(2).gameObject.SetActive(true);
                break;

            case 4:
                //TomatoPaste
                pizzaBuild.transform.GetChild(3).gameObject.SetActive(true);
                break;

            default:
                Debug.LogWarning("Int out of bounds. CuttingBouardInteraction/PizzaBuilder");
                break;
        }
    }   

    public void ResetPizzaMakingStates()
    {
        instantiatePizzaOnce = true;
        doughRollingCompleted = false;

        ingredientsNeeded.Add(IngrdientDough);
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

        PizzaBuilder();

        yield return new WaitForSeconds(2);

        ingredientTxt.SetActive(false);

        for(int i = 0; i < player.transform.childCount; i++)
        {
            player.transform.GetChild(i).gameObject.SetActive(true);
        }
        cuttingCam.SetActive(false);
        pizzariaController.LockPlayer(false);
        
        //remember score 
        objectiveManager.MoneyToAdd(minigameScore);

        if(ingredientsNeeded.Count <= 0)
        {
            objectiveManager.IngredientToppingsCompleted = true;

            pizzaBuild.layer = LayerMask.NameToLayer("Interactable");

            gameObject.layer = LayerMask.NameToLayer("Default");

            ingredientsAdded = false;
        }

        minigameScore = 5;
        currentMinigameCompletion = 0;

        minigameCompleted = false;

        StopAllCoroutines();
    }
}
