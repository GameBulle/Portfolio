using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Monster Data", menuName = "ScriptableObject/Monster Data", order = 1)]
public class MonsterData : ScriptableObject
{
    [Header("Monster ID")]
    [SerializeField] int id = 0;

    [Header("Monster Stats")]
    [SerializeField] float damage = 1f;
    [SerializeField] float moveSpeed = 1f;
    [SerializeField] float attackSpeed = 1f;
    [SerializeField] float stiffness = 1.5f;
    [SerializeField] float health = 1f;
    [SerializeField] float DamagedDelayTime = 0.1f;
    [SerializeField] float attackRange = 5f;

    [Header("Drop Stuff")]
    [SerializeField] int dropBonePiece = 0;
    [SerializeField] int dropIronPiece = 0;
    [SerializeField] int dropDarkMaterial = 0;

    [Header("Defense")]
    [SerializeField] int head = 1;
    [SerializeField] int body = 1;
    [SerializeField] int leg = 1;

    public int ID => id;
    public float Damage => damage;
    public float MoveSpeed => moveSpeed;
    public float AttackSpeed => attackSpeed;
    public float Health => health;
    public int DropBonePiece => dropBonePiece;
    public int DropIronPiece => dropIronPiece;
    public int DropDarkMaterial => dropDarkMaterial;
    public float Stiffness => stiffness;
    public float DelayTime => DamagedDelayTime;
    public float AttackRange => attackRange;

    public int Head => head;
    public int Body => body;
    public int Leg => leg;
}

public class MonsterStateData : ScriptableObject
{
    public IState<Monster> IdleState { get; private set; }
    public IState<Monster> MoveState { get; private set; }
    public IState<Monster> AttackState { get; private set; }
    public IState<Monster> DamagedState { get; private set; }
    public IState<Monster> DieState { get; private set; }

    public void SetData(IState<Monster> idle, IState<Monster> move, IState<Monster> attack, IState<Monster> damaged, IState<Monster> die)
    {
        IdleState = idle;
        MoveState = move;
        AttackState = attack;
        DamagedState = damaged;
        DieState = die;
    }
}