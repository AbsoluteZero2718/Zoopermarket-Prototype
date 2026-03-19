using System.Collections;
using UnityEngine;

public class CustomerSpawner : MonoBehaviour
{
    [SerializeField] private GameObject customerPrefab;
    [SerializeField] private float initialSpawnInterval = 8f;
    [SerializeField] private Transform exitPoint;
    [SerializeField] private bool isSpawning = true;

    private float currentSpawnInterval;
    private const string SPAWN_INTERVAL_KEY = "CustomerSpawnInterval";

    private void Start()
    {
        if (exitPoint == null)
        {
            Debug.LogError("Customer Spawner is missing a reference to the exit point.");
            isSpawning = false;
            return;
        }

        currentSpawnInterval = PlayerPrefs.GetFloat(SPAWN_INTERVAL_KEY, initialSpawnInterval);
        StartCoroutine(SpawnManager());
    }

    private IEnumerator SpawnManager()
    {
        while (isSpawning)
        {
            yield return new WaitForSeconds(currentSpawnInterval);

            if (CashierManager.Instance.ActiveCashiers.Count > 0)
            {
                SpawnCustomer();
            }
        }
    }

    private void SpawnCustomer()
    {
        Cashier bestCashier = CashierManager.Instance.FindBestCashier();
        if (bestCashier == null) return;

        GameObject customerGO = Instantiate(customerPrefab, transform.position, Quaternion.identity);
        Customer customer = customerGO.GetComponent<Customer>();

        if (customer != null)
        {
            customer.Initialize(bestCashier, exitPoint);
        }
        else
        {
            Debug.LogError("Spawned prefab does not have a Customer component.", customerGO);
        }
    }

    public void ReduceSpawnInterval(float amount)
    {
        currentSpawnInterval = Mathf.Max(1f, currentSpawnInterval - amount);
        PlayerPrefs.SetFloat(SPAWN_INTERVAL_KEY, currentSpawnInterval);
    }
}