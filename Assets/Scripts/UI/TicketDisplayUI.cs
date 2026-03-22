using UnityEngine;
using TMPro;

public class TicketDisplayUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI ticketText;

    private void OnEnable()
    {
        EconomyManager.Instance.OnTicketsChanged += UpdateDisplay;
    }

    private void Start()
    {
        UpdateDisplay(EconomyManager.Instance.CurrentTickets);
    }

    private void OnDisable()
    {
        if (EconomyManager.Instance != null)
        {
            EconomyManager.Instance.OnTicketsChanged -= UpdateDisplay;
        }
    }

    private void UpdateDisplay(int currentTickets)
    {
        if (ticketText != null)
        {
            ticketText.text = currentTickets.ToString();
        }
    }
}