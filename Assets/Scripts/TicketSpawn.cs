using UnityEngine;
using System.Collections;

public class TicketSpawn : MonoBehaviour
{
    [SerializeField] private GameObject ticketPrefab;
    [SerializeField] private float spawnInterval = 10f;

    [Header("Spawn Area")]
    [SerializeField] private float minX = -8f;
    [SerializeField] private float maxX = 8f;
    [SerializeField] private float minY = -4f;
    [SerializeField] private float maxY = 4f;

    [Header("Gizmo Settings")]
    [SerializeField] private Color gizmoColor = Color.yellow;

    private void Start()
    {
        StartCoroutine(SpawnTicketsRoutine());
    }

    private IEnumerator SpawnTicketsRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);

            Vector2 randomPos = new Vector2(Random.Range(minX, maxX), Random.Range(minY, maxY));
            Instantiate(ticketPrefab, randomPos, Quaternion.identity);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Vector3 center = new Vector3((minX + maxX) / 2f, (minY + maxY) / 2f, 0);
        Vector3 size = new Vector3(maxX - minX, maxY - minY, 0.1f);

        Gizmos.color = gizmoColor;
        Gizmos.DrawWireCube(center, size);
    }
}