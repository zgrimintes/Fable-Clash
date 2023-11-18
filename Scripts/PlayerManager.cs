using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] private FighterManager fighterManager;
    [SerializeField] private float gravityScale = 1;
    [SerializeField] private float fallGravityScale = 4;
    [SerializeField] private LayerMask layer;
    [SerializeField] private float jumpForce;

    public GameObject textPrfb;
    private Rigidbody2D rb;
    private BoxCollider2D coll;
    private float speed;
    private bool isGrounded; //To check if the player touches the ground
    private float horizontal;
    private float playerYScale;
    public int mana;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<BoxCollider2D>();
        playerYScale = GetComponent<Transform>().localScale.y;
        LoadPlayer(fighterManager);
    }

    private void LoadPlayer(FighterManager data) //Function for loading the data from the ScriptableObject into the GameObject
    {
        WeightClass w_data = (WeightClass)data.w_Class;
        speed = w_data.speed;
        mana = data.mana;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        horizontal = Input.GetAxisRaw("Horizontal");

        Vector2 movement = new Vector2(horizontal * speed, rb.velocity.y);

        rb.velocity = movement;
    }

    private void Update()
    {
        Jump();

        if (horizontal == -1) gameObject.transform.localScale = new Vector3(-1, playerYScale, 1);
        else if (horizontal == 1) gameObject.transform.localScale = new Vector3(1, playerYScale, 1);

        if (Input.GetKeyDown(KeyCode.J))
        {
            fighterManager.normal_Attack(gameObject);
            textPrfb.GetComponentInChildren<TextMeshProUGUI>().text = "Mana: " + mana.ToString();
        }
    }

    private void Jump()
    {
        isGrounded = Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, .1f, layer);

        if (isGrounded == true && Input.GetKeyDown(KeyCode.Space))
        {
            rb.velocity = new Vector2(rb.velocity.y, jumpForce);
        }

        if (rb.velocity.y > 0) rb.gravityScale = gravityScale;
        else rb.gravityScale = fallGravityScale;
    }

}
