using System.Collections.Generic;
using UnityEngine;

public class CustomerManager : MonoBehaviour
{
    //random customer spawner (not previous customer)
    [SerializeField] GameObject[] mousePrefabs;
    [SerializeField] Transform spawnpoints;
    [SerializeField] Transform customerParent;
    int _previousCustomer;

    public List<IngredientSO> ingredientsToAdd;

    ObjectiveManager objectiveManager;

    int ingredientAmount;
    

    void Start()
    {
        objectiveManager = GetComponent<ObjectiveManager>();
    }

    public void CustomerSpawner()
    {
        ingredientsToAdd.Clear();

        IngredientPicker();

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
    public void IngredientPicker()
    {
        if(objectiveManager.introCompleet)
        {
            print("adding pizza part 2");

            ingredientAmount = Random.Range(1, objectiveManager.ingredients.Count);

            for(int i = 0; i < ingredientAmount; i++)
            {
                IngredientSO scriptableObject = objectiveManager.ingredients[Random.Range(0, objectiveManager.ingredients.Count)];

                while(ingredientsToAdd.Contains(scriptableObject))
                {
                    scriptableObject = objectiveManager.ingredients[Random.Range(0, objectiveManager.ingredients.Count)];
                }

                ingredientsToAdd.Add(scriptableObject);
                
                objectiveManager.currentObjectiveIngredients.Add(scriptableObject);
            }
        }
        else
        {
            ingredientAmount = 2;

            print("adding pizza");    

            ingredientsToAdd.Add(objectiveManager.ingredients[0]);
            ingredientsToAdd.Add(objectiveManager.ingredients[1]);

            objectiveManager.currentObjectiveIngredients.Add(objectiveManager.ingredients[0]);
            objectiveManager.currentObjectiveIngredients.Add(objectiveManager.ingredients[1]);

        }
    }

    
}
