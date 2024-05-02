using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword_Blow : Weapon
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
        Vector3 random_pos;
        Vector3 player_pos = GameManager.Instance.Player.transform.position;

        for (int i = 0; i < count; i++)
        {
            random_pos = new Vector3(Random.Range(-16f, 16f) + player_pos.x, Random.Range(-10f, 10f) + player_pos.y, 0f);
            Transform bullet_tr;
            bullet_tr = GameManager.Instance.Pool.GetBullet(id).transform;
            bullet_tr.position = random_pos;
            bullet_tr.gameObject.SetActive(true);
            bullet_tr.GetComponent<Bullet>().Initialize(damage, per, knockback_power, shot_speed, Vector3.zero);
        }
        AudioManager.Instance.PlaySFX("Sword_Blow");
    }

    protected override void SpeedUp(float value)
    {
        speed -= base_speed * value;
    }
}
