using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CustomerManager : MonoBehaviour
{
    //random customer spawner (not previous customer)
    [SerializeField] GameObject[] mousePrefabs;
    [SerializeField] Transform spawnpoints;
    [SerializeField] Transform customerParent;

    ObjectiveManager objectiveManager;
    int _previousCustomer;

    int ingredientAmount;

    public List<IngredientSO> ingredientsToAdd;
    

    void Start()
    {
        objectiveManager = GetComponent<ObjectiveManager>();
    }

    public void CustomerSpawner()
    {
        int customerToSpawn = Random.Range(0, mousePrefabs.Length);
        int randomSpawn = Random.Range(0, spawnpoints.childCount);

        while (customerToSpawn == _previousCustomer)
        {
            customerToSpawn = Random.Range(0, mousePrefabs.Length);
        }

        _previousCustomer = customerToSpawn;    

        GameObject customer = Instantiate(mousePrefabs[customerToSpawn], spawnpoints.GetChild(randomSpawn).transform.position, Quaternion.identity);
        customer.transform.parent = customerParent;

    }

    //amount of indgredient
    void IngredientPicker()
    {
        if(objectiveManager.introCompleet)
        {
            ingredientAmount = Random.Range(0, objectiveManager.ingredients.Count);

            for(int i = 0; i < ingredientAmount; i++)
            {
                IngredientSO scriptableObject = objectiveManager.ingredients[Random.Range(0, objectiveManager.ingredients.Count)];

                while(ingredientsToAdd.Contains(scriptableObject))
                {
                    scriptableObject = objectiveManager.ingredients[Random.Range(0, objectiveManager.ingredients.Count)];
                }

                ingredientsToAdd.Add(scriptableObject);
            }
        }
        else
        {
            ingredientAmount = 2;

            ingredientsToAdd.Add(objectiveManager.ingredients[0]);
            ingredientsToAdd.Add(objectiveManager.ingredients[1]);

        }
    }

    
}
