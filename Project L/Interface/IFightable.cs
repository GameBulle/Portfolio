using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IFightable
{
    public float HPMax { get; set; }
    public float HP { get; set; }
    public bool isDead { get; }
 
    void OnDamage(float damage, Vector3 enemyPos);
    float OnDamage(float damage);
    void Attack();
    void Die();
}
