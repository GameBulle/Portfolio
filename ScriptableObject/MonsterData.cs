using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MonsterData", menuName = "Scriptable Object/MonsterData")]
public class MonsterData : ScriptableObject
{
    public enum MonsterID { Goblin, Slime, Skeleton, Mushroom, Flying_Eye }

    [SerializeField] MonsterID monster_id;
    [SerializeField] float speed;
    [SerializeField] float health;
    [SerializeField] float damage;

    [Header("## Mushroom")]
    [SerializeField] float attack_speed;
    [SerializeField] float time_per_damage;
    [SerializeField] float time_per_hp;
    [SerializeField] float time_per;

    public MonsterID Monster => monster_id;
    public float Speed => speed;
    public float Health => health;
    public float Damage => damage;
    public float AttackSpeed => attack_speed;
    public float TimePerDamage => time_per_damage;
    public float TimeperHp => time_per_hp;
    public float TimePer => time_per;
}
