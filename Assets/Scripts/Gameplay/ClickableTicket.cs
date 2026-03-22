using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class ClickableTicket : MonoBehaviour
{
    private void OnMouseDown()
    {
        EconomyManager.Instance.AddTickets(1);
        Destroy(gameObject);
    }
}