using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dark_Warrior : Boss
{
    [SerializeField] Transform attack_pos;

    protected override IEnumerator OnAttack1()
    {
        anim.SetTrigger("Attack1_Ready");
        Attack1Ready();
        yield return new WaitForSeconds(1.5f);
        anim.SetTrigger("Attack1");
        Attack1();
    }

    protected override IEnumerator OnAttack2()
    {
        anim.SetTrigger("Attack2_Ready");
        Attack2Ready();
        yield return new WaitForSeconds(2f);
        anim.SetTrigger("Attack2");
        Attack2();
    }

    protected override void Attack1Ready()
    {
        attack_dir = (GameManager.Instance.Player.transform.position - attack_pos.position).normalized;

        lineRenderer.enabled = true;
        lineRenderer.startWidth = 1f;
        lineRenderer.positionCount = 6;
        lineRenderer.SetPosition(0, attack_pos.position);
        lineRenderer.SetPosition(1, attack_pos.position + attack_dir * 50f);
        lineRenderer.SetPosition(2, attack_pos.position);
        lineRenderer.SetPosition(3, Quaternion.AngleAxis(15f, Vector3.forward) * (attack_pos.position + attack_dir * 50f));
        lineRenderer.SetPosition(4, attack_pos.position);
        lineRenderer.SetPosition(5, Quaternion.AngleAxis(-15f, Vector3.forward) * (attack_pos.position + attack_dir * 50f));
    }

    protected override void Attack1()
    {
        lineRenderer.enabled = false;
        for (int i = 0; i < 3; i++)
        {
            Bullet bullet;
            bullet = GameManager.Instance.Pool.GetBullet(12);
            bullet.transform.position = attack_pos.position;
            bullet.transform.rotation = Quaternion.FromToRotation(Vector3.up, attack_dir);
            bullet.gameObject.SetActive(true);
            Vector3 axis = Vector3.zero;
            switch(i)
            {
                case 0:
                    axis = attack_dir;
                    break;
                case 1:
                    axis = Quaternion.AngleAxis(15f, Vector3.forward) * attack_dir; 
                    break;
                case 2:
                    axis = Quaternion.AngleAxis(-15f, Vector3.forward) * attack_dir;
                    break;
            }
            bullet.Initialize(damage, 0, 0, 30f, axis);
            bullet.transform.localScale = Vector3.one * 2;
        }
    }

    protected override void Attack2Ready()
    {
        attack_dir = (GameManager.Instance.Player.transform.position - attack_pos.position).normalized;

        lineRenderer.enabled = true;
        lineRenderer.startWidth = 4f;
        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0, attack_pos.position);
        lineRenderer.SetPosition(1, attack_pos.position + attack_dir * 50f);
    }

    protected override void Attack2()
    {
        lineRenderer.enabled = false;
        Bullet bullet;
        bullet = GameManager.Instance.Pool.GetBullet(12);
        bullet.transform.position = attack_pos.position;
        bullet.transform.rotation = Quaternion.FromToRotation(Vector3.up, attack_dir);
        bullet.gameObject.SetActive(true);
        bullet.Initialize(damage, 0, 0, 20f, attack_dir);
        bullet.transform.localScale = Vector3.one * 8;
    }

    public override void Die()
    {
        GameManager.Instance.IsSpawnBoss = false;
        StartCoroutine(OnDie());
    }

    IEnumerator OnDie()
    {
        if (coroutine != null)
            StopCoroutine(coroutine);
        coll.enabled = false;
        isDie = true;
        lineRenderer.enabled = false;
        anim.SetTrigger("Die");
        yield return new WaitForSeconds(2.5f);
        gameObject.SetActive(false);
    }

    public override bool CheckSpawn() { return (!isSpawn && GameManager.Instance.GameTime >= 180); }
}
