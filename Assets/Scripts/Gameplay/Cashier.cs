using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class Cashier : MonoBehaviour
{
    [SerializeField] private Transform checkoutPosition;
    [SerializeField] private Vector3 queueDirection = new Vector3(0, -1.5f, 0);
    [SerializeField] private float processingTime = 4.0f;

    [Header("UI Feedback")]
    [SerializeField] private GameObject processingUIParent;
    [SerializeField] private Image progressBarFill;
    [SerializeField] private TextMeshProUGUI timerText;

    private List<Customer> customerQueue = new List<Customer>();
    private bool isBusy = false;
    private Customer currentCustomer;

    public int QueueCount => customerQueue.Count + (isBusy ? 1 : 0);

    public float ProcessingTime
    {
        get => processingTime;
        set => processingTime = Mathf.Max(0.1f, value);
    }

    private void OnDisable()
    {
        if (CashierManager.Instance != null)
        {
            CashierManager.Instance.UnregisterCashier(this);
        }
    }

    private void Start()
    {
        CashierManager.Instance.RegisterCashier(this);

        processingUIParent.SetActive(false);
    }

    public Vector3 GetLineTailPosition()
    {
        return checkoutPosition.position + (queueDirection * QueueCount);
    }

    public void JoinQueue(Customer customer)
    {
        customerQueue.Add(customer);

        if (!isBusy)
        {
            ProcessNextInQueue();
        }
        else
        {
            UpdateQueuePositions();
        }
    }

    public void LeaveQueue(Customer customer)
    {
        if (customerQueue.Contains(customer))
        {
            customerQueue.Remove(customer);
            UpdateQueuePositions();
        }
    }

    private void ProcessNextInQueue()
    {
        if (customerQueue.Count > 0 && !isBusy)
        {
            isBusy = true;
            currentCustomer = customerQueue[0];
            customerQueue.RemoveAt(0);

            currentCustomer.GoToCheckout(checkoutPosition.position);
            UpdateQueuePositions();
        }
    }

    public void OnCustomerArrivedAtCounter(Customer customer, List<Product> products)
    {
        StartCoroutine(ProcessTransaction(customer, products));
    }

    private IEnumerator ProcessTransaction(Customer customer, List<Product> products)
    {
        processingUIParent.SetActive(true);

        float elapsed = 0f;
        while (elapsed < processingTime)
        {
            elapsed += Time.deltaTime;
            float progress = elapsed / processingTime;

            progressBarFill.fillAmount = progress;
            timerText.text = (processingTime - elapsed).ToString("F1") + "s";

            yield return null;
        }

        float totalSale = 0;
        foreach (Product product in products)
        {
            totalSale += product.SalePricePerUnit;
        }

        if (totalSale > 0)
        {
            EconomyManager.Instance.AddMoney(totalSale);
        }

        processingUIParent.SetActive(false);

        customer.LeaveStore(true);
        TransactionComplete();
    }

    private void TransactionComplete()
    {
        isBusy = false;
        currentCustomer = null;
        ProcessNextInQueue();
    }

    private void UpdateQueuePositions()
    {
        for (int i = 0; i < customerQueue.Count; i++)
        {
            Vector3 targetPos = checkoutPosition.position + (queueDirection * (i + 1));
            customerQueue[i].SetQueuePosition(targetPos);
        }
    }
}