using UnityEngine;
using System;

public class EconomyManager : MonoBehaviour
{
    public static EconomyManager Instance { get; private set; }

    public event Action<long> OnMoneyChanged;

    public event Action<int> OnProduceChanged;

    [SerializeField] private long startingMoney = 200;
    [SerializeField] private int startingProduce = 50;

    private long currentMoney;
    public int currentProduce;

    public long CurrentMoney => currentMoney;
    public int CurrentProduce => currentProduce;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        currentMoney = SaveManager.Instance.LoadLong("PlayerMoney", startingMoney);
        currentProduce = PlayerPrefs.GetInt("PlayerProduce", startingProduce);

        OnMoneyChanged?.Invoke(currentMoney);
        OnProduceChanged?.Invoke(currentProduce);
    }

    public void AddMoney(float amount)
    {
        long longAmount = (long)amount;
        if (longAmount <= 0) return;

        currentMoney += longAmount;
        SaveManager.Instance.SaveLong("PlayerMoney", currentMoney);
        OnMoneyChanged?.Invoke(currentMoney);
    }

    public void SpendMoney(float amount)
    {
        long longAmount = (long)amount;
        if (longAmount <= 0) return;

        currentMoney -= longAmount;
        SaveManager.Instance.SaveLong("PlayerMoney", currentMoney);
        OnMoneyChanged?.Invoke(currentMoney);
    }

    public bool CanAfford(float amount)
    {
        return currentMoney >= amount;
    }

    public void AddProduce(int amount)
    {
        if (amount <= 0) return;

        currentProduce += amount;
        PlayerPrefs.SetInt("PlayerProduce", currentProduce);
        OnProduceChanged?.Invoke(currentProduce);
    }

    public void SpendProduce(int amount)
    {
        if (amount <= 0) return;

        currentProduce -= amount;
        PlayerPrefs.SetInt("PlayerProduce", currentProduce);
        OnProduceChanged?.Invoke(currentProduce);
    }

    public bool HasEnoughProduce(int amount)
    {
        return currentProduce >= amount;
    }
}