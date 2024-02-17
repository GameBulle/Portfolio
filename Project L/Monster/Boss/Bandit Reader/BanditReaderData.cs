using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Bandit Reader Data", menuName = "ScriptableObject/Boss Data/Bandit Reader", order = 1)]
public class BanditReaderData : MonsterBaseData
{
    [Header("Boss Stats")]
    [SerializeField] float hp = 0f;
    [SerializeField] float grogyGauge = 0f;
    [SerializeField] float normalAttack = 0f;
    [SerializeField] float attack2 = 0f;
    [SerializeField] float comboAttack = 0f;
    [SerializeField] float searchRange = 0f;
    [SerializeField] float attackRange = 0f;
    [SerializeField] float buffPercent = 0f;
    [SerializeField] float exp = 0f;

    [Header("Move Speed")]
    [SerializeField] float walkSpeed = 0f;
    [SerializeField] float runSpeed = 0f;

    [Header("State Time")]
    [SerializeField] float maxIdleTime = 0f;
    [SerializeField] float minIdleTime = 0f;
    [SerializeField] float maxWalkTime = 0f;
    [SerializeField] float minWalkTime = 0f;
    [SerializeField] float buffDuration = 0f;
    [SerializeField] float buffCoolTime = 0f;
    [SerializeField] float attackCoolTime = 0f;
    [SerializeField] float comboAttackCoolTime = 0f;
    [SerializeField] float jumpAttackCoolTime = 0f;
    [SerializeField] float grogyTime = 0f;

    public float Hp => hp;
    public float GrogyGauge => grogyGauge;
    public float NormalAttack => normalAttack;
    public float Attack2 => attack2;
    public float ComboAttack => comboAttack;
    public float SearchRange => searchRange;
    public float AttackRange => attackRange;
    public float BuffPercent => buffPercent;
    public float Exp => exp;
    public float WalkSpeed => walkSpeed;
    public float RunSpeed => runSpeed;
    public float MaxIdleTime => maxIdleTime;
    public float MinIdleTime => minIdleTime;
    public float MaxWalkTime => maxWalkTime;
    public float MinWalkTime => minWalkTime;
    public float BuffDuration => buffDuration;
    public float BuffCoolTime => buffCoolTime;
    public float AttackCoolTime => attackCoolTime;
    public float ComboAttackCoolTime => comboAttackCoolTime;
    public float JumpAttackCoolTime => jumpAttackCoolTime;
    public float GrogyTime => grogyTime;
}

public class BanditReaderStateData : ScriptableObject
{
    public IState<BanditReader> IdleState { get; private set; }
    public IState<BanditReader> WalkState { get; private set; }
    public IState<BanditReader> BattleState { get; private set; }
    public IState<BanditReader> BuffState { get; private set; }
    public IState<BanditReader> ChaseState { get; private set; }
    public IState<BanditReader> NormalAttackState { get; private set; }
    public IState<BanditReader> JumpAttackState { get; private set; }
    public IState<BanditReader> ComboAttackState { get; private set; }
    public IState<BanditReader> DieState { get; private set; }
    public IState<BanditReader> GrogyState { get; private set; }
    public IState<BanditReader> DamagedState { get; private set; }
    
    public void SetData(IState<BanditReader> idleState, IState<BanditReader> walkState,IState<BanditReader> battleState, IState<BanditReader> buffState, IState<BanditReader> chaseState, IState<BanditReader> normalAttackState, IState<BanditReader> jumpAttackState, IState<BanditReader> comboAttackState, IState<BanditReader> dieState, IState<BanditReader> grogyState, IState<BanditReader> damagedState)
    {
        IdleState = idleState;
        WalkState = walkState;
        BattleState = battleState;
        BuffState = buffState;
        ChaseState = chaseState;
        NormalAttackState = normalAttackState;
        JumpAttackState = jumpAttackState;
        ComboAttackState = comboAttackState;
        DieState = dieState;
        GrogyState = grogyState;
        DamagedState = damagedState;
    }
}
