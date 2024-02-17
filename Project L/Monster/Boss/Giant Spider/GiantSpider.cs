using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GiantSpider : Boss
{
    [Header("Function")]
    [SerializeField] GiantSpiderAnimation anim;

    [Header("Giant Spider State")]
    [SerializeField] GiantSpiderIdleState idle;
    [SerializeField] GiantSpiderWalkState walk;
    [SerializeField] GiantSpiderBattleState battle;
    [SerializeField] GiantSpiderCriticalAttackState criticalAttack;
    [SerializeField] GiantSpiderChaseState chase;
    [SerializeField] GiantSpiderMeleeAttackState meleeAttack;
    [SerializeField] GiantSpiderGrogyState grogy;
    [SerializeField] GiantSpiderDieState die;
    [SerializeField] GiantSpiderDamagedState damaged;

    [Header("Giant Spider Data")]
    [SerializeField] GiantSpiderData data;

    [Header("Critical Attack Range")]
    [SerializeField] Transform criticalAttackRange;

    [Header("Leg Attack Range")]
    [SerializeField] GameObject legAttackRange;

    FSM<GiantSpider> fsm = null;
    GiantStateData state = null;

    Vector3 attackTargetPos;
    float criticalAttackTimer;
    float normalAttackTimer;
    float legAttackTimer;

    bool isCriticalAttack;
    bool jumpEnd;
    bool isLegAttack;
    bool isNormalAttack;

    private void Update()
    {
        this.Move();

        if (!isBossUIActive && Vector3.Distance(transform.position, GameMgr.Instance.mainCam.transform.position) <= data.SearchRange)
        {
            isBossUIActive = true;
            InterfaceMgr.Instance.BossUIActivate(data.MonsterName, HP, GrogyGauge);
        }
        else if(Vector3.Distance(transform.position, GameMgr.Instance.mainCam.transform.position) > data.SearchRange)
        {
            isBossUIActive = false;
            InterfaceMgr.Instance.BossUIDisabled();
        }
            

        if(jumpEnd)
        {
            Vector3.SmoothDamp(transform.position, attackTargetPos, ref velocity, 0.6f);
            criticalAttackRange.transform.position = attackTargetPos;
        }

        if (GrogyGauge <= 0 && !IsGrogy)
            OnGrogyState();
    }

    public override void Init()
    {
        base.Init();
        isCriticalAttack = false;
        jumpEnd = false;
        IsGrogy = false;
        isLegAttack = false;
        isNormalAttack = false;

        criticalAttackTimer = -1f;
        normalAttackTimer = -1f;
        legAttackTimer = -1f;

        minMaxIdleTime = new Vector2(data.MinIdleTime, data.MaxIdleTime);
        minMaxWalkTime = new Vector2(data.MinWalkTime, data.MaxWalkTime);

        HPMax = data.HP;
        HP = data.HP;
        MaxGrogyGauge = data.GrogyGauge;
        GrogyGauge = data.GrogyGauge;

        legAttackRange.gameObject.SetActive(false);
        criticalAttackRange.gameObject.SetActive(false);

        state = ScriptableObject.CreateInstance<GiantStateData>();
        state.SetData(idle, walk, battle, criticalAttack, chase, meleeAttack, grogy, die, damaged);
        SetData();
    }

    public bool SetData()
    {
        if (null == state)
        {
            ErrorMessage(data.MonsterName + " State Data가 Null 입니다.");
            return false;
        }

        if (null == fsm)
            fsm = new FSM<GiantSpider>(this);

        if(!fsm.SetCurrState(state.IdleState))
        {
            ErrorMessage("Current State가 Null 입니다.");
            return false;
        }

        StartCoroutine(OnUpdate());
        return true;
    }

    IEnumerator OnUpdate()
    {
        while(true)
        {
            fsm.Update();
            AttackTimers();
            yield return new WaitForSeconds(updateTime);
        }
    }

    protected override void AttackTimers()
    {
        if (criticalAttackTimer > 0)
            criticalAttackTimer -= updateTime;

        if (normalAttackTimer > 0)
            normalAttackTimer -= updateTime;

        if (legAttackTimer > 0)
            legAttackTimer -= updateTime;
    }

    public override void AnimStop()
    {
        anim.Stop();
    }

    public override void Move()
    {
        if(!isCriticalAttack)
        {
            accel += data.WalkSpeed * Time.deltaTime;
            accel = Mathf.Clamp(accel, 0, data.WalkSpeed);

            velocity = dir * accel;
        }
        rigid.velocity = velocity;
    }

    public override void WalkAnim()
    {
        anim.Walk();
    }

    public bool FindTarget()
    {
        Collider[] targets = Physics.OverlapSphere(transform.position, data.SearchRange, data.TargetLayer);
        if(null != targets && 0 < targets.Length)
        {
            chaseTarget = targets[0].GetComponent<Rigidbody>();
            return true;
        }

        return false;
    }

    public void RoarAnim()
    {
        bossAudio.PlaySFX(data.MonsterName + " Roar");
        anim.Roar();
    }

    public bool CheckCriticalAttackDuration()
    {
        if (0f > criticalAttackTimer)
            return true;
        return false;
    }

    public bool CheckNormalAttackDuration()
    {
        if (0f > normalAttackTimer)
            return true;
        return false;
    }

    public bool CheckLegAttackDuration()
    {
        if (0f > legAttackTimer)
            return true;
        return false;
    }

    public void CriticalAttack()
    {
        criticalAttackTimer = data.CriticalAttackCoolTime;
        anim.CriticalAttack();
        isCriticalAttack = true;
        bossCollider.enabled = false;

        attackTargetPos = new Vector3(chaseTarget.position.x, 1.5f, chaseTarget.position.z);
        criticalAttackRange.transform.position = attackTargetPos;
        criticalAttackRange.gameObject.SetActive(true);

        if (coroutine != null)
            StopCoroutine(coroutine);
        coroutine = StartCoroutine(OnJump());
    }

    IEnumerator OnJump()
    {
        yield return new WaitForSeconds(0.9f);
        jumpEnd = true;
    }

    public void CriticalAttackEnd()
    {
        isCriticalAttack = false;

        jumpEnd = false;
        bossCollider.enabled = true;
        criticalAttackRange.gameObject.SetActive(false);
        if (coroutine != null)
            StopCoroutine(coroutine);
    }

    public override void SetChaseTarget()
    {
        base.SetChaseTarget();

        anim.Run();
        agent.speed = data.RunSpeed;
        agent.stoppingDistance = data.MeleeAttackRange;
    }

    public override void Attack()
    {
        base.Attack();
        isNormalAttack = true;
        normalAttackTimer = data.NormalAttackCoolTime;
        anim.NormalAttack();
    }

    public void LegAttack()
    {
        base.Attack();
        legAttackRange.gameObject.SetActive(true);
        isLegAttack = true;
        legAttackTimer = data.LegAttackCoolTime;
        anim.LegAttack();
    }

    public void LegAttackEnd()
    {
        isLegAttack = false;
        legAttackRange.gameObject.SetActive(false);
    }

    public void AttackEnd()
    {
        isAttack = false;
        isNormalAttack = false;
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.tag.Equals("Player"))
        {
            float damage = SetDamage();
            IFightable attackTarget = collision.GetComponentInParent<IFightable>();
            attackTarget.OnDamage(damage, transform.position);
            bossAudio.PlaySFX(data.MonsterName + " Hit");
        }
    }

    float SetDamage()
    {
        float damage = data.MeleeAttack1;
        if (isCriticalAttack)
            damage = 100000f;
        else if (isLegAttack)
            damage = data.MeleeAttack2;
        return damage;
    }

    public override float OnDamage(float damage)
    {
        base.OnDamage(damage);
        bossAudio.PlaySFX(data.MonsterName + " Damaged");

        if (isDead)
        {
            OnDieState();
            return data.Exp;
        }
        else if (GrogyGauge <= 0f)
            return 0f;
        else if (!isAttack)
            OnDamagedState();

        return 0;
    }

    public override void Grogy()
    {
        base.Grogy();

        if (coroutine != null)
            StopCoroutine(coroutine);
        coroutine = StartCoroutine(OnGrogy());
    }

    IEnumerator OnGrogy()
    {
        anim.Grogy();
        yield return new WaitForSeconds(data.GrogyTime);
        anim.GrogyEnd();
        IsGrogy = false;
        InterfaceMgr.Instance.UpdateBossGrogyGauge(data.GrogyGauge);
        GrogyGauge = data.GrogyGauge;
    }

    public override void Die()
    {
        base.Die();
        bossAudio.PlaySFX(data.MonsterName + " Die");
        anim.Die();
        QuestMgr.Instance.UpdateMonster(data.ID);

        if (coroutine != null)
            StopCoroutine(coroutine);
        coroutine = StartCoroutine(OnDie());
    }

    IEnumerator OnDie()
    {
        yield return new WaitForSeconds(3.5f);
        gameObject.SetActive(false);
    }

    public override void Damaged()
    {
        anim.Damaged();
    }

    public void BattalAnim()
    {
        anim.Battle();
    }

    public void OnIdleState()
    {
        if (!fsm.ChangeState(state.IdleState))
            ErrorMessage("Idle State가 Null 입니다.");
    }

    public void OnWalkState()
    {
        if (!fsm.ChangeState(state.WalkState))
            ErrorMessage("Walk State가 Null 입니다.");
    }

    public void OnBattleState()
    {
        if (!fsm.ChangeState(state.BattleState))
            ErrorMessage("Battle State가 Null 입니다.");
    }

    public void OnCriticalAttackState()
    {
        if (!fsm.ChangeState(state.CriticalAttackState))
            ErrorMessage("Critical Attack State가 Null 입니다.");
    }

    public void OnChaseState()
    {
        if (!fsm.ChangeState(state.ChaseState))
            ErrorMessage("Chase State가 Null 입니다.");
    }

    public void OnMeleeAttackState()
    {
        if (!fsm.ChangeState(state.MeleeAttackState))
            ErrorMessage("Melee Attack State가 Null 입니다.");
    }

    public void OnGrogyState()
    {
        if (!fsm.ChangeState(state.GrogyState))
            ErrorMessage("Grogy State가 Null 입니다.");
    }

    public void OnDieState()
    {
        if (!fsm.ChangeState(state.DieState))
            ErrorMessage("Die State가 Null 입니다.");
    }

    public void OnDamagedState()
    {
        if (!fsm.ChangeState(state.DamagedState))
            ErrorMessage("Damaged State가 Null 입니다.");
    }
}
