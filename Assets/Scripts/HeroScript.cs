using UnityEngine;

public class Anonimus : Creature
{
    private float moveSpeed =3f;
    private int lives=5;
    private float jumpForce=5f;

    public static Anonimus Instance {get;set;}


    //настройка прыжка
    public bool isGrounded=false;

    private bool doubleJump=true;
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
        //обычный прыжок
        if (Input.GetKeyDown(KeyCode.Space)&&CheckGround())
        {
            Jump();
            //rb.AddForce(transform.up*jumpForce,ForceMode2D.Impulse);
            doubleJump=true;
            
        }
        else if(Input.GetKeyDown(KeyCode.Space)&&doubleJump)
        {
            //Debug.Log("yes");
            //rb.AddForce(transform.up*10,ForceMode2D.Impulse);
            Jump();
            doubleJump=false;
        }

        // if (!Input.GetButton("Jump")&&!CheckGround()&&!singleJump)
        // {
        //     doubleJump=true;
        // }
    }

    public override void GetDamage()
    {
        lives-=1;
        
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
