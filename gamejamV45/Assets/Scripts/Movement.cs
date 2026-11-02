using System;
using Unity.VisualScripting;

using UnityEngine;

public class Movement : MonoBehaviour
{
    // Movement
    [SerializeField] private float MoveSpeed = 8f;
    [SerializeField] private float JumpForce = 16f ;
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
    [SerializeField] private Transform groundcheck;
    [SerializeField] private LayerMask groundLayerMask;
    [SerializeField] private float groundCheckRadius = 0.2f;
    [SerializeField] private float groundCheckDistance = 0.1f;

    //Components
    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    // movement state 
    private float horizontalInput;
    private bool isgrounded;
    private bool wasGrounded;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
