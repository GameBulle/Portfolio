using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    protected float move_speed;
    protected Rigidbody2D rigid;
    protected Animator anim;
    protected SpriteRenderer spriteRenderer;
    protected Vector2 dir;
    protected float hit_delay_timer = 0f;
    protected float hp;
    protected float damage;

    public float Damage => damage;

    protected virtual void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        anim = GetComponentInChildren<Animator>();
    }

    protected virtual void Move()
    {
        rigid.MovePosition(rigid.position + move_speed * dir * Time.fixedDeltaTime);
        rigid.velocity = Vector2.zero;
    }

    public virtual void Die() { }
}
