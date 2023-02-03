using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [SerializeField]
    float maxHealth;

    float currHealth;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(float damage)
    {
        currHealth -= damage;

        if(currHealth <= 0)
        {
            Death();
        }
    }

    public void Death()
    {
        Destroy(gameObject);
    }
}
