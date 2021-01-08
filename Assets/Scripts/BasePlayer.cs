using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BasePlayer : MonoBehaviour
{
    [SerializeField]
    private SwordAttack attackPrefab;

    [SerializeField]
    private float fallThreshold = -10;

    public Action OnDeath;
    public Action ClimbState;
    public static BasePlayer instance;

    //public vars
    public float speed;
    public float jumpVelocity;
    public bool bClimbingMode;
    public Animator anim;
    public float verticalInput;
    public Rigidbody2D rb;
    public LayerMask IgnorePlayer;
    public GameObject highScore;
    public AudioClip footsteps;
    public AudioClip attacksound;

    //private vars
    private bool bFacingLeft;
    private SpriteRenderer sr;
    private float raycastDistance = 0.1f;
    protected float raycastOffset = 0.1f;
    private Vector3 startPos;
    private float horizontalInput;
    private bool deathTriggered = false;

    static int Score;
    string scorePrefix = "Score : ";
    AudioSource footStepSource;
    AudioSource attackSoundSorce;

    private void OnEnable()
    {
        instance = this;
        OnDeath += Death;
        ClimbState += SwitchToClimbingState;
    }

    private void SwitchToClimbingState()
    {
        bClimbingMode = true;
        anim.SetBool("bClimbingMode", true);

        //make the animator go to the climbing state and keep it there untilplayer is not climbing
        anim.SetBool("bClimbing", true);
        rb.gravityScale = 0;
    }

    private void ToggleAnimClimbable()
    {
        anim.SetBool("bClimbing", false);
    }

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        footStepSource = GetComponent<AudioSource>();
        attackSoundSorce = GetComponent<AudioSource>();

        //save the start pos for respawn
        startPos = transform.position + new Vector3(0, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        //doing is grounded all the time so it can detect moving platforms (Hack)
        IsGrounded(raycastOffset);

        Walking();

        Climbing();

        JumpOrAttack();

        CheckFallDeath();

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("MainMenu");
        }

    }

    private void CheckFallDeath()
    {
        if (transform.position.y < fallThreshold)
        {
            Death();
        }
    }

    private void Climbing()
    {
        //up down movement
        verticalInput = Input.GetAxisRaw("Vertical");
        anim.SetFloat("VerticalSpeed", Mathf.Abs(verticalInput));
        if (bClimbingMode == true)
        {
            rb.velocity = new Vector2(rb.velocity.x, verticalInput * speed);
        }
    }

    private void Walking()
    {
        //left right movement
        horizontalInput = Input.GetAxisRaw("Horizontal");

        //We set the velocity based on the input of the player
        //We set the y to rb.velocity.y, because if we set it to 0 our object does not move down with gravity
        rb.velocity = new Vector2(horizontalInput * speed, rb.velocity.y);

        //If moving left...
        if (horizontalInput < 0)
        {
            //Flip sprite
            sr.flipX = true;
            bFacingLeft = true;
            //if moving right...
        }
        else if (horizontalInput > 0)
        {
            //Unflip sprite
            sr.flipX = false;
            bFacingLeft = false;
        }

        //We send this information to the animator, which handles the transition between animations
        //We send the Absolute (= Always positive) value of horizontalInput, so even when Beario moves left, his animation plays
        anim.SetFloat("HorizontalSpeed", Mathf.Abs(horizontalInput));

    }

    private void JumpOrAttack()
    {
        //Jump
        //If Beario is grounded on the left OR the right side...
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Joystick1Button1))
        {
            if (Math.Abs(rb.velocity.x) > 0)
            {
                if (IsGrounded(-raycastOffset) || IsGrounded(raycastOffset))
                {
                    //Jump
                    anim.SetBool("bJump", true);
                    rb.velocity = new Vector2(rb.velocity.x, jumpVelocity);
                }
            }
            else
            {
                anim.SetBool("bAttack", true);
            }
        }
        else
        {
            anim.SetBool("bAttack", false);
            anim.SetBool("bJump", false);
        }
    }

    //Returns whether or not Beario is on the ground
    protected bool IsGrounded(float offsetX)
    {
        //The start of our raycast
        Vector2 origin = transform.position;
        origin.x += offsetX;

        //The direction of our raycast
        Vector2 direction = Vector2.down;

        //We do our raycast, and store the hit-information in a variable
        RaycastHit2D hitInfo = Physics2D.Raycast(origin, direction, raycastDistance);

        //This will draw a ray in the Unity Scene, which makes it easier to debug
        Debug.DrawRay(origin, direction * raycastDistance, Color.red, 10);

        //If there is a collider under Beario...
        if (hitInfo.collider != null)
        {
            //If the collider we are standing on, has a moving platform component...
            if (hitInfo.collider.GetComponent<MovingPlatform>() != null)
            {
                //If on a moving platform, become a child of it - so we move with the platform
                transform.SetParent(hitInfo.transform);
            }
            else if (hitInfo.collider.GetComponent<OneTimePlatform>() != null)
            {
                hitInfo.collider.GetComponent<OneTimePlatform>().Trigger();
            }
            else
            {
                //If not on a platform, unparent me.
                transform.SetParent(null);
            }

            return true;
        }

        //If there is NO collider under the Character, also unparent
        transform.SetParent(null);

        return false;
    }

    public void Death()
    {
        if (deathTriggered == false)
        {
            //play the death animation
            anim.SetTrigger("DeathTrigger");
            anim.SetBool("bDead", true);
            deathTriggered = true;
        }
    }
    private void Respawn()
    {
        SceneManager.LoadScene("DeathMenu");
        Score = 0;
        // transform.position = startPos;
        // anim.SetBool("bDead", false);
        // deathTriggered = false;
        // rb.velocity = new Vector2(0, 0);
    }

    private void Attack()
    {
        attackSoundSorce.PlayOneShot(attacksound);
        Vector3 pos = bFacingLeft ? transform.position + new Vector3(-.406f, 0, 0) : transform.position + new Vector3(.406f, 0, 0);
        SwordAttack sa = Instantiate(attackPrefab, pos, transform.rotation);
        sa.GetComponent<SpriteRenderer>().flipX = bFacingLeft;
        sa.leftDir = bFacingLeft;
    }

    public void AddScore(int value)
    {
        Score += value;
        highScore.GetComponentInParent<Text>().text = scorePrefix + Score;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<Enemy>() != null)
        {
            Respawn();
        }
        else if (other.GetComponent<Spikes>() != null)
        {
            Respawn();
        }
    }

    private void playFootstepSound()
    {
        footStepSource.PlayOneShot(footsteps);
    }
}