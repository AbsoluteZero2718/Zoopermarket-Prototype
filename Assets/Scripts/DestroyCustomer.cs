using UnityEngine;

public class DestroyCustomer : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.CompareTag("Exit"))
        {
            // Destroy the character object the script is attached to
            Destroy(gameObject);
        }
    }
}
