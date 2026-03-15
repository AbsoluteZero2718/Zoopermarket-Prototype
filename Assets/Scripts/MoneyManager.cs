using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class MoneyManager : MonoBehaviour
{
    public static MoneyManager Instance{get; private set;}

    [SerializeField] private long currentMoney;
    [SerializeField] private TextMeshProUGUI moneyText;

    public GameObject cashierPrefab;
    public GameObject spawnVFX;
    public int cashierAmt = 1;
    public int speedUpgradeCost = 50;
    public Button upgradeCashierButton;
    public TextMeshProUGUI speedCostText;

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
        currentMoney = PlayerPrefs.GetInt("Coins", 0);

        UpdateMoneyUI();
        StartCoroutine(AutoEarn(1f));
    }

    private void ChangeMoney(long amount)
    {
        currentMoney += amount;
        UpdateMoneyUI();
    }

    public void BuySpeedUpgrade()
    {
        if (currentMoney >= speedUpgradeCost && spawnVFX != null)
        {
            currentMoney -= speedUpgradeCost;
            Instantiate(cashierPrefab, cashierPrefab.transform.position, Quaternion.identity);
            Instantiate(spawnVFX, cashierPrefab.transform.position, Quaternion.identity);
            cashierAmt++;


            speedUpgradeCost += 100;

            PlayerPrefs.SetInt("Coins", (int)currentMoney);
          

            Debug.Log("Cashiers Upgraded! There are now " + cashierAmt + " cashiers!");
            UpdateMoneyUI();
        }
        else
        {
            Debug.Log("Not enough coins to upgrade!");
        }
    }


    

    private void UpdateMoneyUI()
    {
        if(moneyText != null)
        {
            moneyText.text =  currentMoney.ToString("N0");
        }

        speedCostText.text = speedUpgradeCost.ToString();
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
