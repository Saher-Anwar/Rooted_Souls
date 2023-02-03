using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    // Initializing required variables
    private float horizontal;
    private float speed = 5f;
    private float jumpForce = 10f;
    // True meaning right, false meaning left
    private bool movementDirection = true;

    [SerializeField] private Rigidbody2D rigidBody2D;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {

    }

}
