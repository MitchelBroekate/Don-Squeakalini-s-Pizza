using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuttingBoardInteraction : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] GameObject cuttingCam;

    public void StartMinigame()
    {
        //if ingredient is in inventory
        // if()
        // {   
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
