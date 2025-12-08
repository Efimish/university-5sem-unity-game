using UnityEngine;

public class BulletScript : MonoBehaviour
{
    public float speed = 0.05f;
    public Rigidbody2D rb;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.linearVelocity = transform.up * speed;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position=transform.position+new Vector3(speed,0,0);
    }
}
