using Enemy;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [Header("Player attributes")]
    [SerializeField]
    float attackDamage = 5f;
    [SerializeField]
    float attackCooldown = 1f;
    [SerializeField]
    float knockbackForce = 4f;

    Animator animator;

    [Header("Enemy Area Detection")]
    [SerializeField]
    Vector3 offset;
    [SerializeField]
    Vector3 size;
    [SerializeField]
    LayerMask enemyLayer;

    float elapsedTime = 0;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        elapsedTime += Time.deltaTime;
        if (Input.GetMouseButtonDown(0))
        {
            if (elapsedTime > attackCooldown)
            {
                elapsedTime = 0;
                DoAttack();
                // play attack animation
                animator.SetTrigger("IsAttacking");
            }
        }
    }

    void DoAttack()
    {
        foreach (Collider2D collider2D in CheckForEnemy())
        {
            
            IBlob blob = collider2D.gameObject.GetComponent<IBlob>();
            if(blob != null)
            {
                DoDamage(collider2D.gameObject);
            }
            

            Boss bossBlob = collider2D.gameObject.GetComponent<Boss>();
            bossBlob?.OnDamage((int)attackDamage);
        }
    }

    void DoDamage(GameObject blob)
    {
        blob.GetComponent<IBlob>().TakeDamage(attackDamage);
        ApplyKnockback(blob);
    }

    void ApplyKnockback(GameObject blob)
    {
        Rigidbody2D blobRb = blob.GetComponent<Rigidbody2D>();
        Vector2 direction = (blob.transform.position - transform.position).normalized;
        blobRb.AddForce(direction * knockbackForce, ForceMode2D.Impulse);
    }

    Collider2D[] CheckForEnemy()
    {
        bool direction = GetComponent<PlayerMovement>().faceDirection;
        if (direction)
        {
            // player is facing right

            return Physics2D.OverlapBoxAll(transform.localPosition + offset, size, 0, enemyLayer);
        }
        else
        { 
            return Physics2D.OverlapBoxAll(transform.localPosition + new Vector3(-offset.x, offset.y), size, 0, enemyLayer);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        bool direction = GetComponent<PlayerMovement>().faceDirection;

        if (direction)
        {
            // player is facing right
            Gizmos.DrawWireCube(transform.localPosition + offset, size);
        }
        else
        {
            Gizmos.DrawWireCube(transform.localPosition + new Vector3(-offset.x, offset.y), new Vector2(size.x, size.y));
        }

    }
}
