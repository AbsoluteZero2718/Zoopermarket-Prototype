using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MoreTickets : MonoBehaviour
{
    public int ticketCount;
    public TMP_Text ticketCountText;

    private void Start()
    {
        ticketCountText.text = "x" + ticketCount.ToString();
    }

    public void AddTickets()
    {
        EconomyManager.Instance.AddTickets(ticketCount);
    }
}