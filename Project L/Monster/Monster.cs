using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using System.ComponentModel;

public abstract class Monster : Character,IFightable,IPoolingObject
{
    [Header("Function")]
    [SerializeField] protected MonsterAnimation anim;
    [SerializeField] MonsterUI monsterUI;
    [SerializeField] protected NavMeshAgent agent;
    [SerializeField] protected MonsterAudio monsterAudio;
    
    [Header("Monster state")]
    [SerializeField] MonsterIdleState idle;
    [SerializeField] MonsterWalkState walk;
    [SerializeField] MonsterChaseState chase;
    [SerializeField] MonsterAttackState attack;
    [SerializeField] MonsterDieState die;
    [SerializeField] MonsterDamagedState damaged;
    [SerializeField] MonsterAttackReadyState attackReady;
    [SerializeField] MonsterFindObjectState findObjectState;
    [SerializeField] MonsterExecutedState executedState;

    [Header("Monster Data")]
    [SerializeField] protected MonsterData data;

    [SerializeField] GameObject minimapIcon;
    [SerializeField] Collider collider;

    FSM<Monster> fsm = null;
    MonsterStateData state = null;

    private static readonly float plusWarningGauge = 40f;

    protected Transform chaseTarget;
    protected Coroutine coroutine = null;
    bool IsChase;
    bool IsDie;
    public bool IsAttackAnimEnd { get; set; }
    public bool IsAttackReady { get; set; }

    bool isChaseSound;
    float idleTime = 0f;
    float walkTime = 0f;

    float attackTimer;
    float currWarningGauge;
    float stiffness;

    public bool IsThereTarget { get; private set; }

    public bool Warning { get { return currWarningGauge >= data.MaxWaringGauge; } }

    public int ID => data.ID;

    public float HPMax { get; set; }
    public float HP { get; set; }
    public bool isDead { get { return HP <= 0; } }
    public event System.Action<Monster> OnDieEvent = null;

    public override void Initialize()
    {
        Stop();

        monsterUI.Initialize(data.MaxWaringGauge, data.HP);

        IsChase = false;
        IsDie = false;
        IsAttackAnimEnd = true;
        IsAttackReady = false;
        isChaseSound = false;

        HPMax = data.HP;
        HP = HPMax;

        anim.Initialize();
        monsterAudio.Initialize();
        state = ScriptableObject.CreateInstance<MonsterStateData>();
        state.SetData(idle, walk, chase, attack, die, damaged, attackReady, findObjectState, executedState);
        SetData();
        agent.enabled = false;
    }

    private void Update()
    {
        minimapIcon.transform.forward = new Vector3(-this.transform.forward.x, minimapIcon.transform.position.y, -this.transform.forward.z);
        this.Move();
    }

    public bool SetData()
    {
        if(null == state)
        {
            ErrorMessage(data.MonsterName + " State Data가 Null 입니다.");
            return false;
        }

        if (null == fsm)
            fsm = new FSM<Monster>(this);

        if(!fsm.SetCurrState(state.IdleState))
        {
            ErrorMessage("Current State가 Null 입니다.");
            return false;
        }

        HP = data.HP;
        currWarningGauge = 0f;
        chaseTarget = null;
        IsThereTarget = false;

        StartCoroutine(OnUpdate());
        return true;
    }

    IEnumerator OnUpdate()
    {
        while(true)
        {
            fsm.Update();
            if(attackTimer >=0)
                attackTimer -= updateTime;
            yield return new WaitForSeconds(updateTime);
        }
    }

    public void SetIdleTime()
    {
        idleTime = 0;
        idleTime -= UnityEngine.Random.Range(data.MinIdleTime, data.MaxIdleTime);
    }

    public bool CheckIdleTime()
    {
        if(idleTime >= 0)
            return true;

        idleTime += updateTime;
        return false;
    }

    public void SetWalkTime()
    {
        walkTime = 0;
        walkTime -= UnityEngine.Random.Range(data.MinWalkTime, data.MaxWalkTime);
    }

    public void GetRandomDir()
    {
        dir = new Vector3(UnityEngine.Random.Range(-1.0f, 1.0f), 0, UnityEngine.Random.Range(-1.0f, 1.0f)).normalized;

        transform.rotation = Quaternion.LookRotation(dir);
    }

    public override void Move()
    {
        if(IsChase)
        {
            accel += data.RunSpeed * Time.deltaTime;
            accel = Mathf.Clamp(accel, 0, data.RunSpeed);
        }
        else
        {
            accel += data.WalkSpeed * Time.deltaTime;
            accel = Mathf.Clamp(accel, 0, data.WalkSpeed);
        }
        velocity = dir * accel;
        if (rigid.velocity.y > 0)
            rigid.velocity = new Vector3(velocity.x, -9.86f, velocity.z);
        else
            rigid.velocity = new Vector3(velocity.x, rigid.velocity.y, velocity.z);
    }

