using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class Wheel_Sword : Weapon
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
            Transform weapon_tr;
            if (i < transform.childCount)
                weapon_tr = transform.GetChild(i);
            else
            {
                weapon_tr = GameManager.Instance.Pool.GetBullet(id).transform;
                weapon_tr.parent = transform;
            }

            weapon_tr.localPosition = Vector3.zero;
            weapon_tr.localRotation = Quaternion.identity;

            Vector3 rotate_vec = Vector3.forward * 360 * i / count;
            weapon_tr.Rotate(rotate_vec);
            weapon_tr.Translate(weapon_tr.up * 2.5f, Space.World);

            weapon_tr.GetComponent<Bullet>().Initialize(damage, per, knockback_power, 0f, Vector3.zero);
            weapon_tr.transform.localScale = scale;
        }
    }

    protected override void SpeedUp(float value)
    {
        speed += base_speed * value;
    }
}
