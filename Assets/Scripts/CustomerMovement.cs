using UnityEngine;
using System.Collections;

public class CustomerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float minWaitTime = 1f;
    public float maxWaitTime = 3f;
    public float minWalkTime = 1f;
    public float maxWalkTime = 3f;

    private Rigidbody2D rb;
    private Vector2 moveDirection;
    private bool isWalking = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        StartCoroutine(Wander());
    }

    // Update is called once per frame
    void Update()
    {
        rb.linearVelocity = isWalking ? moveDirection * moveSpeed : Vector2.zero;
    }

    IEnumerator Wander()
    {
        while (true)
        {
            isWalking = false;
            float waitTime = Random.Range(minWaitTime, maxWaitTime);
            yield return new WaitForSeconds(waitTime);

            isWalking = true;
            moveDirection = Random.insideUnitCircle.normalized;

            float walkTime = Random.Range(minWalkTime, maxWalkTime);
            yield return new WaitForSeconds(walkTime);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isWalking)
        {
            isWalking = false;
        }
    }
}
