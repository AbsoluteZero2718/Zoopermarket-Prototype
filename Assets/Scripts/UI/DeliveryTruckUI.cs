using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

[Serializable]
public struct DeliveryOption
{
    public string optionName;
    public float costPHP;
    public int durationMinutes;
    public int produceReward;
}

public class DeliveryTruckUI : MonoBehaviour
{
    [Header("Truck Settings")]
    [SerializeField] private string truckID = "Truck_1";
    [SerializeField] private string truckDisplayName = "Truck 1";
    [SerializeField] private DeliveryOption[] availableOptions;

    [Header("UI Panels")]
    [SerializeField] private GameObject selectionPanel;
    [SerializeField] private GameObject activeDeliveryPanel;

    [Header("General UI")]
    [SerializeField] private TextMeshProUGUI truckNameText;

    [Header("Selection UI")]
    [SerializeField] private TextMeshProUGUI optionNameText;
    [SerializeField] private TextMeshProUGUI optionCostText;
    [SerializeField] private TextMeshProUGUI optionRewardText;
    [SerializeField] private Button sendButton;
    [SerializeField] private Button nextOptionButton;
    [SerializeField] private Button prevOptionButton;

    [Header("Active Delivery UI")]
    [SerializeField] private TextMeshProUGUI statusText;
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private Button claimButton;

    private int currentOptionIndex = 0;
    private bool isDelivering = false;
    private bool isReadyToClaim = false;
    private DateTime deliveryEndTime;
    private int pendingReward = 0;

    private void Start()
    {
        if (truckNameText != null)
        {
            truckNameText.text = truckDisplayName;
        }

        LoadState();
        UpdateSelectionUI();

        sendButton.onClick.AddListener(StartDelivery);
        claimButton.onClick.AddListener(ClaimProduce);
        nextOptionButton.onClick.AddListener(() => ChangeOption(1));
        prevOptionButton.onClick.AddListener(() => ChangeOption(-1));
    }

    private void Update()
    {
        if (isDelivering && !isReadyToClaim)
        {
            TimeSpan timeRemaining = deliveryEndTime - DateTime.UtcNow;

            if (timeRemaining.TotalSeconds <= 0)
            {
                CompleteDelivery();
            }
            else
            {
                timerText.text = string.Format("{0:D2}:{1:D2}:{2:D2}",
                    timeRemaining.Hours,
                    timeRemaining.Minutes,
                    timeRemaining.Seconds);
            }
        }
    }

    private void ChangeOption(int direction)
    {
        currentOptionIndex += direction;

        if (currentOptionIndex >= availableOptions.Length) currentOptionIndex = 0;
        if (currentOptionIndex < 0) currentOptionIndex = availableOptions.Length - 1;

        UpdateSelectionUI();
    }

    private void UpdateSelectionUI()
    {
        if (availableOptions.Length == 0) return;

        DeliveryOption currentOption = availableOptions[currentOptionIndex];
        optionNameText.text = currentOption.optionName;
        optionCostText.text = $"Cost: {currentOption.costPHP:N0} Gold";
        optionRewardText.text = $"Reward: {currentOption.produceReward} Produce";

        sendButton.interactable = EconomyManager.Instance.CanAfford(currentOption.costPHP);
    }

    private void StartDelivery()
    {
        DeliveryOption selectedOption = availableOptions[currentOptionIndex];

        if (EconomyManager.Instance.CanAfford(selectedOption.costPHP))
        {
            EconomyManager.Instance.SpendMoney(selectedOption.costPHP);

            pendingReward = selectedOption.produceReward;
            deliveryEndTime = DateTime.UtcNow.AddMinutes(selectedOption.durationMinutes);
            isDelivering = true;
            isReadyToClaim = false;

            SaveState();
            UpdatePanels();
        }
    }

    private void CompleteDelivery()
    {
        isReadyToClaim = true;
        timerText.text = "ARRIVED!";
        statusText.text = $"Claim Reward:\n{pendingReward} Produce";
        claimButton.gameObject.SetActive(true);
    }

    private void ClaimProduce()
    {
        EconomyManager.Instance.AddProduce(pendingReward);

        isDelivering = false;
        isReadyToClaim = false;
        pendingReward = 0;

        PlayerPrefs.DeleteKey(truckID + "_EndTime");
        PlayerPrefs.DeleteKey(truckID + "_Reward");

        UpdatePanels();
        UpdateSelectionUI();
    }

    private void UpdatePanels()
    {
        selectionPanel.SetActive(!isDelivering);
        activeDeliveryPanel.SetActive(isDelivering);
        claimButton.gameObject.SetActive(isReadyToClaim);

        if (isDelivering && !isReadyToClaim)
        {
            statusText.text = "On Delivery...";
        }
        else if (isReadyToClaim)
        {
            statusText.text = $"Claim Reward:\n{pendingReward} Produce";
        }
    }

    private void SaveState()
    {
        PlayerPrefs.SetString(truckID + "_EndTime", deliveryEndTime.ToBinary().ToString());
        PlayerPrefs.SetInt(truckID + "_Reward", pendingReward);
        PlayerPrefs.Save();
    }

    private void LoadState()
    {
        string savedTime = PlayerPrefs.GetString(truckID + "_EndTime", "");
        if (!string.IsNullOrEmpty(savedTime))
        {
            long binaryTime = Convert.ToInt64(savedTime);
            deliveryEndTime = DateTime.FromBinary(binaryTime);
            pendingReward = PlayerPrefs.GetInt(truckID + "_Reward", 0);
            isDelivering = true;

            if (DateTime.UtcNow >= deliveryEndTime)
            {
                CompleteDelivery();
            }
        }
        else
        {
            isDelivering = false;
        }

        UpdatePanels();
    }
}