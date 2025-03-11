using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuttingBoardInteraction : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] GameObject cuttingCam;

    int skillcheckAmount;

    public void StartMinigame()
    {
        //if ingredient is in inventory
        // if()
        // {   
            skillcheckAmount = Random.Range(5, 9);
            //camera switch
            cuttingCam.SetActive(true);
            player.SetActive(false);

            //Skillcheck button randomizer
            
            //Skillcheck spawn location

            //Skillcheck button pop-up
        // }
        // else
        // {
        //     //pop up (no ingredient grabbed)
        // }
    }
}
