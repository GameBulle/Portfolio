using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shaman_Skill : Skill
{
    GameObject skill_object;

    private void Awake()
    {
        gameObject.name = "Shaman_Skill";
    }

    public override void UseSkill()
    {
        Fire();
    }

    protected override void Fire()
    {
        Transform bullet_tr;
        bullet_tr = GameManager.Instance.Pool.GetSkill((int)CharacterData.CharacterType.Shaman).transform;
        bullet_tr.position = transform.position;
        bullet_tr.rotation = Quaternion.identity;
        bullet_tr.gameObject.SetActive(true);
        bullet_tr.GetComponent<Bullet>().Initialize(damage, -10, 0, 0, Vector3.zero);
        skill_object = bullet_tr.gameObject;
        AudioManager.Instance.PlaySFX("Shaman_Skill");
    }

    public override void StopSkill()
    {
        skill_object.gameObject.SetActive(false);
    }
}
