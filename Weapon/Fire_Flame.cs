using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire_Flame : Weapon
{
    List<Animator> anims;
    float timer = 0f;

    private void Awake()
    {
        anims = new();
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer >= speed)
            Fire();
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
        for (int i = 0; i < count * 2; i++)
        {
            if (i < anims.Count)
            {
                anims[i].GetComponent<Bullet>().Initialize(damage, -10, knockback_power, 0f, Vector3.zero);
                anims[i].SetTrigger("Stop");
                anims[i].transform.localScale = scale;
            }
            else
            {
                GameObject fire_flame = new GameObject("Fire Flame");
                fire_flame.transform.parent = transform;
                fire_flame.transform.localPosition = Vector3.zero;
                fire_flame.transform.localRotation = Quaternion.identity;
                for (int j = 0; j < 2; j++)
                {
                    Bullet bullet = GameManager.Instance.Pool.GetBullet(id);
                    bullet.transform.parent = fire_flame.transform;
                    bullet.transform.localPosition = Vector3.zero;
                    bullet.transform.localRotation = Quaternion.identity;
                    bullet.transform.Translate(new Vector3(5f, 0f, 0f), Space.World);
                    if (j == 1)
                    {
                        bullet.transform.Translate(new Vector3(-10f, 0f, 0f), Space.World);
                        bullet.transform.Rotate(new Vector3(0, 0, 180f));
                    }
                    bullet.Initialize(damage, per, knockback_power, shot_speed, Vector3.zero);
                    anims.Add(bullet.GetComponent<Animator>());
                    bullet.GetComponent<Collider2D>().enabled = false;
                    bullet.transform.localScale = scale;
                }

                Vector3 rotate = Vector3.zero;

                switch (count)
                {
                    case 2:
                        rotate = new Vector3(0, 0, 90f);
                        break;
                    case 3:
                        rotate = new Vector3(0, 0, 45f);
                        break;
                    case 4:
                        rotate = new Vector3(0, 0, -45f);
                        break;
                }

                fire_flame.transform.Rotate(rotate);
                i++;
            }
        }
    }
    protected override void Fire()
    {
        timer = 0f;
        for (int i = 0; i < anims.Count; i++)
            anims[i].SetTrigger("Fire");
    }

    protected override void SpeedUp(float value)
    {
        speed -= base_speed * value;
    }
}
