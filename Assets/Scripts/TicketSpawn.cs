using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;


public class TicketSpawn : MonoBehaviour
{
    public GameObject ticketPrefab;

    public float minX = -8f;
    public float maxX = 8f;
    public float minY = -4f;
    public float maxY = 4f;

    public TMP_Text ticketCountText;
    public int ticketCount = 0;

    public float spawnInterval = 10f;
    public static TicketSpawn instance;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    void Start()
    {
        StartCoroutine(SpawnTickets());
    }

    // Update is called once per frame
    void Update()
    {
        SpawnItem();

    }

    public void SpawnItem()
    {
        if(ticketCountText != null)
        {
            ticketCountText.text = "" + ticketCount;

            if(Input.GetMouseButtonDown(0))
            {
                Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

                RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);

                if(hit.collider != null)
                {
                    Destroy(hit.collider.gameObject);
                    ticketCount++;

                }
            }
          
           
        }

    }

   
    IEnumerator SpawnTickets()
    {

        Vector2 randomPos = new Vector2(Random.Range(minX, maxX), Random.Range(minY, maxY));
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);
            Instantiate(ticketPrefab, randomPos, Quaternion.identity);
            

        }
       
    }
}
