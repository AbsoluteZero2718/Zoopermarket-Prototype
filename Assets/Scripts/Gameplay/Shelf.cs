using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Shelf : MonoBehaviour
{
    [SerializeField] private string shelfID;
    [SerializeField] private Product product;
    [SerializeField] private int maxStock = 100;

    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI productText;
    [SerializeField] private TextMeshProUGUI stockText;
    [SerializeField] private Button restockButton;
    [SerializeField] private TextMeshProUGUI restockButtonText;

    private int currentStock;

    private void Awake()
    {
        EconomyManager.Instance.OnProduceChanged += HandleProduceChanged;
    }

    private void Start()
    {
        if (string.IsNullOrEmpty(shelfID))
        {
            Debug.LogError("Shelf ID is not set!", this);
            return;
        }

        currentStock = SaveManager.Instance.LoadShelfStock(shelfID, maxStock);

        productText.text = product.ProductName;

        UpdateStockDisplay();
        UpdateRestockUI();

        restockButton.onClick.AddListener(Restock);
    }

    private void OnDisable()
    {
        if (EconomyManager.Instance != null)
        {
            EconomyManager.Instance.OnProduceChanged -= HandleProduceChanged;
        }
    }

    public void Restock()
    {
        int neededStock = maxStock - currentStock;
        if (neededStock <= 0) return;

        int totalProduceCost = neededStock * product.RestockProduceCostPerUnit;

        if (EconomyManager.Instance.HasEnoughProduce(totalProduceCost))
        {
            EconomyManager.Instance.SpendProduce(totalProduceCost);
            currentStock = maxStock;

            SaveManager.Instance.SaveShelfStock(shelfID, currentStock);
            UpdateStockDisplay();
            UpdateRestockUI();

            // Play restock SFX (you can put SFX here)
        }
        else
        {
            // Play insufficient produce SFX (you can put SFX here)
        }
    }

    public bool HasStock() => currentStock > 0;

    public Product TakeProduct()
    {
        if (HasStock())
        {
            currentStock--;
            SaveManager.Instance.SaveShelfStock(shelfID, currentStock);
            UpdateStockDisplay();
            UpdateRestockUI();
            return product;
        }
        return null;
    }

    private void HandleProduceChanged(int newProduceAmount)
    {
        UpdateRestockUI();
    }

    private void UpdateStockDisplay()
    {
        stockText.text = $"Stock: {currentStock}";
    }

    private void UpdateRestockUI()
    {
        int needed = maxStock - currentStock;
        int produceCost = needed * product.RestockProduceCostPerUnit;

        bool hasEnoughProduce = EconomyManager.Instance.HasEnoughProduce(produceCost);
        restockButton.interactable = needed > 0 && hasEnoughProduce;

        restockButtonText.text = needed > 0 ? $"Restock ({produceCost} Produce)" : "Full";
    }
}