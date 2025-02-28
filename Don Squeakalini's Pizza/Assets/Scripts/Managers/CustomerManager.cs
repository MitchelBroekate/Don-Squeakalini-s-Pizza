using UnityEngine;

public class CustomerManager : MonoBehaviour
{
    //random customer spawner (not previous customer)
    [SerializeField] GameObject[] mousePrefabs;
    [SerializeField] Transform spawnpoints;
    [SerializeField] Transform customerParent;
    
    int _previousCustomer;

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

    //Wave system
    

    //amount of indgredient

    //Random indgredient selector 

    
}
