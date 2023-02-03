using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    
    // Initializing required variables
    public float moveDirection;
    public float moveSpeed = 7f;
    public float jumpForce = 15f;
    public float jumpFloat = 1f;
    public float checkRadius = 0.5f;
    // True meaning right, false meaning left
    public bool faceDirection = true;
    

    [SerializeField] private Rigidbody2D rigidBody2D;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;

    // Update is called once per frame
    void Update()
    {
        moveDirection = Input.GetAxis("Horizontal");
        changeDirection();

        // Jump Component
        // Allow jump if grounded and add force equal to jumpForce
        if (Input.GetButtonDown("Jump") && isGrounded())
        {
            rigidBody2D.velocity = new Vector2(rigidBody2D.velocity.x, jumpForce);
        }
        // If jump button is released and velocity is greater than 0, reduce velocity to add float effect
        if (Input.GetButtonUp("Jump") && rigidBody2D.velocity.y > 0f)
        {
            rigidBody2D.velocity = new Vector2(rigidBody2D.velocity.x, rigidBody2D.velocity.y * jumpFloat);
        }
    }

    void FixedUpdate()
    {
        rigidBody2D.velocity = new Vector2(moveDirection * moveSpeed, rigidBody2D.velocity.y);
    }

    // Method to change direction when face and move direction are not same
    private void changeDirection()
    {
        if (faceDirection && moveDirection < 0f || !faceDirection && moveDirection > 0f)
        {
            faceDirection = !faceDirection;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }

    private bool isGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, checkRadius, groundLayer);
    }

    void OnDrawGizmosSelected()
    {
        // Display the ground checking radius when selected
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(groundCheck.position, checkRadius);
    }

}
