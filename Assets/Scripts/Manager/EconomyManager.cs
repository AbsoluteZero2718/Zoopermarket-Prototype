using UnityEngine;
using System;

public class EconomyManager : MonoBehaviour
{
    public static EconomyManager Instance { get; private set; }

    public event Action<long> OnMoneyChanged;

    public event Action<int> OnProduceChanged;

    public event Action<int> OnTicketsChanged;

    [Header("Starting Values")]
    [SerializeField] private long startingMoney = 200;
    [SerializeField] private int startingProduce = 50;
    [SerializeField] private int startingTickets = 0;

    private long currentMoney;
    private int currentProduce;
    private int currentTickets;

    public long CurrentMoney => currentMoney;
    public int CurrentProduce => currentProduce;
    public int CurrentTickets => currentTickets;

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
        currentTickets = PlayerPrefs.GetInt("PlayerTickets", startingTickets);

        OnMoneyChanged?.Invoke(currentMoney);
        OnProduceChanged?.Invoke(currentProduce);
        OnTicketsChanged?.Invoke(currentTickets);
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

    public void AddTickets(int amount)
    {
        if (amount <= 0) return;

        currentTickets += amount;
        PlayerPrefs.SetInt("PlayerTickets", currentTickets);
        OnTicketsChanged?.Invoke(currentTickets);
    }

    public void SpendTickets(int amount)
    {
        if (amount <= 0) return;

        currentTickets -= amount;
        PlayerPrefs.SetInt("PlayerTickets", currentTickets);
        OnTicketsChanged?.Invoke(currentTickets);
    }

    public bool HasEnoughTickets(int amount)
    {
        return currentTickets >= amount;
    }
}