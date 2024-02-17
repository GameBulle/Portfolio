using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BanditReader : Boss
{
    [Header("Function")]
    [SerializeField] BanditReaderAnimation anim;

    [Header("Bandit Reader State")]
    [SerializeField] BanditReaderIdleState idle;
    [SerializeField] BanditReaderWalkState walk;
    [SerializeField] BanditReaderBattleState battle;
    [SerializeField] BanditReaderBuffState buff;
    [SerializeField] BanditReaderChaseState chase;
    [SerializeField] BanditReaderNormalAttackState normalAttack;
    [SerializeField] BanditReaderJumpAttackState jumpAttack;
    [SerializeField] BanditReaderComboAttackState comboAttack;
    [SerializeField] BanditReaderDieState die;
    [SerializeField] BanditReaderGrogyState grogy;
    [SerializeField] BanditReaderDamagedState damaged;

    [Header("Bandit Reader Data")]
    [SerializeField] BanditReaderData data;

    [Header("Test")]
    [SerializeField] ParticleSystem buffEffect;

    [Header("JumpAttack")]
    [SerializeField] Transform jumpAttackRange;

    FSM<BanditReader> fsm = null;
    BanditReaderStateData state = null;
    Coroutine buffCoroutine = null;
    Vector3 jumpAttackTargetPos;

    float jumpAttackTimer;
    float normalAttackTimer;
    float comboAttackTimer;
    float buffTimer;

    bool isJumpAttack;
    bool isBuff;
    bool isRunToJump;
    bool isComboAttack;

    private void Update()
    {
        this.Move();

        if((!isBossUIActive && Vector3.Distance(transform.position, GameMgr.Instance.mainCam.transform.position) <= data.SearchRange) && !isDead)
        {
            isBossUIActive = true;
            InterfaceMgr.Instance.BossUIActivate(data.MonsterName, HP, GrogyGauge);
        }
        else if(Vector3.Distance(transform.position, GameMgr.Instance.mainCam.transform.position) > data.SearchRange)
        {
            isBossUIActive = false;
            InterfaceMgr.Instance.BossUIDisabled();
        }
       
        if(isRunToJump)
        {
            Vector3.SmoothDamp(transform.position, chaseTarget.position, ref velocity, 2.5f);
        }

        if (isJumpAttack)
        {
            Vector3.SmoothDamp(transform.position, jumpAttackTargetPos, ref velocity, 0.6f);
            jumpAttackRange.transform.position = new Vector3(jumpAttackTargetPos.x, 1.0f, jumpAttackTargetPos.z);
        }

        if (GrogyGauge <= 0 && !IsGrogy)
            OnGrogyState();
    }

    public override void Stop()
    {
        base.Stop();
        agent.acceleration = 0f;
        agent.velocity = Vector3.zero;
        agent.isStopped = true;
    }

    public override void Init()
    {
        base.Init();
        isBossUIActive = false;
        isAttack = false;
        isBuff = false;
        isJumpAttack = false;
        isRunToJump = false;
        isComboAttack = false;

        jumpAttackTimer = -1f;
        normalAttackTimer = -1f;
        comboAttackTimer = -1f;
        buffTimer = -1f;

        minMaxIdleTime = new Vector2(data.MinIdleTime, data.MaxIdleTime);
        minMaxWalkTime = new Vector2(data.MinWalkTime, data.MaxWalkTime);

        HPMax = data.Hp;
        HP = data.Hp;
        MaxGrogyGauge = data.GrogyGauge;
        GrogyGauge = data.GrogyGauge;

        buffEffect.gameObject.SetActive(false);
        jumpAttackRange.gameObject.SetActive(false);

        state = ScriptableObject.CreateInstance<BanditReaderStateData>();
        state.SetData(idle, walk, battle, buff, chase, normalAttack, jumpAttack, comboAttack, die, grogy, damaged);
        SetData();
    }

    public bool SetData()
    {
        if(state == null)
        {
            ErrorMessage(data.MonsterName + "State Data가 Null 입니다.");
            return false;
        }

        if (fsm == null)
            fsm = new FSM<BanditReader>(this);

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

    public override void AnimStop()
    {
        anim.Stop();
    }

    public override void Move()
    {
        if (!isAttack && !isJumpAttack)
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
        if (null != targets && 0 < targets.Length)
        {
            chaseTarget = targets[0].GetComponent<Rigidbody>();
            return true;
        }

        return false;
    }

    public void BattleAnim()
    {
        anim.Battle();
    }

    public bool CheckBuffCoolTime()
    {
        if (buffTimer < 0f)
            return true;
        return false;
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.tag.Equals("Player"))
        {
            float damage = SetDamage();
            IFightable attackTarget = collision.GetComponentInParent<IFightable>();

            if (isBuff)
                damage *= data.BuffPercent;
            attackTarget.OnDamage(damage, transform.position);
            bossAudio.PlaySFX(data.MonsterName + " Hit");
        }
    }

    float SetDamage()
    {
        float damage = data.NormalAttack;
        if (isJumpAttack)
            damage = 100000f;
        else if (isComboAttack)
            damage = data.ComboAttack;
        return damage;
    }

    protected override void AttackTimers()
    {
        if (buffTimer > 0)
            buffTimer -= updateTime;

        if (normalAttackTimer > 0)
            normalAttackTimer -= updateTime;

        if (jumpAttackTimer > 0)
            jumpAttackTimer -= updateTime;

        if (comboAttackTimer > 0)
            comboAttackTimer -= updateTime;
    }

    public void Buff()
    {
        anim.Buff();
        buffEffect.gameObject.SetActive(true);
        if (buffCoroutine != null)
        {
            StopCoroutine(buffCoroutine);
            buffEffect.Stop();
        }

        bossAudio.PlaySFX(data.MonsterName + " Buff");
        buffCoroutine = StartCoroutine(OnBuff());
    }

    IEnumerator OnBuff()
    {
        isBuff = true;
        buffTimer = data.BuffCoolTime;
        buffEffect.Play();
        yield return new WaitForSeconds(data.BuffDuration);
        isBuff = false;
        agent.speed = data.RunSpeed;
        buffEffect.Stop();
        buffEffect.gameObject.SetActive(false);
    }

    public override void SetChaseTarget()
    {
        base.SetChaseTarget();

        anim.Run();
        if (isBuff)
            agent.speed = data.RunSpeed * (1 + data.BuffPercent);
        else
            agent.speed = data.RunSpeed;
        agent.stoppingDistance = data.AttackRange;
    }

    public bool CheckNormalAttackDuration()
    {
        if (normalAttackTimer < 0f)
            return true;
        return false;
    }

    public bool IsJumpAttackBound()
    {
        float dist = DistanceTarget();
        if (0 > dist)
            return false;
        return (dist > agent.stoppingDistance * 1.5f);
    }

    public bool CheckJumpAttackDuration()
    {
        if (jumpAttackTimer < 0f)
            return true;
        return false;
    }

    public override void Attack()
    {
        base.Attack();
        normalAttackTimer = data.AttackCoolTime;
        int random = UnityEngine.Random.Range(0, 2);
        switch(random)
        {
            case 0:
                anim.NormalAttack();
                break;
            case 1:
                anim.KickAtack();
                break;
        }
    }

    public void ComboAttack()
    {
        base.Attack();
        isComboAttack = true;
        LookTarget();
        comboAttackTimer = data.ComboAttackCoolTime;
        anim.ComboAttack();
    }

    public void ComboAttackEnd()
    {
        isComboAttack = false;
    }

    public bool CheckComboAttackDuration()
    {
        if (comboAttackTimer < 0f)
            return true;
        return false;
    }

    public void NormalAttackEnd()
    {
        isAttack = false;
    }

    public override void Die()
    {
        base.Die();
        anim.Die();
        buffEffect.Stop();
        QuestMgr.Instance.UpdateMonster(data.ID);
        bossAudio.PlaySFX(data.MonsterName + " Die");

        if (coroutine != null)
            StopCoroutine(coroutine);
        coroutine = StartCoroutine(OnDie());
    }

    IEnumerator OnDie()
    {
        yield return new WaitForSeconds(3.5f);
        gameObject.SetActive(false);
    }

    public override void Grogy()
    {
        base.Grogy();
        bossAudio.PlaySFX(data.MonsterName + " Grogy");

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
        GrogyGauge = data.GrogyGauge;
        InterfaceMgr.Instance.UpdateBossGrogyGauge(data.GrogyGauge);
    }

    public override float OnDamage(float damage)
    {
        base.OnDamage(damage);

        if (isDead)
        {
            OnDieState();
            return data.Exp;
        }
        else if (GrogyGauge <= 0f)
            return 0f;
        else if (!isAttack)
            OnDamagedState();
        SoundMgr.Instance.PlaySFXAudio(data.MonsterName + " Damaged");

        return 0;
    }

    public void JumpAttack()
    {
        jumpAttackTimer = data.JumpAttackCoolTime;
        anim.JumpAttack();

        isRunToJump = true;
        isAttack = true;
        bossCollider.enabled = false;

        if(coroutine != null)
            StopCoroutine(coroutine);
        coroutine = StartCoroutine(OnRunToJump());
    }
    
    IEnumerator OnRunToJump()
    {
        yield return new WaitForSeconds(1.0f);
        isRunToJump = false;
        StartCoroutine(OnJump());
    }

    IEnumerator OnJump()
    {
        jumpAttackTargetPos = chaseTarget.position;
        jumpAttackRange.transform.position = jumpAttackTargetPos;
        isJumpAttack = true;
        jumpAttackRange.gameObject.SetActive(true);
        yield return new WaitForSeconds(1.0f);
        isJumpAttack = false;
        JumpAttackEnd();
    }

    public void JumpAttackEnd()
    {
        Stop();
        EffectMgr.Instance.PlayParticleSystem("Dust", jumpAttackRange.transform.position);
        bossAudio.PlaySFX(data.MonsterName + " JumpAttack");
        jumpAttackRange.gameObject.SetActive(false);
        isAttack = false;
        isRunToJump = false;
        isRunToJump = false;
        bossCollider.enabled = true;
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

    public void OnBuffState()
    {
        if (!fsm.ChangeState(state.BuffState))
            ErrorMessage("Buff State가 Null 입니다.");
    }

    public void OnChaseState()
    {
        if (!fsm.ChangeState(state.ChaseState))
            ErrorMessage("Chase State가 Null 입니다.");
    }

    public void OnNormalAttackState()
    {
        if (!fsm.ChangeState(state.NormalAttackState))
            ErrorMessage("Normal Attack State가 Null 입니다.");
    }

    public void OnJumpAttackState()
    {
        if (!fsm.ChangeState(state.JumpAttackState))
            ErrorMessage("Jump Attack State가 Null 입니다.");
    }

    public void OnComboAttackState()
    {
        if (!fsm.ChangeState(state.ComboAttackState))
            ErrorMessage("Combo Attack State가 Null 입니다.");
    }

    public void OnDieState()
    {
        if (!fsm.ChangeState(state.DieState))
            ErrorMessage("Die State가 Null 입니다.");
    }

    public void OnGrogyState()
    {
        if (!fsm.ChangeState(state.GrogyState))
            ErrorMessage("Grogy State가 Null 입니다.");
    }

    public void OnDamagedState()
    {
        if (!fsm.ChangeState(state.DamagedState))
            ErrorMessage("Damaged State가 Null 입니다.");
    }
}
