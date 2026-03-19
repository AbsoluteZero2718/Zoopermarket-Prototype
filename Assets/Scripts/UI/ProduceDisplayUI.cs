using UnityEngine;
using TMPro;

public class ProduceDisplayUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI produceText;

    private void Awake()
    {
        EconomyManager.Instance.OnProduceChanged += UpdateDisplay;
    }

    private void Start()
    {
        UpdateDisplay(EconomyManager.Instance.CurrentProduce);
    }

    private void OnDisable()
    {
        if (EconomyManager.Instance != null)
        {
            EconomyManager.Instance.OnProduceChanged -= UpdateDisplay;
        }
    }

    private void UpdateDisplay(int currentProduce)
    {
        produceText.text = currentProduce.ToString("N0");
    }
}