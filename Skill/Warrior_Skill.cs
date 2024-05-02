using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Warrior_Skill : Skill
{
    Vector3 dir;
    Vector3 prev_dir = Vector3.left;
    int flip = -1;
    Coroutine coroutine;

    private void Awake()
    {
        gameObject.name = "Warrior_Skill";
    }

    private void Update()
    {
        dir = GameManager.Instance.Player.Direction;
        if (dir != Vector3.zero)
            prev_dir = dir;
    }

    public override void UseSkill()
    {
        if (coroutine != null)
            StopCoroutine(coroutine);
        coroutine = StartCoroutine(OnSkill());
    }

    public IEnumerator OnSkill()
    {
        while (true)
        {
            Fire();
            yield return new WaitForSeconds(0.25f);
        }
    }

    protected override void Fire()
    {
        if (dir == Vector3.zero)
            dir = prev_dir;
        Transform bullet_tr;
        bullet_tr = GameManager.Instance.Pool.GetSkill((int)CharacterData.CharacterType.Warrior).transform;
        bullet_tr.position = transform.position;
        bullet_tr.rotation = Quaternion.FromToRotation(Vector3.left, dir);
        bullet_tr.gameObject.SetActive(true);
        bullet_tr.GetComponent<Bullet>().Initialize(damage, 0, 0, 15f, dir);
        flip = -flip;
        bullet_tr.transform.localScale = new Vector3(1 , 1 * flip, 1);
        AudioManager.Instance.PlaySFX("Warrior_Skill");
    }

    public override void StopSkill()
    {
        if (coroutine != null)
            StopCoroutine(coroutine);
    }
}
