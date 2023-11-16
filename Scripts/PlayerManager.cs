using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] private float gravityScale = 1;
    [SerializeField] private float fallGravityScale = 4;
    [SerializeField] private LayerMask layer;

    public float speed;
    public float jumpForce;
    public Rigidbody2D rb;
    private BoxCollider2D coll;
    private bool isGrounded; //To check if the player touches the ground

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float x = Input.GetAxisRaw("Horizontal");

        Vector2 movement = new Vector2(x * speed, rb.velocity.y);

        rb.velocity = movement;
    }

    private void Update()
    {
        isGrounded = Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, .1f, layer);

        if (isGrounded == true && Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("jump");
            rb.velocity = new Vector2(rb.velocity.y, jumpForce);
        }

        if (rb.velocity.y > 0) rb.gravityScale = gravityScale;
        else rb.gravityScale = fallGravityScale;
    }

}
