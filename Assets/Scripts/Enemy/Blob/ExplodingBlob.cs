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
        StartCoroutine(StartAttack());

    }

    IEnumerator StartAttack()
    {
        DisableMovement();
        yield return StartCoroutine(TriggerExplosion());
        Death();
    }

    IEnumerator TriggerExplosion()
    {        
        if (explosionVFX == null)
        {
            Debug.LogError("No explosion VFX allocated");
            yield return null;
        }

        Instantiate(explosionVFX, transform.position, Quaternion.identity, transform);
        yield return new WaitForSeconds(explosionVFX.GetComponent<ParticleSystem>().main.duration * 0.2f);

        DisableUnnecessaryComponents();
        ApplyExplosionKnockback();

        yield return new WaitForSeconds(explosionVFX.GetComponent<ParticleSystem>().main.duration * 0.5f);
    }

    void ApplyExplosionKnockback()
    {
        if (!CheckForPlayer) return;
        Vector2 knockbackDir = (playerPos.position - transform.position).normalized;
        Rigidbody2D rigidbody2D = playerPos.gameObject.GetComponent<Rigidbody2D>();
        rigidbody2D.AddForce(knockbackDir * knockbackForce, ForceMode2D.Impulse);

        // Do damage
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
        GetComponent<SpriteRenderer>().enabled = false;
    }

    void DisableMovement()
    {
        GetComponent<EnemyBlobMovement>().enabled = false;
    }
}
