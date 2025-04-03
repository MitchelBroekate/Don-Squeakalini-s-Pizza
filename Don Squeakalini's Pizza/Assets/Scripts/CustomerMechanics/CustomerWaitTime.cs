using UnityEngine;
using System.Collections;

public class CustomerWaitTime : MonoBehaviour
{
    CustomerInteraction customerInteraction;
    Animator animator;
    float waitingtime;

    void Start()
    {
        customerInteraction = GetComponent<CustomerInteraction>();
        animator = GetComponent<Animator>();
    }

    public IEnumerator WaitTime()
    {
        while(!customerInteraction.PizzaRecieved)
        {
            waitingtime = Random.Range(10, 30);

            if(waitingtime < 20)
            {
                yield return new WaitForSeconds(waitingtime);
                
                animator.SetBool("Idle(Sniff)", true);

                yield return new WaitForSeconds(4.15f);

                animator.SetBool("Idle(Sniff)", false);
            }
            else
            {
                yield return new WaitForSeconds(waitingtime);
                
                animator.SetBool("Grr", true);

                yield return new WaitForSeconds(1.20f);

                animator.SetBool("Grr", false);
            }

        }
    }
}