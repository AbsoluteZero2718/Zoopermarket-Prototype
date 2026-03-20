using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MoreTickets : MonoBehaviour
{
    public Button ticketBundleButton;
    public int ticketCount;
    public TMP_Text ticketCountText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        UpdateUI();    
    }

    public void AddTickets()
    {
        TicketSpawn.instance.ticketCount += 50;
        UpdateUI();
    }

    void UpdateUI()
    {
        ticketCountText.text = TicketSpawn.instance.ticketCount.ToString();
    }

    // Update is called once per frame
  
}