    public void Damaged()
    {
        stiffness = data.Stiffness;
        if (chaseTarget == null)
            chaseTarget = FindObjectOfType<Player>().transform;
        LookRotationTarget();
        currWarningGauge = data.MaxWaringGauge;
        monsterUI.WarningSliderUpdate(currWarningGauge, true);
        anim.Hit();
    }

    public void Die()
    {
        IsDie = true;
        Stop();
        ItemBoxMgr.Instance.SetDropBoxes(transform.position, data.DropTableKey, data.MonsterName);
        collider.enabled = false;
        rigid.useGravity = false;
        minimapIcon.gameObject.SetActive(false);
        monsterUI.gameObject.SetActive(false);
        agent.enabled = false;
        QuestMgr.Instance.UpdateMonster(data.ID);
        monsterAudio.PlaySFX(data.MonsterName + " Die");

        IInteractionable interaction = GetComponentInChildren<IInteractionable>();
        interaction.RemoveInteractionToList();

        if (coroutine != null)
            StopCoroutine(coroutine);
        StartCoroutine(OnDie());
    }

    public void DieAnim()
    {
        anim.Die();
    }

    IEnumerator OnDie()
    {
        yield return new WaitForSeconds(3.5f);

        gameObject.SetActive(false);
        OnDieEvent?.Invoke(this);
        OnDieEvent = null;
    }

    private void OnDisable()
    {
        OnDieEvent = null;
    }

    public bool CheckStiffness()
    {
        if(stiffness > 0)
        {
            stiffness -= updateTime;
            return false;
        }
        return true;
    }

    public bool CheckWalkTime()
    {
        if (walkTime >= 0)
            return true;

        walkTime += updateTime;
        return false;
    }

    public void AnimStop()
    {
        anim.Stop();
    }

    public void IdleToRun()
    {
        anim.IdleToRun();
    }

    public void WalkAnim()
    {
        anim.Walk();
    }

    bool CheckObstacle(Vector3 targetPos)
    {
        if (Physics.Raycast(transform.position + Vector3.up * 5, (targetPos - transform.position), out RaycastHit hit)) 
        {
            if (hit.collider.tag != "Obstacle")
                return true;
        }
        return false;
    }

    bool CheckAngle(Vector3 targetPos)
    {
        Vector2 angle = new Vector2(targetPos.x, targetPos.z) - new Vector2(transform.position.x, transform.position.z);
        float dot = Vector2.Dot(new Vector2(transform.forward.x, transform.forward.z), angle);
        return (dot >= data.FindAngle);
    }

    public bool FindTarget()
    {
        Collider[] targets = Physics.OverlapSphere(transform.position, data.SearchRange, data.TargetLayer);
        if(null != targets && 0< targets.Length)
        {
            if (CheckObstacle(targets[0].transform.position) && CheckAngle(targets[0].transform.position)) 
            {
                IsThereTarget = true;
                chaseTarget = targets[0].transform;
                //chaseTarget = targets[0].GetComponent<Rigidbody>();
                currWarningGauge += plusWarningGauge * Time.deltaTime;
                currWarningGauge = Mathf.Clamp(currWarningGauge, 0, data.MaxWaringGauge);
                monsterUI.WarningSliderUpdate(currWarningGauge);
                return true;
            }
        }

        if (currWarningGauge > 0)
        {
            currWarningGauge -= plusWarningGauge * Time.deltaTime;
            currWarningGauge = Mathf.Clamp(currWarningGauge, 0, data.MaxWaringGauge);
            if (currWarningGauge == 0)
                IsThereTarget = false;
            monsterUI.WarningSliderUpdate(currWarningGauge);
        }
        return false;
    }

    public void LookAround()
    {
        anim.LookAround();
    }

