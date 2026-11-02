using System;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;

using UnityEngine;

public class Movement : MonoBehaviour
{
    // Movement
    [SerializeField] private float MoveSpeed = 8f;
    [SerializeField] private float JumpForce = 16f;
    [SerializeField] private float MaxFallSpeed = -25f;
    [SerializeField] private float accelerationTime = 0.1f;
    [SerializeField] private float decelerationTime = 0.1f;

    // Jumping
    [SerializeField] private float coyoteTime = 0.15f;
    [SerializeField] private float jumpBufferTime = 0.15f;
    [SerializeField] private float variablejumpHeight = 0.5f;
    [SerializeField] private float fallMultiplier = 2.5f;
    [SerializeField] private float lowJumpMultiplier = 2f;

    //ground detection
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayerMask;
    [SerializeField] private float groundCheckRadius = 0.2f;
    [SerializeField] private float groundCheckDistance = 0.1f;

    //Components
    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    // movement state 
    private float horizontalInput;
    private bool isGrounded;
    private bool wasGrounded;
    private float velocityXSmoothing;

    //jump state
    private bool jumpInputDown;
    private bool jumpInputHeld;
    private float coyoteTimeCounter;
    private float jumpbuffercounter;
    private bool isJumping;



    // animation state
    private bool facingright = true;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        rb.freezeRotation = true;
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        rb.interpolation = RigidbodyInterpolation2D.Interpolate;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    private void Update()
    {
        handleInput();
        CheckGroundStatus();
        updateTimers();
        handleJumping();
         // UpdateAnimations();
    }

    private void FixedUpdate()
    {
        applyMovement();
        applyJumpPhysics();
        ClampFallSpeed();

    }

    private void handleInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        jumpInputDown = Input.GetButtonDown("Jump");
        jumpInputHeld = Input.GetButton("Jump");
        if (jumpInputDown)
        {
            jumpbuffercounter = jumpBufferTime;

        }
        else
        {
            jumpbuffercounter -= Time.deltaTime;
        }
    }

    private void CheckGroundStatus()
    {
        wasGrounded = isGrounded;

        Vector2 groundCheckPosition = (Vector2)groundCheck.position;
        isGrounded = Physics2D.OverlapCircle(groundCheckPosition, groundCheckRadius, groundLayerMask);

        if (!isGrounded)
        {
            RaycastHit2D hit = Physics2D.Raycast(
                groundCheckPosition,
                Vector2.down,
                groundCheckDistance,
                groundLayerMask);
        }

        if (isGrounded && !wasGrounded)
        {
            isJumping = false;
            // Onlanding();
        }

    }
    private void updateTimers()
    {
        if (isGrounded)
        {
            coyoteTimeCounter = coyoteTime;
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }
    }
    private void handleJumping()
    {
        bool canJump = (isGrounded || coyoteTimeCounter > 0f) && jumpbuffercounter > 0f;

        if (canJump && !isJumping)
        {
            PerformJump();
        }

        if (!jumpInputHeld && rb.linearVelocity.y > 0f && isJumping)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * variablejumpHeight);
            isJumping = false;
        }
    }

    private void PerformJump()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, JumpForce);

        isJumping = true;
        coyoteTimeCounter = 0f;
        jumpbuffercounter = 0f;

        //  animator.SetTrigger("Jump");
        // ParticleSystem jumpEffect = Instantiate(jumpParticlePrefab, transform.position, Quaternion.identity);
    }

    private void applyMovement()
    {
        float targetVelocityX = horizontalInput * MoveSpeed;
        float smoothTime = (horizontalInput != 0) ? accelerationTime : decelerationTime;

        float newVelocityX = Mathf.SmoothDamp(
            rb.linearVelocity.x,
            targetVelocityX,
            ref velocityXSmoothing,
            smoothTime
            );

        rb.linearVelocity = new Vector2(newVelocityX, rb.linearVelocity.y);
        if (horizontalInput > 0 && !facingright)
        {
            Flip();
        }
        else if (horizontalInput < 0 && facingright)
        {
            Flip();
        }
        else if (horizontalInput < 0 && facingright)
        {
            Flip();
        }

    }
    private void applyJumpPhysics()
    {
        if (rb.linearVelocity.y < 0)
        {
            rb.linearVelocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.fixedDeltaTime;
        }

        else if (rb.linearVelocity.y > 0 && !jumpInputHeld)
        {
            rb.linearVelocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.fixedDeltaTime;
        }

    }
    private void ClampFallSpeed()
    {
        if (rb.linearVelocity.y < MaxFallSpeed)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, MaxFallSpeed);
        }
    }
    private void Flip()
    {
        facingright = !facingright;
        spriteRenderer.flipX = !spriteRenderer.flipX;
    }

    /*private void OnLanding()
    {

    }
    */
    private void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = isGrounded ? Color.green : Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
            Gizmos.DrawLine(groundCheck.position,
                           groundCheck.position + Vector3.down * groundCheckDistance);
        }

    }

}



