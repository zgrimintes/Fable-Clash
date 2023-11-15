using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] private float gravityS = 1;
    [SerializeField] private float fallGravityS = 4;

    public float speed;
    public float jumpForce;
    public Rigidbody2D rb;
    private bool isGrounded; //To check if the player touches the ground

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float x = Input.GetAxisRaw("Horizontal");

        Vector2 movement = new Vector2(x, rb.velocity.y);

        rb.velocity = movement * speed;
    }

    private void Update()
    {
        /// Doesn't work!!! isGrounded = Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, .1f, 1);

        if (isGrounded == false && Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("jump");
            rb.AddForce( new Vector2(rb.velocity.y, jumpForce));
        }

        if (rb.velocity.y > 0) rb.gravityScale = gravityS;
        else rb.gravityScale = fallGravityS;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

}
