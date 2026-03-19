using UnityEngine;

[CreateAssetMenu(fileName = "New Product", menuName = "Gameplay/Product")]
public class Product : ScriptableObject
{
    [SerializeField] private string productName;
    [SerializeField] private int restockProduceCostPerUnit = 1;
    [SerializeField] private float salePricePerUnit;

    public string ProductName => productName;
    public int RestockProduceCostPerUnit => restockProduceCostPerUnit;
    public float SalePricePerUnit => salePricePerUnit;
}