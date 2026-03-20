using UnityEngine;
using TMPro;

public class ProduceDisplayUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI produceText;

    public void Awake()
    {
        EconomyManager.Instance.OnProduceChanged += UpdateDisplay;
    }

    public void Start()
    {
        UpdateDisplay(EconomyManager.Instance.CurrentProduce);
    }

    public void OnDisable()
    {
        if (EconomyManager.Instance != null)
        {
            EconomyManager.Instance.OnProduceChanged -= UpdateDisplay;
        }
    }

    public void UpdateDisplay(int currentProduce)
    {
        produceText.text = currentProduce.ToString("N0");
    }
}