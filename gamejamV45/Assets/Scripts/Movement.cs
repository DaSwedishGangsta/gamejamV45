using System;
using System.Collections;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;

using UnityEngine;

public class Movement : MonoBehaviour
{

    // Movement
    [SerializeField] private float MoveSpeed = 8f;
    [SerializeField] private float sprintspeed = 12f;
    [SerializeField] private float JumpForce = 16f;
    [SerializeField] private float MaxFallSpeed = -25f;
    [SerializeField] private float accelerationTime = 0.15f;
    [SerializeField] private float decelerationTime = 0.15f;

    // Dash
    [SerializeField] private bool canDash = true;
    [SerializeField] private bool isDashing;
    [SerializeField] private float dashPower = 24f;
    [SerializeField] private float dashTime = 0.2f;
    [SerializeField] private float dashCooldown = 1f;
    private int dashDirection;
   //  [SerializeField] private TrailRenderer dashTrail;

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
    [SerializeField] private Animator animator;
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

    private void Awake() // när spelet startar så hämtar vi komponenterna som vi behöver för att kunna röra på karaktären.
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
        if (isDashing)
        {
            return; 
        }
        handleInput();
        CheckGroundStatus();
        updateTimers();
        handleJumping();
       
        shiftInputDown();
        handleDashing();
    }

    private void FixedUpdate()
    {
        if (isDashing) // När du använder dash så kan du inte röra dig åt något annat håll
        {
            return;
        }
        
        applyMovement();
        applyJumpPhysics();
        ClampFallSpeed();

    }

    private void handleInput()
    {
         // använder mig av unitys nya input system
        horizontalInput = Input.GetAxisRaw("Horizontal");
        jumpInputDown = Input.GetButtonDown("Jump");
        jumpInputHeld = Input.GetButton("Jump");
        // om vi rör oss så startar animationen för "walking". 
        if (horizontalInput != 0)
        {
            animator.SetBool("isWalking", true);

            // om vi rör oss snabbar än hastigheten 8 så börjar karaktären animationen för att springa, om vi inte gör det så avslutas sprint animationen  
            if (MoveSpeed > 8) 
            {
                animator.SetBool("isRunning", true);
            }
            else
            {
                animator.SetBool("isRunning", false);
            }
        }
        // om vi inte rör oss så blir "isWalking" false. 
        else
        {
            animator.SetBool("isWalking", false);
        }
        
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
        // kollar vart groundchecken befinner sig och definerar nör karkatären rör marken och när den inte gör det 
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
        // om vi är på marken så definerar spelet att vi inte hoppar 
        if (isGrounded && !wasGrounded) 
        {
            isJumping = false;
            // Onlanding();
        }

    }
    private void updateTimers() // kontrollerar timers specifict för när man hoppar 
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
        bool canJump = (isGrounded || coyoteTimeCounter > 0f) && jumpbuffercounter > 0f; // om vi kan hoppa hoppa så måste cooldownen vara avklarad samt vara på marken 

        if (canJump && !isJumping) // om vi kan hoppa och inte äri processen av ett hopp.
        {
            PerformJump();
            
        }

        if (!jumpInputHeld && rb.linearVelocity.y > 0f && isJumping) // om vi inte håller in hopp och vi är påväg uppåt i hoppet så kan vi hoppa längre genom att hålla ner hopp knappen
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * variablejumpHeight);
            isJumping = false;
        }
    }

    
    private void PerformJump() // när vi hoppar så sätter vi velocityn på karaktären till jumpforce och sätter "isJumping" till true samt nollställer coyoteTimeCounter, jumpbuffercounter.
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
        // använder smoothtime/smoothdamp för att göra rörelsen mer flytande och inte så ryckig.
        float targetVelocityX = horizontalInput * MoveSpeed;
        float smoothTime = (horizontalInput != 0) ? accelerationTime : decelerationTime;

        float newVelocityX = Mathf.SmoothDamp( 
            rb.linearVelocity.x,
            targetVelocityX,
            ref velocityXSmoothing,
            smoothTime
            );
         
         // flippar sprites beroende på vilken riktning man rör sig åt, genom att 
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
    private void handleDashing() // om vi trycker på tab så aktiveras dashen och vi kan inte dasha igen förrän cooldownen är avklarad.
    {
        if (Input.GetKeyDown(KeyCode.Tab) && canDash)
        {
            StartCoroutine(dash());
        }
    }
    private void applyJumpPhysics() // om vi är påväg neråt så ökar vi gravitationen för att göra hoppet mer realistiskt, om vi håller in hopp knappen så kan vi hoppa högre.
    {
        if (rb.linearVelocity.y < 0)
        {
            rb.linearVelocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.fixedDeltaTime;
        }

        else if (rb.linearVelocity.y > 0 && jumpInputHeld)
        {
            rb.linearVelocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.fixedDeltaTime;
        }

    }
    private void ClampFallSpeed() // om vi faller så sätter vi en max hastighet för hur snabbt vi kan falla.
    {
        if (rb.linearVelocity.y < MaxFallSpeed)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, MaxFallSpeed);
        }
    }
    private void Flip() // flippar karaktären beroende på vilken riktning man rör sig åt.
    {
        facingright = !facingright;
        spriteRenderer.flipX = !spriteRenderer.flipX;
    }
    private void shiftInputDown() // om vi håller in shift så ökar vi hastigheten.
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            MoveSpeed = sprintspeed;
        }
        else
        {
            MoveSpeed = 8;
        }
    }
    private IEnumerator dash() // när vi dasha så sätter vi gravitationen till 0 och sätter velocityn till dashpower i den riktning vi rör oss åt, efter dashTime så återställer vi gravitationen och sätter isDashing till false, efter dashCooldown så kan vi dasha igen.
     {
         canDash = false;
         isDashing = true;
         float origionalGravity = rb.gravityScale;
         rb.gravityScale = 0f;
        Vector2 dashDirection = facingright ? Vector2.right : Vector2.left;
         rb.linearVelocity = new Vector2(dashPower * dashDirection.x, 0f);
         // dashTrail.emitting = true;
         yield return new WaitForSeconds(dashTime);
         rb.gravityScale = origionalGravity;
         isDashing = false;
         yield return new WaitForSeconds(dashCooldown);
         canDash = true;

     } 
    /*
    private void OnLanding()
    {

    }
    */
    private void OnDrawGizmosSelected() // ritar en gizmo som visar vart groundchecken befinner sig och om vi är på marken eller inte.
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



