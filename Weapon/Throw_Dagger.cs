using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Throw_Dagger : Weapon
{
    float timer = 0f;

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer > speed)
            Fire();
    }

    public override void LevelUp(float damage, float speed,int per, int count, float knockback)
    {
        base.LevelUp(damage, speed,per, count, knockback);
        this.damage += base_damage * damage;
        this.per += per;
        this.count += count;
        this.knockback_power += knockback;
    }

    protected override void Fire()
    {
        timer = 0f;
        Vector3 dir = Vector3.zero;
        for (int i = 0; i < count; i++)
        {
            switch(i)
            {
                case 0:
                    dir = Vector3.right;
                    break;
                case 1:
                    dir = Vector3.up;
                    break;
                case 2:
                    dir = Quaternion.AngleAxis(45, Vector3.forward) * Vector3.right;
                    break;
                case 3:
                    dir = Quaternion.AngleAxis(-45,Vector3.forward) * Vector3.right;
                    break;
                default:
                    return;
            }

            for (int j = 0; j < 2; j++)
            {
                Transform bullet_tr;
                bullet_tr = GameManager.Instance.Pool.GetBullet(id).transform;
                bullet_tr.position = transform.position;

                if (j == 1)
                    dir *= -1;
                bullet_tr.rotation = Quaternion.FromToRotation(Vector3.up, dir);
                bullet_tr.gameObject.SetActive(true);
                bullet_tr.GetComponent<Bullet>().Initialize(damage, per, knockback_power, shot_speed, dir);
                bullet_tr.transform.localScale = scale;
            }
            AudioManager.Instance.PlaySFX("Throw_Dagger");
        }
    }

    protected override void SpeedUp(float value)
    {
        speed -= base_speed * value;
    }
}
