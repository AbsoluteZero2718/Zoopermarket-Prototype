using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class CashierPurchaseSystem : MonoBehaviour
{
    [Header("Configuration")]
    [SerializeField] private GameObject cashierPrefab;
    [SerializeField] private List<Transform> cashierSpawnPoints;
    [SerializeField] private List<int> purchaseCosts;
    [SerializeField] private float spawnReductionPerCashier = 1.5f;

    [Header("System References")]
    [SerializeField] private CustomerSpawner customerSpawner;

    [Header("UI References")]
    [SerializeField] private Button purchaseButton;
    [SerializeField] private TextMeshProUGUI costText;
    [SerializeField] private TextMeshProUGUI countText;

    private int purchasedCount = 1;
    private const string SAVE_KEY = "PurchasedCashierCount";

    private void Awake()
    {
        EconomyManager.Instance.OnMoneyChanged += HandleMoneyChanged;
    }

    private void Start()
    {
        purchasedCount = PlayerPrefs.GetInt(SAVE_KEY, 1);

        for (int i = 0; i < purchasedCount - 1; i++)
        {
            if (i < cashierSpawnPoints.Count)
            {
                Instantiate(cashierPrefab, cashierSpawnPoints[i].position, cashierSpawnPoints[i].rotation);
            }
        }

        UpdateUI();
        purchaseButton.onClick.AddListener(AttemptPurchase);
    }

    private void OnDisable()
    {
        if (EconomyManager.Instance != null)
        {
            EconomyManager.Instance.OnMoneyChanged -= HandleMoneyChanged;
        }
    }

    public void AttemptPurchase()
    {
        int maxCashiers = cashierSpawnPoints.Count + 1;
        if (purchasedCount >= maxCashiers) return;

        int cost = purchaseCosts[purchasedCount - 1];

        if (EconomyManager.Instance.CanAfford(cost))
        {
            EconomyManager.Instance.SpendMoney(cost);

            int spawnIndex = purchasedCount - 1;
            Instantiate(cashierPrefab, cashierSpawnPoints[spawnIndex].position, cashierSpawnPoints[spawnIndex].rotation);

            purchasedCount++;
            PlayerPrefs.SetInt(SAVE_KEY, purchasedCount);

            customerSpawner.ReduceSpawnInterval(spawnReductionPerCashier);

            UpdateUI();
        }
    }

    private void HandleMoneyChanged(long newAmount)
    {
        UpdateUI();
    }

    private void UpdateUI()
    {
        int maxCashiers = cashierSpawnPoints.Count + 1;
        countText.text = $"{purchasedCount} / {maxCashiers}";

        if (purchasedCount >= maxCashiers)
        {
            costText.text = "MAX";
            purchaseButton.interactable = false;
        }
        else
        {
            int cost = purchaseCosts[purchasedCount - 1];
            costText.text = cost.ToString("N0");
            purchaseButton.interactable = EconomyManager.Instance.CanAfford(cost);
        }
    }
}