using UnityEngine;

public class DroidScript : Creature
{
    [SerializeField] private int hp = 3; // Здоровье дроида
    private float moveSpeed = 1.5f;

    private int timer;
    public int timerKD;
    private Vector3 dir;
    private SpriteRenderer sprite;

    public int fireKD;
    private int fire;

    // стрельба
    public GameObject Bullet;
    public Transform BulletPosition;

    private Rigidbody2D rb;
    private Animator anim;

    void Start()
    {
        dir = transform.right;
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
        sprite = GetComponentInChildren<SpriteRenderer>();
    }

    void Update()
    {
        Move();
        
        // Таймер разворота
        timer += 1;
        if (timer >= timerKD) // Используем >= на всякий случай
        {
            dir = dir * -1f;
            timer = 0;
            sprite.flipX = !sprite.flipX;
        }

        // Таймер стрельбы
        if (fire >= fireKD)
        {
            GameObject bulletObj = Instantiate(Bullet, BulletPosition.position, Quaternion.identity);
            BulletScript bullet = bulletObj.GetComponent<BulletScript>();
            
            // Передаем текущее направление движения (dir) как направление пули
            bullet.SetDirection(dir);
            fire = 0;
        }
        fire += 1;
    }

    private void Move()
    {
        transform.position = Vector3.MoveTowards(transform.position, transform.position + dir, moveSpeed * Time.deltaTime);
    }

    // --- РЕАЛИЗАЦИЯ УРОНА ---

    public override void GetDamage()
    {
        hp -= 1; // Уменьшаем здоровье
        Debug.Log("Дроид получил урон! Осталось HP: " + hp);

        // Можно запустить анимацию получения урона, если она есть
        // anim.SetTrigger("takeDamage"); 

        if (hp <= 0)
        {
            Die();
        }
    }

    public override void Die()
    {
        Debug.Log("Дроид уничтожен!");
        // Здесь можно спавнить эффект взрыва перед удалением
        Destroy(gameObject);
    }
}