using UnityEngine;

public class DroidScript : Creature
{
    [SerializeField] private int hp = 3; 
    private float moveSpeed = 1.5f;

    private int timer;
    public int timerKD;
    private Vector3 dir;
    private SpriteRenderer sprite;

    public int fireKD;
    private int fire;

    
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
        
        
        timer += 1;
        if (timer >= timerKD) 
        {
            dir = dir * -1f;
            timer = 0;
            sprite.flipX = !sprite.flipX;
        }

        
        if (fire >= fireKD)
        {
            GameObject bulletObj = Instantiate(Bullet, BulletPosition.position, Quaternion.identity);
            BulletScript bullet = bulletObj.GetComponent<BulletScript>();
            
            
            bullet.SetDirection(dir);
            fire = 0;
        }
        fire += 1;
    }

    private void Move()
    {
        transform.position = Vector3.MoveTowards(transform.position, transform.position + dir, moveSpeed * Time.deltaTime);
    }

    

    public override void GetDamage()
    {
        hp -= 1; 
        Debug.Log("Дроид получил урон! Осталось HP: " + hp);

   

        if (hp <= 0)
        {
            Die();
        }
    }

    public override void Die()
    {
        Debug.Log("Дроид уничтожен!");
        
        Destroy(gameObject);
    }
}