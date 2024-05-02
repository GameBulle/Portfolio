using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Skill : MonoBehaviour
{
    protected float damage;
    public void Initialize(float damage)
    {
        this.damage = damage;
    }
    public abstract void UseSkill();
    protected abstract void Fire();

    public virtual void StopSkill() { }
}
