using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CashierManager : MonoBehaviour
{
    public static CashierManager Instance { get; private set; }

    public List<Cashier> ActiveCashiers { get; private set; } = new List<Cashier>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void RegisterCashier(Cashier cashier)
    {
        if (!ActiveCashiers.Contains(cashier))
        {
            ActiveCashiers.Add(cashier);
        }
    }

    public void UnregisterCashier(Cashier cashier)
    {
        if (ActiveCashiers.Contains(cashier))
        {
            ActiveCashiers.Remove(cashier);
        }
    }

    public Cashier FindBestCashier()
    {
        if (ActiveCashiers.Count == 0)
        {
            return null;
        }

        Cashier bestCashier = ActiveCashiers.OrderBy(c => c.QueueCount).FirstOrDefault();
        return bestCashier;
    }
}