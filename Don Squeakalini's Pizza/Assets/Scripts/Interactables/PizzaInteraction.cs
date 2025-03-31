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

        objectiveManager.ChangeLayer(gameObject, 9);
    }
}
