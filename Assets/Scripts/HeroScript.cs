using UnityEngine;

public class Anonimus : Creature
{
    private float moveSpeed = 3f;
    private int lives = 5;

    // dir используется только для движения, поэтому она может быть локальной в Run, 
    // но оставляем ее здесь, так как она используется в Run()
    private Vector3 dir; 
    private float jumpForce = 5f;

    public static Anonimus Instance { get; set; }

    // настройка прыжка
    public bool isGrounded = false;
    private bool doubleJump = true;
    public LayerMask groundLayer;

    public Transform groundCheck;

    private float vInput;
    private float hInput;

    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer sprite;

    // стрельба
    public GameObject Bullet;
    public Transform BulletPosition;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
        sprite = GetComponentInChildren<SpriteRenderer>();
        Instance = this;
    }

    public enum States
    {
        idle,
        run,
        jump
    }

    private void Run()
    {
        // Логика состояния бега, когда игрок на земле
        if (Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer))
        {
            State = States.run;
        }

        // Вычисляем направление движения
        dir = transform.right * Input.GetAxis("Horizontal");

        // Движение
        transform.position = Vector3.MoveTowards(transform.position, transform.position + dir, moveSpeed * Time.deltaTime);

        // Отзеркаливание спрайта: если dir.x < 0.0f, спрайт смотрит влево (flipX = true)
        if (dir.x != 0) // Проверяем, чтобы избежать ошибок, если dir.x = 0
        {
            sprite.flipX = dir.x < 0.0f;
        }
    }

    private void Update()
    {
        // Проверка на землю для перехода в состояние Idle
        if (Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer))
        {
            State = States.idle;
        }

        if (Input.GetButton("Horizontal"))
        {
            Run();
        }

        // Обычный прыжок
        if (Input.GetKeyDown(KeyCode.Space) && CheckGround())
        {
            Jump();
            doubleJump = true;
        }
        // Двойной прыжок
        else if (Input.GetKeyDown(KeyCode.Space) && doubleJump)
        {
            Jump();
            doubleJump = false;
        }

        // --- ЛОГИКА СТРЕЛЬБЫ ---
        if (Input.GetMouseButtonDown(1))
        {
            // Определяем направление выстрела на основе отзеркаливания спрайта
            Vector2 fireDirection;
            
            if (sprite.flipX)
            {
                // Если спрайт отзеркален, значит игрок смотрит влево
                fireDirection = Vector2.left; // (-1, 0)
            }
            else
            {
                // Если спрайт не отзеркален, значит игрок смотрит вправо
                fireDirection = Vector2.right; // (1, 0)
            }
            
            GameObject bulletObj = Instantiate(Bullet, BulletPosition.position, Quaternion.identity);
            BulletScript bullet = bulletObj.GetComponent<BulletScript>();
            
            // Передаем правильное направление пуле
            bullet.SetDirection(fireDirection);
        }
    }

    public override void GetDamage()
    {
        lives -= 1;

        if (lives <= 0)
        {
            Die();
        }
    }

    public override void Die()
    {
        Destroy(this.gameObject);
    }

    private void Jump()
    {
        // Обнуляем вертикальную скорость перед прыжком, чтобы прыжки были последовательными
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f); 
        rb.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
    }

    private States State
    {
        get
        {
            return (States)anim.GetInteger("state");
        }
        set
        {
            anim.SetInteger("state", (int)value);
        }
    }

    private bool CheckGround()
    {
        if (!Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer))
        {
            State = States.jump;
        }
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }
}