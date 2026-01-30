using UnityEngine;
using UnityEngine.UI;

public class UpgradePurchase : MonoBehaviour
{
    long buildingCost = 300;

    public Button upgradeButton;

    void Start()
    {
        Button btn = upgradeButton.GetComponent<Button>();
        btn.onClick.AddListener(PurchaseUpgrade);
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public void PurchaseUpgrade()
    {
        if (MoneyManager.Instance.TryPurchase(buildingCost))
        {
            Debug.Log("Building Upgraded!");
        }
        else
        {
            Debug.Log("Not Enough money!");
        }
    }
}
