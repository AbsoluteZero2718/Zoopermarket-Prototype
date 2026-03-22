using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MoreProduce : MonoBehaviour
{
    public int produceCount;
    public TMP_Text produceCountText;

    private void Start()
    {
        produceCountText.text = "x" + produceCount.ToString();
    }

    public void AddProduce()
    {
        EconomyManager.Instance.AddProduce(produceCount);
    }
}