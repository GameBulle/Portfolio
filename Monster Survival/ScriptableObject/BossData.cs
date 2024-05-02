using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BossData", menuName = "Scriptable Object/BossData")]
public class BossData : ScriptableObject
{
    [SerializeField] float hp;
    [SerializeField] float attack_timer;
    [SerializeField] float damage;
    [SerializeField] float speed;

    public float HP => hp;
    public float AttackTimer => attack_timer;
    public float Damage => damage;
    public float Speed => speed;
}
