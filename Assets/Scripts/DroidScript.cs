using UnityEngine;

public class DroidScript : Creature
{
    private float moveSpeed=1.5f;

    private int timer;
    public int timerKD;
    private Vector3 dir;
    private SpriteRenderer sprite;


    private Rigidbody2D rb;
    private Animator anim;
    void Start()
    {
        dir=transform.right;
        rb = GetComponent<Rigidbody2D>();
        anim=GetComponentInChildren<Animator>();
        sprite=GetComponentInChildren<SpriteRenderer>();  
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        timer+=1;
        if (timer == timerKD)
        {
            dir=dir*-1f;
            timer=0;
            sprite.flipX=!sprite.flipX;
        }

    }

    private void Move()
    {
        transform.position=Vector3.MoveTowards(transform.position, transform.position+dir,moveSpeed*Time.deltaTime);

        //sprite.flipX=dir.x<0.0f;
    }
}
