using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(Rigidbody2D))]
public class Arrow : MonoBehaviour, IPoolingObject
{
    Rigidbody2D rigid;

    float damage = 0f;
    float penetration = 0f;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    //public void Initialize() { }

    public void SetPosition(Vector2 pos)
    {
        transform.position = pos;
    }

    public void OnShot(Vector2 dir, float damage, float penetration, float speed)
    {
        this.damage = damage;
        this.penetration = penetration;

        Quaternion rot = Quaternion.LookRotation(dir);
        transform.eulerAngles = -Vector3.forward * rot.eulerAngles.x;

        gameObject.SetActive(true);

        rigid.velocity = Vector2.zero;
        rigid.AddForce(dir * speed);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch(collision.tag)
        {
            case "DeadZone":
                ArrowSpawner.Instance.GiveBackItem(this);
                break;
            case "Monster":
                IDamageable target = collision.GetComponentInParent<IDamageable>();
                if (null != target)
                    penetration -= target.OnDamage(damage, collision.name);
                SoundMgr.Instance.HitSoundPlay();
                break;
        }

        if (penetration <= 0)
            ArrowSpawner.Instance.GiveBackItem(this);
    }

    public void ReturnBack()
    {

    }
}
