using TMPro;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] LayerMask layerMask;
    [SerializeField] int rayDistance;

    [SerializeField] GameObject textPopUp;
    
    RaycastHit hit;

    void Start()
    {
        //deactivate txt incase active
        textPopUp.SetActive(false);
    }

    void Update()
    {
        Interaction();
    }

    void Interaction()
    {
        if(Physics.Raycast(transform.position, transform.forward, out hit, rayDistance, layerMask))
        {
            //activate txt
            textPopUp.SetActive(true);

            if(Input.GetButtonDown("Fire1"))
            {
                hit.transform.GetComponent<InteractionManager>().Interact.Invoke();
            }
        }
        else
        {
            //deactivate txt
            textPopUp.SetActive(false);
        }
    }
}
