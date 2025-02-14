using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] LayerMask layerMask;
    [SerializeField] int rayDistance;
    RaycastHit hit;

    void Update()
    {
        Interaction();
    }

    void Interaction()
    {
        if(Physics.Raycast(transform.position, transform.forward, out hit, rayDistance, layerMask))
        {
            //activate txt

            if(Input.GetButtonDown("Fire1"))
            {
                hit.transform.GetComponent<InteractionManager>().Interact.Invoke();
            }
        }
        else
        {
            //deactivate txt
        }
    }
}
