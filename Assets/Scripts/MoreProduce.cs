using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MoreProduce : MonoBehaviour
{
    public Button seasonalButton;
    public int produceCount;
    public TMP_Text produceCountText;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        UpdateUI();
    }

    public void AddProduce()
    {
        EconomyManager.Instance.currentProduce += 200;
        UpdateUI();
    }

    void UpdateUI()
    {
        produceCountText.text = EconomyManager.Instance.CurrentProduce.ToString();
    }

    // Update is called once per frame
   
}
