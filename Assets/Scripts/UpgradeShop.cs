using UnityEngine;

public class UpgradeShop : MonoBehaviour
{
    public GameObject upgradePanel;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
   public void ToggleUpgradeMenu()
    {
        if(upgradePanel != null)
        {
            upgradePanel.SetActive(!upgradePanel.activeSelf);
        }
    }
}
