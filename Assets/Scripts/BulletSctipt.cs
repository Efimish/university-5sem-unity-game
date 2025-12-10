using UnityEngine;

// BulletScript.cs

public class BulletScript : MonoBehaviour
{
    public float speed = 10f; 
    public Rigidbody2D rb;
    private Vector2 direction = Vector2.right; 
    public float lifetime = 5f; 

    public void SetDirection(Vector2 dir)
    {
        direction = dir.normalized; 
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Destroy(gameObject, lifetime); 
    }
    
    void FixedUpdate() 
    {
        // Использовать .velocity
        rb.linearVelocity = direction * speed; 
    }

    // private void OnCollisionEnter2D(Collision2D collision)
    // {

    //     if (collision.gameObject.GetComponent<BulletScript>() != null)
    //     {
    //         return; 
    //     }
        
    //     if (collision.gameObject.CompareTag("Player"))
    //     {
    //         return;
    //     }

    //     Destroy(gameObject);
    // }


    private void OnTriggerEnter2D(Collider2D other) 
{
    GameObject hitObject = other.gameObject;

    // Игнорируем игрока и другие пули
    if (hitObject.CompareTag("Player") || hitObject.GetComponent<BulletScript>() != null)
    {
        return; 
    }


    // EnemyScript enemy = hitObject.GetComponent<EnemyScript>();
    // if (enemy != null)
    // {
    //     enemy.TakeDamage(damageAmount);
    // }

    // Уничтожаем пулю
    Destroy(gameObject);
}

    void Update()
    {
        
    }
}