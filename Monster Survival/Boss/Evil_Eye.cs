using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Evil_Eye : Boss
{
    int attack1_count;
    protected override IEnumerator OnAttack1()
    {
        attack1_count = 0;
        lineRenderer.positionCount = 8;
        lineRenderer.startWidth = 4f;
        lineRenderer.loop = true;
        anim.SetBool("Attack", true);
        for (int i = 0; i < 2; i++)
        {
            Attack1Ready();
            yield return new WaitForSeconds(1f);
            Attack1();
        }
        anim.SetBool("Attack", false);
        IsAttack = false;
    }

    protected override IEnumerator OnAttack2()
    {
        anim.SetBool("Attack", true);
        Attack2Ready();
        yield return new WaitForSeconds(2.5f);
        Attack2();
    }

    protected override void Attack1Ready()
    {
        lineRenderer.enabled = true;
        switch (attack1_count)
        {
            case 0:
                lineRenderer.SetPosition(0, transform.position + Vector3.right * 50f);
                lineRenderer.SetPosition(1, transform.position);
                lineRenderer.SetPosition(2, transform.position + Vector3.left * 50f);
                lineRenderer.SetPosition(3, transform.position);
                lineRenderer.SetPosition(4, transform.position + Vector3.up * 50f);
                lineRenderer.SetPosition(5, transform.position);
                lineRenderer.SetPosition(6, transform.position + Vector3.down * 50f);
                lineRenderer.SetPosition(7, transform.position);
                break;
            case 1:
                lineRenderer.SetPosition(0, transform.position + new Vector3(1, 1, 0).normalized * 50f);
                lineRenderer.SetPosition(1, transform.position);
                lineRenderer.SetPosition(2, transform.position + new Vector3(-1, -1, 0).normalized * 50f);
                lineRenderer.SetPosition(3, transform.position);
                lineRenderer.SetPosition(4, transform.position + new Vector3(1, -1, 0).normalized * 50f);
                lineRenderer.SetPosition(5, transform.position);
                lineRenderer.SetPosition(6, transform.position + new Vector3(-1, 1, 0).normalized * 50f);
                lineRenderer.SetPosition(7, transform.position);
                break;
        }
        
    }

    protected override void Attack1()
    {
        lineRenderer.enabled = false;
        for(int i=0;i<4;i++)
        {
            Bullet bullet = GameManager.Instance.Pool.GetBullet(12);
            bullet.transform.position = transform.position;
            
            switch(attack1_count)
            {
                case 0:
                    attack_dir = Vector3.right;
                    break;
                case 1:
                    attack_dir = new Vector3(1, 1, 0).normalized;
                    break;
            }

            attack_dir = Quaternion.AngleAxis(90f * i, Vector3.forward) * attack_dir;
            bullet.transform.rotation = Quaternion.FromToRotation(Vector3.up, attack_dir);
            bullet.gameObject.SetActive(true);
            bullet.Initialize(damage, 0, 0, 25f, attack_dir);
            bullet.transform.localScale = Vector3.one * 8;
        }
        attack1_count++;
    }

    protected override void Attack2Ready()
    {
        attack_dir = (GameManager.Instance.Player.transform.position - transform.position).normalized;

        lineRenderer.enabled = true;
        lineRenderer.positionCount = 2;
        lineRenderer.startWidth = 9f;
        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, transform.position + attack_dir * 30f);
    }

    protected override void Attack2()
    {
        lineRenderer.enabled = false;
        StartCoroutine(OnAttack());
        StartCoroutine(OnFire());
    }

    IEnumerator OnAttack()
    {
        Vector3 target_pos = transform.position + attack_dir * 30f;
        while (true)
        {
            transform.position = Vector3.MoveTowards(transform.position, target_pos, 15f * Time.fixedDeltaTime);
            if (Vector3.Distance(transform.position, target_pos) < 0.1f)
                break;
            yield return new WaitForFixedUpdate();
        }
        anim.SetBool("Attack", false);
        IsAttack = false;
    }

    IEnumerator OnFire()
    {
        while(true)
        {
            if (!isAttack)
                break;
            Fire();
            yield return new WaitForSeconds(0.25f);
        }
    }

    public override void Die()
    {
        GameManager.Instance.IsSpawnBoss = false;
        StopAllCoroutines();
        StartCoroutine(OnDie());
    }

    IEnumerator OnDie()
    {
        coll.enabled = false;
        isDie = true;
        lineRenderer.enabled = false;
        anim.SetTrigger("Die");
        yield return new WaitForSeconds(2.5f);
        gameObject.SetActive(false);
        GameManager.Instance.GameVictory();
    }

    void Fire()
    {
        Vector3 fire_dir = (GameManager.Instance.Player.transform.position - transform.position).normalized;

        Bullet bullet = GameManager.Instance.Pool.GetBullet(12);
        bullet.transform.position = transform.position;
        bullet.transform.rotation = Quaternion.FromToRotation(Vector3.up, fire_dir);
        bullet.gameObject.SetActive(true);
        bullet.Initialize(damage, 0, 0, 30f, fire_dir);
        bullet.transform.localScale = Vector3.one * 2f;
    }

    public override bool CheckSpawn() { return (!isSpawn && GameManager.Instance.GameTime >= 360); }
}
