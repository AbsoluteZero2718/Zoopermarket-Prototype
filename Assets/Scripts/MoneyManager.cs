using UnityEngine;
using TMPro;
using System.Collections;

public class MoneyManager : MonoBehaviour
{
    public static MoneyManager Instance{get; private set;}

    [SerializeField] private long currentMoney;
    [SerializeField] private TextMeshProUGUI moneyText;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        UpdateMoneyUI();
        StartCoroutine(AutoEarn(1f));
    }

    private void ChangeMoney(long amount)
    {
        currentMoney += amount;
        UpdateMoneyUI();
    }

    public bool canAfford(long cost)
    {
        return currentMoney >= cost;
    }

    public bool TryPurchase(long cost)
    {
        if(canAfford(cost))
        {
            ChangeMoney(-cost);
            return true;
        }
        return false;
    }

    private void UpdateMoneyUI()
    {
        if(moneyText != null)
        {
            moneyText.text = "$" + currentMoney.ToString("N0");
        }
    }

    private IEnumerator AutoEarn(float interval)
    {
        while (true)
        {
            yield return new WaitForSeconds(interval);
            ChangeMoney(15);
        }
    }

    // Update is called once per frame
   
}
