using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
public class CashierUpgrade : MonoBehaviour
{
    public int totalCoins;
    public TextMeshProUGUI coinText;

    public int cashierSpeedlevel = 1;
    public int speedUpgradeCost = 50;
    public Button upgradeCashierButton;
    public TextMeshProUGUI speedCostText;

    public MoneyManager moneyManager;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        totalCoins = PlayerPrefs.GetInt("Coins", 0);
        UpdateUI();
    }

    public void BuySpeedUpgrade()
    {
        if (totalCoins >= speedUpgradeCost)
        {
            totalCoins -= speedUpgradeCost;
            cashierSpeedlevel++;

            speedUpgradeCost += 50;

            PlayerPrefs.SetInt("Coins", totalCoins);
            PlayerPrefs.SetInt("SpeedLevel", cashierSpeedlevel);

            Debug.Log("Cashier Speed Upgraded! Cashier now " + cashierSpeedlevel);
            UpdateUI();
        }
        else
        {
            Debug.Log("Not enough coins to upgrade!");
        }
    }

    public void UpdateUI()
    {
        coinText.text = "Coins: " + totalCoins.ToString();

        upgradeCashierButton.interactable = (totalCoins >= speedUpgradeCost);
    }

    
}

    // Update is called once per frame
    