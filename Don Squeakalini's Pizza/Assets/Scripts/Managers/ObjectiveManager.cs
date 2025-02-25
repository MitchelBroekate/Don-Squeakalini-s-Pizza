using UnityEngine;

public class ObjectiveManager : MonoBehaviour
{
    //spawn customer when intro is done

    [SerializeField] CustomerManager customerManager;
    bool spawnFirstCustomerOnce = true;
    bool introCompleet = false;

    void Update()
    {
        if (spawnFirstCustomerOnce)
        {
            IntroObjectives();
        }
    }

    void IntroObjectives()
    {

        //Objectives for the intro

        introCompleet = true;

        if(introCompleet)
        {
            if(Input.GetKeyDown(KeyCode.K))
            {
                //Spawns the first customer
                print("Pressed");
                spawnFirstCustomerOnce = false;
                customerManager.CustomerSpawner();
            }
        }

    }

    void CustomerCompletion()
    {
        if(introCompleet)
        {
            //Objectives for making the pizza


            //Wait for customer to leave
            //Kill 'em

            //Spawn new customer
        }

    }
}
