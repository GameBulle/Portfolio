using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spinning_Sword : Weapon
{
    float timer = 0f;

    private void Update()
    {
        transform.Rotate(Vector3.back * 500f * Time.deltaTime);
        timer += Time.deltaTime;
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
        if (GameManager.Instance.Player.Scanner.NearestTarget)
        {
            timer = 0f;
            Vector3 dir = (GameManager.Instance.Player.Scanner.NearestTarget.position - transform.position).normalized;

            Transform bullet_tr;
            bullet_tr = GameManager.Instance.Pool.GetBullet(id).transform;
            bullet_tr.position = transform.position;

            bullet_tr.gameObject.SetActive(true);
            bullet_tr.GetComponent<Bullet>().Initialize(damage, per, knockback_power, shot_speed, dir, true);
            bullet_tr.transform.localScale = scale;
            AudioManager.Instance.PlaySFX("Spinning_Sword");
        }
    }

    protected override void SpeedUp(float value)
    {
        speed -= base_speed * value;
    }
}
