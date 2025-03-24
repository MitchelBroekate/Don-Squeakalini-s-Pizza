using UnityEngine;

public class PizzaInteraction : MonoBehaviour
{
    public ObjectiveManager objectiveManager;
    public Transform itemHolder;
    public bool grabbedPizza = false;

    public void GrabPizza()
    {
        objectiveManager.ingredientGrabbed = true;
        objectiveManager.PizzaGrabbed = true;

        GetComponent<BoxCollider>().enabled = false;

        transform.parent = itemHolder;
        transform.position = itemHolder.position;

        grabbedPizza = true;

        gameObject.layer = LayerMask.NameToLayer("Food");

        for(int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.layer = LayerMask.NameToLayer("Food");
        }
    }
}
