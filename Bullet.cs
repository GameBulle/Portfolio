using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    Rigidbody2D rigid;
    Collider2D coll;

    float damage;
    int per;
    float knockback;
    bool spin;

    public float Damage => damage;
    public float Knockback => knockback;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();
    }

    private void Update()
    {
        if(spin)
            transform.Rotate(Vector3.back * 500f * Time.deltaTime);
    }

    public void Initialize(float damage, int per, float knockback, float power, Vector3 dir, bool spin = false)
    {
        this.damage = damage;
        this.per = per;
        this.knockback = knockback;
        this.spin = spin;

        if (per >= 0)
            rigid.velocity = dir * power;
    }

    public void OnCollider()
    {
        coll.enabled = true;
    }

    public void OffObject()
    {
        gameObject.SetActive(false);
    }

    public void OffCollider()
    {
        coll.enabled = false;
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(transform.CompareTag("Bullet"))
        {
            if (per == -10)
                return;

            if(collision.CompareTag("Monster"))
            {
                per--;
                if(per < 0)
                    gameObject.SetActive(false);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (transform.CompareTag("Bullet_Melee"))
            return;

        if(collision.CompareTag("Bullet_Wall"))
            gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        transform.localScale = Vector3.one;
    }

    public void PlaySFX()
    {
        AudioManager.Instance.PlaySFX("Fire_Flame");
    }
}
