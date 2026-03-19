using UnityEngine;
using TMPro;

public class EconomyDisplayUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI goldText;

    private void OnEnable()
    {
        EconomyManager.Instance.OnMoneyChanged += UpdateDisplay;
    }

    private void Start()
    {
        UpdateDisplay(EconomyManager.Instance.CurrentMoney);
    }

    private void OnDisable()
    {
        if (EconomyManager.Instance != null)
        {
            EconomyManager.Instance.OnMoneyChanged -= UpdateDisplay;
        }
    }

    private void UpdateDisplay(long currentGold)
    {
        goldText.text = currentGold.ToString("N0");
    }
}