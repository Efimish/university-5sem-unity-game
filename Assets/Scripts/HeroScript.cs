using UnityEngine;

public class Anonimus : Creature
{
    private float moveSpeed = 3f;
    private int lives = 5;
    private Vector3 dir; 
    private float jumpForce = 5f;

    private bool isAttacking = false;

    // --- НОВЫЕ ПЕРЕМЕННЫЕ ДЛЯ АТАКИ ---
    public Transform attackPoint;      // Пустой объект перед игроком (центр удара)
    public float attackRange = 0.5f;   // Радиус удара
    public LayerMask enemyLayers;      // Слой, на котором находятся враги
    public int attackDamage = 1;       // Урон от катаны
    // ----------------------------------

    public int ShoutTimerKD = 1;
    public int ShoutTimer = 0;

    public static Anonimus Instance { get; set; }

    public bool isGrounded = false;
    private bool doubleJump = true;
    public LayerMask groundLayer;
    public Transform groundCheck;

    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer sprite;

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
        jump,
        katana_atack
    }

    private void Run()
    {
        if (Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer))
        {
            State = States.run;
        }

        dir = transform.right * Input.GetAxis("Horizontal");
        transform.position = Vector3.MoveTowards(transform.position, transform.position + dir, moveSpeed * Time.deltaTime);

        if (dir.x != 0) 
        {
            sprite.flipX = dir.x < 0.0f;
            
            // РАЗВОРОТ ТОЧКИ АТАКИ
            // Чтобы точка удара всегда была перед лицом, меняем её позицию
            if (attackPoint != null)
            {
                float posX = Mathf.Abs(attackPoint.localPosition.x);
                attackPoint.localPosition = new Vector2(sprite.flipX ? -posX : posX, attackPoint.localPosition.y);
            }
        }
    }

    private void Update()
    {
        if (isAttacking) return;
        
        if (Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer))
        {
            State = States.idle;
        }

        if (Input.GetButton("Horizontal"))
        {
            Run();
        }

        if (Input.GetKeyDown(KeyCode.Space) && CheckGround())
        {
            Jump();
            doubleJump = true;
        }
        else if (Input.GetKeyDown(KeyCode.Space) && doubleJump)
        {
            Jump();
            doubleJump = false;
        }

        if (Input.GetMouseButtonDown(1) && (ShoutTimer > ShoutTimerKD))
        {
            ShoutTimer = 0;
            Vector2 fireDirection = sprite.flipX ? Vector2.left : Vector2.right;
            
            GameObject bulletObj = Instantiate(Bullet, BulletPosition.position, Quaternion.identity);
            BulletScript bullet = bulletObj.GetComponent<BulletScript>();
            bullet.SetDirection(fireDirection);
        }
        ShoutTimer += 1;

        if (Input.GetMouseButtonDown(0) && CheckGround())
        {
            KatanaAttack();
        }
    }

    private void KatanaAttack()
    {
        isAttacking = true;
        State = States.katana_atack;
        rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);

        // ЛОГИКА НАНЕСЕНИЯ УРОНА
        // Создаем невидимый круг и собираем всех, кто попал в LayerMask enemyLayers
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        foreach (Collider2D enemy in hitEnemies)
        {
            // Проверяем, есть ли у врага скрипт, наследуемый от Creature (как и игрок)
            Creature enemyScript = enemy.GetComponent<Creature>();
            if (enemyScript != null)
            {
                enemyScript.GetDamage(); // Наносим урон
                Debug.Log("Попал по: " + enemy.name);
            }
        }
    }

    public void OnAttackEnded()
    {
        isAttacking = false;
        State = States.idle;
    }

    // ВИЗУАЛИЗАЦИЯ В РЕДАКТОРЕ
    // Рисует красный круг в Unity, чтобы было удобно настроить радиус атаки
    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }

    // --- ОСТАЛЬНЫЕ МЕТОДЫ (Jump, GetDamage и т.д.) БЕЗ ИЗМЕНЕНИЙ ---
    public override void GetDamage()
    {
        lives -= 1;
        if (lives <= 0) Die();
    }

    public override void Die() { Destroy(gameObject); }

    private void Jump()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f); 
        rb.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
    }

    private States State
    {
        get { return (States)anim.GetInteger("state"); }
        set { anim.SetInteger("state", (int)value); }
    }

    private bool CheckGround()
    {
        bool grounded = Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
        if (!grounded) State = States.jump;
        return grounded;
    }
}