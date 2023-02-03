using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBlob
{
    void Attack();
    void Death();
    void TakeDamage(float damage);
}