    public void LookAroundStop()
    {
        anim.LookAroundStop();
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, data.SearchRange);
        //Gizmos.DrawRay(transform.position, aa);
        //Gizmos.DrawLine(transform.position + Vector3.up * 5, testTarget + Vector3.up * 5);
    }

    protected float DistanceToTarget()
    {
        if (!chaseTarget)
            return -1.0f;
        return Vector3.Distance(chaseTarget.position, transform.position);
    }

    public bool TargetLostCheck()
    {
        float dist = DistanceToTarget();
        if((dist <0) || (dist > data.SearchRange * 1.5f))
        {
            LostTarget();
            return true;
        }
        return false;
    }

    public abstract bool IsAttackBound();


    public bool CheckAttackDuration()
    {
        if(0f > attackTimer)
            return true;
        return false;
    }

    public abstract void AttackReady();

    public void ReadyAttack()
    {
        agent.isStopped = true;
        agent.ResetPath();
        anim.Stop();
    }

    void LostTarget()
    {
        IsChase = false;
        chaseTarget = null;
        agent.isStopped = true;
        agent.velocity = Vector3.zero;
        agent.ResetPath();
    }

    public void ChaseStart()
    {
        if(!agent.enabled)
            agent.enabled = true;
        agent.speed = data.RunSpeed;
        agent.stoppingDistance = data.AttackRange;
        
        monsterUI.WarningImageOn();
        monsterUI.WarningSliderOff();
        anim.Run();

        if(!isChaseSound)
        {
            monsterAudio.PlaySFX(data.MonsterName + " Chase");
            isChaseSound = true;
        }
    }

    public void ChaseEnd()
    {
        monsterUI.WarningImageOff();
        monsterUI.WarningSliderOff();
        anim.Stop();
        agent.isStopped = true;
        agent.velocity = Vector3.zero;
        IsChase = false;
        
    }

    public void ChaseTarget()
    {
        if (!chaseTarget)
            return;

        agent.isStopped = false;
        agent.acceleration = accel;
        agent.SetDestination(chaseTarget.position);
    }


    public void OnDamage(float damage, Vector3 pos) { }

    public float OnDamage(float damage)
    {
        if(!IsDie)
        {
            HP -= damage;
            HP = Mathf.Clamp(HP, 0, HPMax);
            GameMgr.Instance.ShakeCamera(0.1f, 0.5f);
            monsterAudio.PlaySFX(data.MonsterName + " Damaged");
            EffectMgr.Instance.PlayParticleSystem("Hit", transform.position + Vector3.up * 10);
            monsterUI.UpdateHP(HP);
            if (isDead)
            {
                OnDieState();
                return data.EXP;
            }  
            else
                OnDamagedState();   
        }
        return 0;
    }

    public virtual void Attack()
    {
        attackTimer = data.AttackDelay;
    }


    public void AttackReadyEnd()
    {
        if(coroutine != null)
            StopCoroutine(coroutine);
        coroutine = null;
        IsAttackReady = false;
        IsAttackAnimEnd = false;
    }

    public void LookRotationTarget()
    {
        Vector3 targetPos = chaseTarget.position;
        Vector3 currPos = transform.position;
        currPos.y = targetPos.y;

        Vector3 dir = (targetPos - currPos).normalized;
        transform.rotation = Quaternion.LookRotation(dir);
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.tag.Equals("Player"))
        {
            IFightable attackTarget = collision.GetComponentInParent<IFightable>();
            attackTarget.OnDamage(data.Damage, transform.position);
            monsterAudio.PlaySFX(data.MonsterName + " Hit");
        }
    }

    public void SetExecuted(Vector3 executedPos)
    {
        transform.position = new Vector3(executedPos.x, transform.position.y, executedPos.z);
        transform.LookAt(executedPos * 10f);
    }

    public void Executed()
    {
        Die();
    }

    public void SetPosition(Vector3 pos)
    {
        transform.position = pos;
    }

    public void SetAngle(Vector3 angle)
    {
        transform.eulerAngles = angle;
    }

    public void ExecutedAnim()
    {
        anim.Executed();
    }

    public void ExecutedEnd()
    {
        
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

    public void OnChaseState()
    {
        IsChase = true;
        if (!fsm.ChangeState(state.ChaseState))
            ErrorMessage("Chase State가 Null 입니다.");
    }

    public void OnAttackState()
    {
        if (!fsm.ChangeState(state.AttackState))
            ErrorMessage("Attack State가 Null 입니다.");
    }

    public void OnDamagedState()
    {
        if (!fsm.ChangeState(state.DamagedState))
            ErrorMessage("Damaged State가 Null 입니다.");
    }

    public void OnDieState()
    {
        if (!fsm.ChangeState(state.DieState))
            ErrorMessage("Die State가 Null 입니다.");
    }

    public void OnAttackReadyState()
    {
        if (!fsm.ChangeState(state.AttackReadyState))
            ErrorMessage("Attack Ready State가 Null 입니다.");
    }

    public void OnFindObjectState()
    {
        if (!fsm.ChangeState(state.FindObjectState))
            ErrorMessage("Find Object State가 Null 입니다.");
    }

    public void OnExecutedState()
    {
        if (!fsm.ChangeState(state.ExecutedState))
            ErrorMessage("Executed State가 Null 입니다.");
    }
}
