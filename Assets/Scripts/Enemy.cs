using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    //health of the enemys
    public int health = 100;
    public int points = 100;

    //enemy move speed
    public float speed;

    // public GameObject deathAnimation;

    //move direction
    private int direction = 1;

    float raycastDistance = 2f;
    protected float raycastOffset = 0.5f;

    Rigidbody2D rb;
    SpriteRenderer sr;
    public Animator anim;
    public BoxCollider2D bc;
    public bool dead = false;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        bc = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //check if enemy is grounded
        if (IsGrounded(raycastOffset) == false)
        {
            //go left
            direction = -1;
        }
        if (IsGrounded(-raycastOffset) == false)
        {
            //go right
            direction = 1;
        }
        if (!dead)
        {
            print("moving");
            Move(direction);
        }
        else
        {
            rb.velocity = new Vector2(0,0);
        }
    }

    //once bullet hit enemy, the enemy lose health
    public void TakeDamage(int damage)
    {
        health -= damage;

        if (health <= 0)
        {
            Die();
        }
    }

    //enemy death
    void Die()
    {
        //show deatheffect
        // Instantiate(deathAnimation, transform.position, Quaternion.identity);
        // Destroy(gameObject);
    }

    //set the raycast to check if enemy reach the end of th platform
    protected bool IsGrounded(float offsetX)
    {
        //start of our raycast
        Vector2 origin = transform.position;
        origin.x += offsetX;

        //direction of our raycast
        Vector2 direction = Vector2.down;

        //store the hit-information in a variable
        RaycastHit2D hitInfo = Physics2D.Raycast(origin, direction, raycastDistance);


        //If there is a collider under enemy
        if (hitInfo.collider != null)
        {
            return true;
        }
        return false;
    }

    void Move(float horizontalInput)
    {
        //We set the velocity based on the input of the player
        //We set the y to rb.velocity.y, because if we set it to 0 our object does not move down with gravity
        rb.velocity = new Vector2(horizontalInput * speed, rb.velocity.y);
        sr = GetComponent<SpriteRenderer>();

        if (horizontalInput < 0)
        {
            //Flip sprite
            sr.flipX = false;
            //if moving right...
        }
        else if (horizontalInput > 0)
        {
            //Unflip sprite
            sr.flipX = true;
        }

    }
    private void DestroyEnemy()
    {
        Destroy(gameObject);
    }
}
