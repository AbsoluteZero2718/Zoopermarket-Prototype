using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Collections.Generic;

[Serializable]
public class TruckTabSettings
{
    public string truckName = "Truck";
    public string saveID = "Truck_ID";
    public int unlockCost = 0;
    public Button tabButton;
    public TextMeshProUGUI buttonText;
    public GameObject interfacePanel;
}

public class DeliveryMenuManager : MonoBehaviour
{
    [SerializeField] private List<TruckTabSettings> truckTabs = new List<TruckTabSettings>();

    private void OnEnable()
    {
        EconomyManager.Instance.OnMoneyChanged += HandleMoneyChanged;
    }

    private void Start()
    {
        for (int i = 0; i < truckTabs.Count; i++)
        {
            int index = i;
            truckTabs[i].tabButton.onClick.AddListener(() => OnTabClicked(index));
        }

        UpdateAllTabVisuals();
        SelectTruck(0);
    }

    private void OnDisable()
    {
        if (EconomyManager.Instance != null)
        {
            EconomyManager.Instance.OnMoneyChanged -= HandleMoneyChanged;
        }
    }

    private void OnTabClicked(int index)
    {
        TruckTabSettings settings = truckTabs[index];

        if (!IsUnlocked(index))
        {
            AttemptUnlockTruck(index);
            return;
        }

        SelectTruck(index);
    }

    private void SelectTruck(int index)
    {
        for (int i = 0; i < truckTabs.Count; i++)
        {
            bool isSelected = (i == index);
            truckTabs[i].interfacePanel.SetActive(isSelected && IsUnlocked(i));
        }
    }

    private void AttemptUnlockTruck(int index)
    {
        TruckTabSettings settings = truckTabs[index];

        if (EconomyManager.Instance.CanAfford(settings.unlockCost))
        {
            EconomyManager.Instance.SpendMoney(settings.unlockCost);

            PlayerPrefs.SetInt("Unlocked_" + settings.saveID, 1);
            PlayerPrefs.Save();

            UpdateAllTabVisuals();
            SelectTruck(index);
        }
    }

    private bool IsUnlocked(int index)
    {
        TruckTabSettings settings = truckTabs[index];

        if (settings.unlockCost <= 0) return true;

        return PlayerPrefs.GetInt("Unlocked_" + settings.saveID, 0) == 1;
    }

    private void HandleMoneyChanged(long currentMoney)
    {
        UpdateAllTabVisuals();
    }

    private void UpdateAllTabVisuals()
    {
        for (int i = 0; i < truckTabs.Count; i++)
        {
            TruckTabSettings settings = truckTabs[i];

            if (IsUnlocked(i))
            {
                settings.buttonText.text = settings.truckName;
                settings.tabButton.interactable = true;
            }
            else
            {
                settings.buttonText.text = $"Unlock\n{settings.unlockCost:N0} Gold";
                settings.tabButton.interactable = EconomyManager.Instance.CanAfford(settings.unlockCost);
            }
        }
    }
}