using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
    public float HP { get; set; }
    public bool isDead { get; }

    int OnDamage(float atk, string HitPart = "null");
    void RestoreHealth(float value);
}