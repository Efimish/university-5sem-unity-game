using UnityEngine;

public class Anonimus : Creature
{
    private float moveSpeed = 3f;
    private int lives = 5;
    private Vector3 dir; 
    private float jumpForce = 5f;

    private bool isAttacking = false;

    public int ShoutTimerKD = 1;

    public int ShoutTimer=0;

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

        
        if (Input.GetMouseButtonDown(1)&&(ShoutTimer > ShoutTimerKD))
        {
            ShoutTimer=0;
            Vector2 fireDirection;
            
            if (sprite.flipX)
            {
                
                fireDirection = Vector2.left; 
            }
            else
            {
                
                fireDirection = Vector2.right; 
            }
            
            GameObject bulletObj = Instantiate(Bullet, BulletPosition.position, Quaternion.identity);
            BulletScript bullet = bulletObj.GetComponent<BulletScript>();
            bullet.SetDirection(fireDirection);
        }
        ShoutTimer+=1;

        if (Input.GetMouseButtonDown(0) && CheckGround())
            {
                KatanaAttack();
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

    private void KatanaAttack()
    {
        isAttacking = true;
        State = States.katana_atack;
        // Можно добавить небольшую остановку персонажа при атаке:
        rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
    }

    // Этот метод мы вызовем через событие в самой анимации!
    public void OnAttackEnded()
    {
        isAttacking = false;
        State = States.idle; // Возвращаемся в покой
    }
}