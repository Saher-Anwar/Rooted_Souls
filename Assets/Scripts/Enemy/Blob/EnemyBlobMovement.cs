using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBlobMovement : MonoBehaviour
{
    new Rigidbody2D rigidbody;
    [SerializeField]
    Transform groundCheck;
    [SerializeField]
    LayerMask groundLayer;
    
    [SerializeField]
    float jumpForce = 2f;
    [SerializeField]
    float moveForce = 1f;
    [SerializeField]
    float jumpReloadTimeInSec = 2f;
    [SerializeField]
    float fallingGravityScale = 2f;

    float elapsedTime = 0;
    Transform player;
    BlobAnimation blobAnimation;
    bool isFalling = false;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        player = GameObject.FindWithTag("Player").transform;
        blobAnimation = new(GetComponent<Animator>());
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Need blob to jump around Vector.up * jumpForce * Time.deltaTime
        if (isGrounded) {
            blobAnimation?.SetTrigger(BlobAnimation.Animation_States.Squat);
            rigidbody.gravityScale = 1f;
            elapsedTime += Time.deltaTime;
            
            if(elapsedTime > jumpReloadTimeInSec)
            {
                elapsedTime = 0;
                Jump();
            }
        }

        if(rigidbody.velocity.y < 0)
        {
            rigidbody.gravityScale = fallingGravityScale;
            isFalling = true;
        }

        if (isFalling)
        {
            blobAnimation?.SetTrigger(BlobAnimation.Animation_States.EndJump);
            isFalling = false;
        }
    }

    bool isGrounded => Physics2D.OverlapCapsule(groundCheck.position, new Vector3(.35f, .11f), CapsuleDirection2D.Horizontal, 0, groundLayer);

    void Jump()
    {
        Vector2 result = (Vector2.up * jumpForce) + (MoveTowardsPlayer() * moveForce);
        rigidbody.AddForce(result, ForceMode2D.Impulse);
        blobAnimation?.SetTrigger(BlobAnimation.Animation_States.StartJump);

        MoveTowardsPlayer();

    }

    private Vector2 MoveTowardsPlayer()
    {
        Vector2 relativePos = gameObject.transform.position - player.position;

        if(relativePos.x < 0)
        {
            // move to the right
            transform.rotation = Quaternion.Euler(0, 0, 0);
            return Vector2.right;
        }
        else
        {
            // move to the left
            transform.rotation = Quaternion.Euler(0, 180, 0);
            return Vector2.left;
        }
    }
}
