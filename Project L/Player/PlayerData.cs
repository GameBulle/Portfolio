using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using UnityEngine;

[CreateAssetMenu(fileName = "Player Data", menuName = "ScriptableObject/Player Data", order = 1)]
public class PlayerData : ScriptableObject
{
    [Header("Player Stats")]
    [SerializeField] float attack = 0f;
    [SerializeField] float defence = 0f;
    [SerializeField] float hp = 0f;
    [SerializeField] float mp = 0f;
    [SerializeField] float sp = 0f;
    [SerializeField] float gp = 0f;
    [SerializeField] float nextLevelEXP = 0f;

    [Header("Target Layer")]
    [SerializeField] LayerMask targetLayer;

    [Header("Player Move Speed")]
    [SerializeField] float walkSpeed = 0f;
    [SerializeField] float runSpeed = 0f;
    [SerializeField] float guardSpeed = 0f;
    [SerializeField] float evadeSpeed = 0f;

    [Header("Range")]
    [SerializeField] float lockOnRange = 0f;

    [Header("Level UP Plus Status")]
    [SerializeField] float plusDamage = 0f;
    [SerializeField] float plusDefence = 0f;
    [SerializeField] float plusHP = 0f;
    [SerializeField] float plusMP = 0f;
    [SerializeField] float plusRecoveryMP = 0f;
    [SerializeField] float plusRecoverySP = 0f;

    public float Attack => attack;
    public float Defence => defence;
    public float HP => hp;
    public float MP => mp;
    public float SP => sp;
    public float GP => gp;
    public float NextLevelEXP => nextLevelEXP;

    public LayerMask TargetLayer => targetLayer;

    public float WalkSpeed => walkSpeed;
    public float RunSpeed => runSpeed;
    public float GuardSpeed => guardSpeed;
    public float EvadeSpeed => evadeSpeed;

    public float LockOnRange => lockOnRange;

    public float PlusDamage => plusDamage;
    public float PlusDefence => plusDefence;
    public float PlusHP => plusHP;
    public float PlusMP => plusMP;
    public float PlusRecoveryMP => plusRecoveryMP;
    public float PlusRecoverySP => plusRecoverySP;
}
public class PlayerStateData : ScriptableObject
{
    public IState<Player> IdleState { get; private set; }
    public IState<Player> RunState { get; private set; }
    public IState<Player> EvadeState { get; private set; }
    public IState<Player> GuardState { get; private set; }
    public IState<Player> AttackState { get; private set; }
    public IState<Player> DamagedState { get; private set; }
    public IState<Player> DieState { get; private set; }
    public IState<Player> GuardBreakState { get; private set; }
    public IState<Player> SkillState { get; private set; }
    public IState<Player> ExecuteState { get; private set; }
    public IState<Player> ChargeState { get; private set; }
    public IState<Player> ChargeAttackState { get; private set; }
    public IState<Player> RushState { get; private set; }
    public void SetData(IState<Player> idle, IState<Player> run, IState<Player> evade, IState<Player> guard, IState<Player> attack, IState<Player> damaged, IState<Player> die, IState<Player> guardBreak, IState<Player> skill, IState<Player> execute, IState<Player> charge, IState<Player> chargeAttack, IState<Player> rushState)
    {
        IdleState = idle;
        RunState = run;
        EvadeState = evade;
        GuardState = guard;
        AttackState = attack;
        DamagedState = damaged;
        DieState = die;
        GuardBreakState = guardBreak;
        SkillState = skill;
        ExecuteState = execute;
        ChargeState = charge;
        ChargeAttackState = chargeAttack;
        RushState = rushState;
    }
}
