using UnityEngine;
using System.Collections;

public class CustomerWaitTime : MonoBehaviour
{
    CustomerInteraction customerInteraction;

    void Start()
    {
        customerInteraction = GetComponent<CustomerInteraction>();
    }

    public IEnumerator WaitTime()
    {
        while(!customerInteraction.PizzaRecieved)
        {
            yield return new WaitForSeconds(Random.Range(80, 180));

            //Emote
        }
        StopCoroutine(WaitTime());


    }
}