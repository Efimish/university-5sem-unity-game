using UnityEngine;

public class Anonimus : MonoBehaviour
{
    private float moveSpeed =3f;
    private int lives=5;
    private float jumpForce=1f;

    private bool isGrounded=false;

    private float vInput;
    private float hInput;


    private Rigidbody2D rb;
    private SpriteRenderer sprite;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sprite=GetComponentInChildren<SpriteRenderer>();
    }



    private void Run()
    {
        Vector3 dir= transform.right*Input.GetAxis("Horizontal");

        transform.position=Vector3.MoveTowards(transform.position, transform.position+dir,moveSpeed*Time.deltaTime);

        sprite.flipX=dir.x<0.0f;
    }

    private void Update()
    {
        if (Input.GetButton("Horizontal"))
        {
            Run();
        }
        if (Input.GetButton("Jump")&& isGrounded)
        {
            Jump();
        }
    }

    private void FixedUpdate()
    {
        CheckGround();
    }

    private void Jump()
    {
        rb.AddForce(transform.up*jumpForce,ForceMode2D.Impulse);
    }

    private void CheckGround()
    {
        Collider2D[] collider=Physics2D.OverlapCircleAll(transform.position, 0.3f);
        isGrounded=collider.Length>1;
    }



}
