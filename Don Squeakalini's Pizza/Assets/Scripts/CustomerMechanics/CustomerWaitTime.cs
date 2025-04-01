using UnityEngine;
using System.Collections;

public class CustomerWaitTime : MonoBehaviour
{
    CustomerInteraction customerInteraction;
    Animator animator;


    void Start()
    {
        customerInteraction = GetComponent<CustomerInteraction>();
        animator = GetComponent<Animator>();
    }

    public IEnumerator WaitTime()
    {
        while(!customerInteraction.PizzaRecieved)
        {
            yield return new WaitForSeconds(Random.Range(15, 40));
            
            animator.SetBool("Idle(Sniff)", true);

            yield return new WaitForSeconds(4.14f);

            animator.SetBool("Idle(Sniff)", false);
        }
        StopCoroutine(WaitTime());
    }
}