using UnityEngine;

public class BoxInteraction : MonoBehaviour
{
    [SerializeField] ObjectiveManager objectiveManager;
    [SerializeField] PlayerInteraction playerInteraction;
    [SerializeField] Transform itemHolder;
    [SerializeField] GameObject pizzaBox;
    public void BoxInteract()
    {
        if(objectiveManager.OvenMinigameCompleted)
        {
            Destroy(itemHolder.GetChild(0).gameObject);
            objectiveManager.PizzaCompleet = true;
            GameObject currentObject = Instantiate(pizzaBox, itemHolder.transform.position, Quaternion.identity);
            currentObject.transform.parent = itemHolder;
            currentObject.layer = LayerMask.NameToLayer("Food");
        }
        else if(objectiveManager.PizzaCompleet)
        {
            StartCoroutine(playerInteraction.PopUpText(2, "I packed the pizza"));
        }
        else
        {
            StartCoroutine(playerInteraction.PopUpText(2, "I need to prepare the pizza first"));
        }
    }

    void Update()
    {
        if(objectiveManager.BoxDestroyer)
        {
            Destroy(itemHolder.GetChild(0).gameObject);
            objectiveManager.BoxDestroyer = false;
        }
    }
}
