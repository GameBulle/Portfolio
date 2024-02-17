using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Boss : Character, IFightable
{
    [Header("Function")]
    [SerializeField] protected NavMeshAgent agent;
    [SerializeField] protected MonsterAudio bossAudio;

    [SerializeField] protected Collider bossCollider;
    [SerializeField] protected GameObject minimapIcon;

    protected Rigidbody chaseTarget;
    protected Coroutine coroutine = null;
    public float HPMax { get; set; }
    public float HP { get; set; }
    public bool isDead { get { return HP <= 0; } }
    public float MaxGrogyGauge { get; set; }
    public float GrogyGauge { get; set; }
    public bool IsGrogy { get; set; }

    float idleTime = 0f;
    protected Vector2 minMaxIdleTime;
    float walkTime = 0f;
    protected Vector2 minMaxWalkTime;

    protected bool isBossUIActive;
    protected bool isAttack;

    public override void Initialize()
    {
        Stop();
        AnimStop();
        bossAudio.Initialize();

        isBossUIActive = false;
        isAttack = false;
    }

    public virtual void AnimStop() { }
    public virtual void WalkAnim() { }
    public virtual void Damaged() { }
    protected virtual void AttackTimers() { }

    public void SetIdleTime()
    {
        idleTime = 0;
        idleTime -= UnityEngine.Random.Range(minMaxIdleTime.x, minMaxIdleTime.y);
    }

    public void SetWalkTime()
    {
        walkTime = 0;
        walkTime -= UnityEngine.Random.Range(minMaxWalkTime.x, minMaxWalkTime.y);
    }

    public bool CheckIdleTime()
    {
        if (idleTime >= 0)
            return true;

        idleTime += updateTime;
        return false;
    }

    public bool CheckWalkTime()
    {
        if (walkTime >= 0)
            return true;

        walkTime += updateTime;
        return false;
    }

    public void GetRandomDir()
    {
        dir = new Vector3(UnityEngine.Random.Range(-1.0f, 1.0f), 0, UnityEngine.Random.Range(-1.0f, 1.0f)).normalized;

        transform.rotation = Quaternion.LookRotation(dir);
    }

    public void LookTarget()
    {
        transform.LookAt(chaseTarget.position);
    }

    public void ChaseTarget()
    {
        agent.SetDestination(chaseTarget.position);
    }

    public void ChaseTargetEnd()
    {
        AnimStop();
        agent.isStopped = true;
        agent.velocity = Vector3.zero;
    }

    public virtual void SetChaseTarget()
    {
        if (!chaseTarget)
            return;

        agent.acceleration = 40f;
        agent.isStopped = false;
    }

    public bool IsAttackBound()
    {
        float dist = DistanceTarget();
        if (0 > dist)
            return false;
        return (dist <= agent.stoppingDistance);
    }

    protected float DistanceTarget()
    {
        if (!chaseTarget)
            return -1.0f;
        return Vector3.Distance(chaseTarget.position, transform.position);
    }

    public virtual void Attack()
    {
        isAttack = true;
        LookTarget();
    }

    public virtual void Die()
    {
        Stop();
        AnimStop();
        agent.isStopped = true;
        agent.velocity = Vector3.zero;
        agent.ResetPath();
        chaseTarget = null;
        bossCollider.enabled = false;
        InterfaceMgr.Instance.BossUIDisabled();
        //minimapIcon.gameObject.SetActive(false);
    }

    public  void OnDamage(float damage, Vector3 enemyPos)
    {

    }

    public virtual float OnDamage(float damage)
    {
        if (!isDead)
        {
            GameMgr.Instance.ShakeCamera(0.5f, 0.5f);
            EffectMgr.Instance.PlayParticleSystem("Hit",transform.position + Vector3.up * 15);
            if (IsGrogy)
                HP -= damage * 1.5f;
            else
            {
                HP -= damage;
                GrogyGauge -= damage;
            }

            HP = Mathf.Clamp(HP, 0f, HPMax);
            GrogyGauge = Mathf.Clamp(GrogyGauge, 0f, MaxGrogyGauge);

            InterfaceMgr.Instance.UpdateBossHP(HP);
            InterfaceMgr.Instance.UpdateBossGrogyGauge(GrogyGauge);

            //if (isDead)
            //{
            //    OnDieState();
            //    return data.Exp;
            //}
            //else if (GrogyGauge <= 0f)
            //    return 0f;
            //else if (!isAttack)
            //    OnDamagedState();
        }

        return 0;
    }

    public virtual void Grogy()
    {
        Stop();
        AnimStop();
        IsGrogy = true;
        EffectMgr.Instance.PlayParticleSystem("Grogy", transform.position + Vector3.up * 20);
    }

    public virtual void Init()
    {
        Initialize();
    }
}
