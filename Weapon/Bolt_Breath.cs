using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bolt_Breath : Weapon
{
    float timer = 0f;
    Vector3 prev_dir = Vector3.left;
    Vector3 dir;

    private void Update()
    {
        timer += Time.deltaTime;
        dir = GameManager.Instance.Player.Direction;
        if (dir != Vector3.zero)
            prev_dir = dir;
        if (timer > speed)
            Fire();
    }

    public override void LevelUp(float damage, float speed, int per, int count, float knockback)
    {
        base.LevelUp(damage, speed, per, count, knockback);
        this.damage += base_damage * damage;
        this.per += per;
        this.count += count;
        this.knockback_power += knockback;
    }

    protected override void Fire()
    {
        timer = 0f;
        if (dir == Vector3.zero)
            dir = prev_dir;

        switch (count)
        {
            case 1:
                {
                    Transform bullet_tr;
                    bullet_tr = GameManager.Instance.Pool.GetBullet(id).transform;
                    bullet_tr.position = transform.position;
                    bullet_tr.rotation = Quaternion.FromToRotation(Vector3.left, dir);
                    bullet_tr.gameObject.SetActive(true);
                    bullet_tr.GetComponent<Bullet>().Initialize(damage, per, knockback_power, shot_speed, dir);
                    bullet_tr.transform.localScale = scale;
                }
                break;
            case 3:
                for (int i = 0; i < count; i++)
                {
                    Transform bullet_tr;
                    bullet_tr = GameManager.Instance.Pool.GetBullet(id).transform;
                    bullet_tr.position = transform.position;
                    Vector3 fire_dir = Vector3.zero;
                    switch (i)
                    {
                        case 0:
                            fire_dir = Quaternion.AngleAxis(30, Vector3.forward) * dir;
                            break;
                        case 1:
                            fire_dir = dir;
                            break;
                        case 2:
                            fire_dir = Quaternion.AngleAxis(-30, Vector3.forward) * dir;
                            break;
                    }
                    bullet_tr.rotation = Quaternion.FromToRotation(Vector3.left, fire_dir);
                    bullet_tr.gameObject.SetActive(true);
                    bullet_tr.GetComponent<Bullet>().Initialize(damage, per, knockback_power, shot_speed, fire_dir);
                    bullet_tr.transform.localScale = scale;
                }
                break;
        }
        AudioManager.Instance.PlaySFX("Bolt_Breath");
    }

    protected override void SpeedUp(float value)
    {
        speed -= base_speed * value;
    }
}
