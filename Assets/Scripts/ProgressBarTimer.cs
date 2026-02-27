using UnityEngine;
using UnityEngine.UI;

public class ProgressBarTimer : MonoBehaviour
{
    public Image progressBarIMG;
    public float duration = 5.0f; // Seconds to fill
    private float timeLeft;
    private bool isCounting = false;

    public GameObject progressBarPrefab;
    public Transform progressBarSpawnPoint;

    public float currentProgress = 0f;
    public float maxProgress = 100f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        if (progressBarIMG != null) {
            progressBarIMG.fillAmount = 0;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isCounting)
        {
            if(timeLeft > 0)
            {
                timeLeft -= Time.deltaTime;
                progressBarIMG.fillAmount = 1 - (timeLeft / duration);
            }
            else
            {
                progressBarIMG.fillAmount = 1;
                isCounting = false;
            }
        }

        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Customer"))
        {
            Instantiate(progressBarPrefab, progressBarSpawnPoint.position, Quaternion.identity);
            StartTimer();
        }

        if (collision.gameObject.CompareTag("Counter"))
        {
            Instantiate(progressBarPrefab, progressBarSpawnPoint.position, Quaternion.identity);
            StartTimer();
        }
    }

    public void StartTimer()
    {
        if (!isCounting) // Prevents restarting if already counting
        {
            timeLeft = duration;
            isCounting = true;
        }
    }

    public void UpdateProgress(float addedProgress)
    {
        currentProgress += addedProgress;
        currentProgress = Mathf.Clamp(currentProgress, 0f, maxProgress);
        UpdateVisuals();
    }

    public void UpdateVisuals()
    {
        float fillAmount = currentProgress / maxProgress;

        if (progressBarIMG != null)
        {
            progressBarIMG.fillAmount = fillAmount;
        }
    }
}
