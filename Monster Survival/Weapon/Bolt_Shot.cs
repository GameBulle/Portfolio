using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bolt_Shot : Weapon
{
    float timer = 0f;
    Vector3 prev_dir = new Vector3(-1f, 0f, 0f);
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

        Transform bullet_tr;
        bullet_tr = GameManager.Instance.Pool.GetBullet(id).transform;
        bullet_tr.position = transform.position;
        bullet_tr.rotation = Quaternion.FromToRotation(Vector3.left, dir);
        bullet_tr.gameObject.SetActive(true);
        bullet_tr.transform.localScale = scale;
        bullet_tr.GetComponent<Bullet>().Initialize(damage, per, knockback_power, shot_speed, dir);
        bullet_tr.transform.localScale = scale;
        AudioManager.Instance.PlaySFX("Bolt_Shot");
    }

    protected override void SpeedUp(float value)
    {
        speed -= base_speed * value;
    }
}
