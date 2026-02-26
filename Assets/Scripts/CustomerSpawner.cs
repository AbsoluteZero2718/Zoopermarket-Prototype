using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CustomerSpawner : MonoBehaviour
{
    [SerializeField] public GameObject customerPrefab;
    [SerializeField] public int maxCustomers;
    [SerializeField] private int currentCustomerCount = 0;
    [SerializeField] private float spawnTime = 5f;
    [SerializeField] private bool isSpawning = true;

    void Start()
    {
        StartCoroutine(SpawnManager());
    }

    IEnumerator SpawnManager()
    {
        while (isSpawning)
        {
            if (currentCustomerCount < maxCustomers)
            {
                SpawnCustomer();
            }
            yield return new WaitForSeconds(spawnTime);
        }
    }

    void SpawnCustomer()
    {
        Instantiate(customerPrefab, transform.position, Quaternion.identity);
        currentCustomerCount++;
    }
    public void CustomerDestroyed()
    {
        currentCustomerCount--;
    }
}

    
