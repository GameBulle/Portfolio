using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bolt_Ring : Weapon
{
    private void Update()
    {
        transform.Rotate(Vector3.back * speed * Time.deltaTime);
    }

    public override void Initialize(WeaponData data)
    {
        base.Initialize(data);
        ArrangeWeapon();
    }

    public override void LevelUp(float damage, float speed, int per, int count, float knockback)
    {
        base.LevelUp(damage, speed, per, count, knockback);
        this.damage += base_damage * damage;
        this.per += per;
        this.count += count;
        this.knockback_power += knockback;
        ArrangeWeapon();
    }

    protected override void ArrangeWeapon()
    {
        for (int i = 0; i < count; i++)
        {
            Transform bullet_tr;
            if (i < transform.childCount)
                bullet_tr = transform.GetChild(i);
            else
            {
                bullet_tr = GameManager.Instance.Pool.GetBullet(id).transform;
                bullet_tr.parent = transform;
            }

            bullet_tr.localPosition = Vector3.zero;
            bullet_tr.localRotation = Quaternion.identity;

            Vector3 rotate_vec = Vector3.forward * 360 * i / count;
            bullet_tr.Rotate(rotate_vec);
            bullet_tr.Translate(bullet_tr.up * 2f, Space.World);
            bullet_tr.GetComponent<Bullet>().Initialize(damage, per, knockback_power, shot_speed, Vector3.zero);
        }
    }

    protected override void SpeedUp(float value)
    {
        speed += base_speed * value;
    }
}
