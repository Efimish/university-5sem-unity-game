using UnityEngine;

// BulletScript.cs

public class BulletScript : MonoBehaviour
{
    public float speed = 10f; 
    public Rigidbody2D rb;
    public float lifetime = 2f; 

    void Awake() // Используем Awake, чтобы подготовить RB сразу
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void SetDirection(Vector2 dir)
    {
        if (rb == null) rb = GetComponent<Rigidbody2D>();
        
        rb.linearVelocity = dir.normalized * speed; 

        // Разворачиваем саму пулю (визуально)
        if (dir.x < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
    }

    void Start()
    {
        Destroy(gameObject, lifetime); 
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        // 1. Проверяем столкновение с другой пулей
        if (other.GetComponent<BulletScript>() != null)
        {
            Destroy(other.gameObject); 
            Destroy(gameObject);
            return; 
        }

        // 2. Пытаемся получить компонент Creature (игрок или враг)
        Creature victim = other.GetComponent<Creature>();

        if (victim != null)
        {
            // Наносим урон
            victim.GetDamage();
            // Уничтожаем пулю после попадания
            Destroy(gameObject);
            return;
        }

        // 3. Столкновение со стенами (если у них нет скрипта Creature)
        // Убедитесь, что слои настроены так, чтобы пуля не билась об "пустоту"
        if (!other.isTrigger) 
        {
            Destroy(gameObject);
        }
    }

    
}