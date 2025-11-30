using UnityEngine;

public class Anonimus : Creature
{
    private float moveSpeed =3f;
    private int lives=5;
    private float jumpForce=0.5f;

    public static Anonimus Instance {get;set;}


    //настройка прыжка
    public bool isGrounded=false;
    public LayerMask groundLayer;

    public Transform groundCheck;

    private float vInput;
    private float hInput;


    private Rigidbody2D rb;
    private SpriteRenderer sprite;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sprite=GetComponentInChildren<SpriteRenderer>();
        Instance=this;  

    }



    private void Run()
    {
        Vector3 dir= transform.right*Input.GetAxis("Horizontal");

        transform.position=Vector3.MoveTowards(transform.position, transform.position+dir,moveSpeed*Time.deltaTime);

        sprite.flipX=dir.x<0.0f;
    }

    private void Update()
    {
        if (Input.anyKeyDown)
        {
            Run();
        }        
    }

    public override void GetDamage()
    {
        lives-=1;
        Debug.Log(lives);
        if (lives <= 0)
        {
            Die();
        }
    }

    public override void Die()
    {
        Destroy(this.gameObject);
    }

    // private void FixedUpdate()
    // {
    //     CheckGround();
    // }

    private void Jump()
    {

        rb.AddForce(transform.up*jumpForce,ForceMode2D.Impulse);
    }

    private bool CheckGround() 
    {  
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }  



}
