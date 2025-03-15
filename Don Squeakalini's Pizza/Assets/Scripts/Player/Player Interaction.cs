using System.Collections;
using TMPro;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] LayerMask layerMask;
    [SerializeField] int rayDistance;

    [SerializeField] GameObject textPopUpInteraction;
    [SerializeField] GameObject textObjectivePopUpObject;
    [SerializeField] TMP_Text textObjectivePopUpTxt;
    
    RaycastHit hit;

    void Start()
    {
        //deactivate txt incase active
        textPopUpInteraction.SetActive(false);
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
            textPopUpInteraction.SetActive(true);

            if(Input.GetButtonDown("Fire1"))
            {
                hit.transform.GetComponent<InteractionManager>().Interact.Invoke();
            }
        }
        else
        {
            //deactivate txt
            textPopUpInteraction.SetActive(false);
        }
    }

    public IEnumerator PopUpText(float waitTime, string popUpText)
    {
        //enable pop up and change text
        textObjectivePopUpTxt.text = popUpText;
        textObjectivePopUpObject.SetActive(true);

        yield return new WaitForSeconds(waitTime);

        //disable pop up
        textObjectivePopUpObject.SetActive(false);
    }
}