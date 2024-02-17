using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Giant Spider Data", menuName = "ScriptableObject/Boss Data/Giant Spider", order = 0)]
public class GiantSpiderData : MonsterBaseData
{
    [Header("Boss Stats")]
    [SerializeField] float hp = 0f;
    [SerializeField] float grogyGauge = 0f;
    [SerializeField] float meleeAttack1 = 0f;
    [SerializeField] float meleeAttack2 = 0f;
    [SerializeField] float rangeAttack = 0f;
    [SerializeField] float searchRange = 0f;
    [SerializeField] float meleeAttackRange = 0f;
    [SerializeField] float exp = 0f;

    [Header("Move Speed")]
    [SerializeField] float walkSpeed = 0f;
    [SerializeField] float runSpeed = 0f;

    [Header("State Time")]
    [SerializeField] float maxIdleTime = 0f;
    [SerializeField] float minIdleTime = 0f;
    [SerializeField] float maxWalkTime = 0f;
    [SerializeField] float minWalkTime = 0f;
    [SerializeField] float normalAttackCoolTime = 0f;
    [SerializeField] float legAttackCoolTime = 0f;
    [SerializeField] float criticalAttackCoolTime = 0f;
    [SerializeField] float grogyTime = 0f;

    public float HP => hp;
    public float GrogyGauge => grogyGauge;
    public float MeleeAttack1 => meleeAttack1;
    public float MeleeAttack2 => meleeAttack2;
    public float RangeAttack => rangeAttack;
    public float SearchRange => searchRange;
    public float MeleeAttackRange => meleeAttackRange;
    public float Exp => exp;
    public float WalkSpeed => walkSpeed;
    public float RunSpeed => runSpeed;
    public float MaxIdleTime => maxIdleTime; 
    public float MinIdleTime => minIdleTime;
    public float MaxWalkTime => maxWalkTime;
    public float MinWalkTime => minWalkTime;
    public float NormalAttackCoolTime => normalAttackCoolTime;
    public float LegAttackCoolTime => legAttackCoolTime;
    public float CriticalAttackCoolTime => criticalAttackCoolTime;
    public float GrogyTime => grogyTime;
}

public class GiantStateData : ScriptableObject
{
    public IState<GiantSpider> IdleState { get; private set; }
    public IState<GiantSpider> WalkState { get; private set; }
    public IState<GiantSpider> BattleState { get; private set; }
    public IState<GiantSpider> CriticalAttackState { get; private set; }
    public IState<GiantSpider> ChaseState { get; private set; }
    public IState<GiantSpider> MeleeAttackState { get; private set; }
    public IState<GiantSpider> GrogyState { get; private set; }
    public IState<GiantSpider> DieState { get; private set; }
    public IState<GiantSpider> DamagedState { get; private set; }

    public void SetData(IState<GiantSpider> idleState, IState<GiantSpider> walkState, IState<GiantSpider> battleState, IState<GiantSpider> criticalAttackState, IState<GiantSpider> chaseState, IState<GiantSpider> meleeAttackState, IState<GiantSpider> grogyState, IState<GiantSpider> dieState, IState<GiantSpider> damagedState)
    {
        IdleState = idleState;
        WalkState = walkState;
        BattleState = battleState;
        CriticalAttackState = criticalAttackState;
        ChaseState = chaseState;
        MeleeAttackState = meleeAttackState;
        GrogyState = grogyState;
        DieState = dieState;
        DamagedState = damagedState;
    }
}
