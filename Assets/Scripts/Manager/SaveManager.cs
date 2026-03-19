using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void SaveShelfStock(string shelfID, int stock)
    {
        PlayerPrefs.SetInt($"ShelfStock_{shelfID}", stock);
    }

    public int LoadShelfStock(string shelfID, int defaultStock)
    {
        return PlayerPrefs.GetInt($"ShelfStock_{shelfID}", defaultStock);
    }

    public void SaveLong(string key, long value)
    {
        PlayerPrefs.SetString(key, value.ToString());
    }

    public long LoadLong(string key, long defaultValue)
    {
        string savedValue = PlayerPrefs.GetString(key, defaultValue.ToString());
        if (long.TryParse(savedValue, out long result))
        {
            return result;
        }
        return defaultValue;
    }
}