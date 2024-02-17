using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Monster Data", menuName = "ScriptableObject/Monster Data", order = 2)]
public class MonsterData : MonsterBaseData
{
    [Header("Monster Stats")]
    [SerializeField] float hp = 0f;
    [SerializeField] float damage = 0f;
    [SerializeField] float attackDelay = 0f;
    [SerializeField] float attackRange = 0;
    [SerializeField] float searchRange = 0f;
    [SerializeField] float backWalkRange = 0f;
    [SerializeField] float maxWaringGauge = 0f;
    [SerializeField] float exp = 0f;
    [SerializeField] float findAngle = 0f;

    [Header("Monster Move Speed")]
    [SerializeField] float walkSpeed = 0f;
    [SerializeField] float runSpeed = 0f;

    [Header("State Time")]
    [SerializeField] float maxidleTime = 0f;
    [SerializeField] float minidleTime = 0f;
    [SerializeField] float maxwalkTime = 0f;
    [SerializeField] float minwalkTime = 0f;
    [SerializeField] float attackReadyTime = 0f;
    [SerializeField] float stiffness = 0f;

    [Header("Connect Drop Table key")]
    [SerializeField] int dropTableKey;

    public float HP => hp;
    public float Damage => damage;
    public float AttackDelay => attackDelay;
    public float AttackRange => attackRange;
    public float SearchRange => searchRange;
    public float BackWalkRange => backWalkRange;
    public float MaxWaringGauge => maxWaringGauge;
    public float EXP => exp;
    public float FindAngle => findAngle;  
    public float WalkSpeed => walkSpeed;
    public float RunSpeed => runSpeed;
    public float MaxIdleTime => maxidleTime;
    public float MinIdleTime => minidleTime;
    public float MaxWalkTime => maxwalkTime;
    public float MinWalkTime => minwalkTime;
    public float AttackReadyTime => attackReadyTime;
    public float Stiffness => stiffness;
    public int DropTableKey => dropTableKey;
}

public class MonsterStateData : ScriptableObject
{
    public IState<Monster> IdleState { get; private set; }
    public IState<Monster> WalkState { get; private set; }
    public IState<Monster> ChaseState { get; private set; }
    public IState<Monster> AttackState { get; private set; }
    public IState<Monster> DieState { get; private set; }
    public IState<Monster> DamagedState { get; private set; }
    public IState<Monster> AttackReadyState { get; private set; }
    public IState<Monster> FindObjectState { get; private set; }

    public IState<Monster> ExecutedState { get; private set; }

    public void SetData(IState<Monster> idleState, IState<Monster> walkState, IState<Monster> chaseState, IState<Monster> attackState, IState<Monster> dieState, IState<Monster> damagedState, IState<Monster> attackReadyState, IState<Monster> findObjectState, IState<Monster> executedState)
    {
        IdleState = idleState;
        WalkState = walkState;
        ChaseState = chaseState;
        AttackState = attackState;
        DieState = dieState;
        DamagedState = damagedState;
        AttackReadyState = attackReadyState;
        FindObjectState = findObjectState;
        ExecutedState = executedState;
    }
}
