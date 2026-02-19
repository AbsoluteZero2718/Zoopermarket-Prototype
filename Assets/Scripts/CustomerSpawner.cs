using Unity.VisualScripting;
using UnityEngine;
using System.Collections.Generic;

public class CustomerSpawner : MonoBehaviour
{
    [SerializeField] public GameObject customerPrefab;
    [SerializeField] public int maxCustomers;
    [SerializeField] private int currentCustomerCount = 0;
    [SerializeField] private float spawnTime = 5f;

    [SerializeField] private List<GameObject> instantiatedObjects = new List<GameObject>();

    private float timer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= spawnTime)
        {
            SpawnSprite();
            timer = 0f;
        }
    }

    public void SpawnSprite()
    {
        
        GameObject newObject = Instantiate(customerPrefab, transform.position, Quaternion.identity);
        instantiatedObjects.Add(newObject);
        currentCustomerCount++;

        if(instantiatedObjects.Count > maxCustomers)
        {
            GameObject oldestCustomer = instantiatedObjects[0];
            instantiatedObjects.RemoveAt(0);
            Destroy(oldestCustomer);
            currentCustomerCount--;

        }


    }
    
}
