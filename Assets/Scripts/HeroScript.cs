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
    private Animator anim;
    private SpriteRenderer sprite;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim=GetComponentInChildren<Animator>();
        sprite=GetComponentInChildren<SpriteRenderer>();
        Instance=this;  

    }

    public enum States
    {
        idle,
        run,
        jump
    }



    private void Run()
    {
        if (Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer))
        {
            State=States.run;
        }
        Vector3 dir= transform.right*Input.GetAxis("Horizontal");

        transform.position=Vector3.MoveTowards(transform.position, transform.position+dir,moveSpeed*Time.deltaTime);

        sprite.flipX=dir.x<0.0f;
    }

    private void Update()
    {

        if (Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer))
        {
            State=States.idle;
        }
        if (Input.GetButton("Horizontal"))
        {
            Run();
        }
        if (Input.GetButton("Jump")&&CheckGround())
        {

            Jump();

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


    private void Jump()
    {

        rb.AddForce(transform.up*jumpForce,ForceMode2D.Impulse);
    }

    private States State
    {
        get {
            return (States)anim.GetInteger("state");
            }
        set
        {
            anim.SetInteger("state",(int)value);
        }
    }

    private bool CheckGround() 
    {  
        if (!Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer))
        {
            State=States.jump;
        }
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }  
}
