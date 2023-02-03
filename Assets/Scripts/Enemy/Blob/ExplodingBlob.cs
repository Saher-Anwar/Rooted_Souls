using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ExplodingBlob : EnemyAI, IBlob
{
    [Header("Explosion information")]
    [SerializeField]
    GameObject explosionVFX;
    [SerializeField]
    float knockbackForce = 2f;
    [SerializeField]
    float deathWaitTime = 1f;

    bool isExplosionTriggered = false;

    // Start is called before the first frame update
    void Start()
    {
        FindPlayerPos();
    }

    // Update is called once per frame
    void Update()
    {
        if (CheckForPlayer && !isExplosionTriggered)
        {
            isExplosionTriggered = true;
            Attack();
        }
    }

    public void Attack()
    {
        StartCoroutine(TriggerExplosion());
        // Do damage to player
    }

    IEnumerator TriggerExplosion()
    {
        // stop enemy movement
        GetComponent<EnemyBlobMovement>().enabled = false;
        
        yield return new WaitForSeconds(deathWaitTime);
        DisableUnnecessaryComponents();

        if(explosionVFX != null)
        {
            Debug.Log($"{explosionVFX.GetType()}");
            Instantiate(explosionVFX, transform.position, Quaternion.identity);
        }
        else
        {
            Debug.Log("No explosion VFX allocated");
        }

        ApplyExplosionKnockback();

        yield return new WaitForSeconds(explosionVFX.GetComponent<ParticleSystem>().main.duration);
        Death();
    }

    void ApplyExplosionKnockback()
    {
        if (!CheckForPlayer) return;
        Vector2 knockbackDir = (playerPos.position - transform.position).normalized;
        Rigidbody2D rigidbody2D = playerPos.gameObject.GetComponent<Rigidbody2D>();
        rigidbody2D.AddForce(knockbackDir * knockbackForce, ForceMode2D.Impulse);
    }

    public void Death(float deathWaitTime = 0)
    {
        Destroy(gameObject, deathWaitTime);
    }

    public void TakeDamage(float damage)
    {
        currHealth -= damage;

        if(currHealth < 0)
        {
            Death();
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position + offset, playerDetectionRadius);
    }

    void DisableUnnecessaryComponents()
    {
        GetComponent<BoxCollider2D>().enabled = false;
        GetComponent<SpriteRenderer>().enabled = false;
    }
}
