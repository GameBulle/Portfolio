using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Samurai_Skill : Skill
{
    Collider2D player_collider;
    Vector3 prev_dir;
    Vector3 dir;
    Coroutine coroutine;

    private void Awake()
    {
        player_collider = GetComponentInParent<Collider2D>();
        gameObject.name = "Samurai_Skill";
        prev_dir = Vector3.left;
    }

    private void Update()
    {
        dir = GameManager.Instance.Player.Direction;
        if (dir != Vector3.zero)
            prev_dir = dir;
    }

    public override void UseSkill()
    {
        if(coroutine != null)
            StopCoroutine(coroutine);
        coroutine = StartCoroutine(OnSkill());
    }

    public IEnumerator OnSkill()
    {
        StartCoroutine(OnSkillMove());
        yield return null;
    }

    protected override void Fire()
    {
        if (dir == Vector3.zero)
            dir = prev_dir;
        Transform bullet_tr;
        bullet_tr = GameManager.Instance.Pool.GetSkill((int)CharacterData.CharacterType.Samurai).transform;
        bullet_tr.position = transform.position + dir * 5f;
        bullet_tr.rotation = Quaternion.FromToRotation(Vector3.left, dir);
        bullet_tr.gameObject.SetActive(true);
        bullet_tr.GetComponent<Bullet>().Initialize(damage, -10, 0, 0, Vector3.zero);
    }

    IEnumerator OnSkillMove()
    {
        GameManager.Instance.Player.UseSkill = true;
        yield return new WaitForSeconds(0.1f);

        if (dir == Vector3.zero)
            dir = prev_dir;
        Fire();
        Vector3 target_pos = GameManager.Instance.Player.transform.position + dir * 10f;
        player_collider.enabled = false;
        while (true)
        {
            GameManager.Instance.Player.transform.position = Vector3.MoveTowards(GameManager.Instance.Player.transform.position, target_pos, 100f * Time.fixedDeltaTime);
            if (Vector3.Distance(GameManager.Instance.Player.transform.position, target_pos) < 0.1f)
            {
                GameManager.Instance.Player.UseSkill = false;
                player_collider.enabled = true;
                break;
            }
            yield return new WaitForFixedUpdate();
        }
    }

    public override void StopSkill()
    {
        StopCoroutine(coroutine);
    }
}
