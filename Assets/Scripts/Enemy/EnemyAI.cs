using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [SerializeField, OptionalField]
    ParticleSystem hitEffect;
    [SerializeField, OptionalField]
    ParticleSystem deathEffect;
    [SerializeField]
    float maxHealth;
    [SerializeField]
    protected float playerDetectionRadius;
    [SerializeField]
    LayerMask playerLayer;

    protected float currHealth;
    protected Transform playerPos;

    protected void FindPlayerPos()
    {
        playerPos = GameObject.FindGameObjectWithTag("Player").transform;
    }

    protected bool CheckForPlayer => Physics2D.OverlapCircle(transform.position, playerDetectionRadius, playerLayer);
}
