using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

[RequireComponent(typeof(Rigidbody2D))]
public class Monster : Character, IDamageable,IPoolingObject
{
    [Header("Function")]
    [SerializeField] MonsterAnimation anim;

    [Header("Monster State")]
    [SerializeField] MonsterIdleState idle;
    [SerializeField] MonsterMoveState move;
    [SerializeField] MonsterAttackState attack;
    [SerializeField] MonsterDamagedState damaged;
    [SerializeField] MonsterDieState die;

    [Header("Monster Ability")]
    [SerializeField] MonsterData abilityData;

    [Header("Collider")]
    [SerializeField] CircleCollider2D head;
    [SerializeField] BoxCollider2D body;
    [SerializeField] BoxCollider2D leg;
    [SerializeField] BoxCollider2D hit;

    [Header("Position")]
    [SerializeField] GameObject targetPos;
    [SerializeField] Transform effectPos;

    FSM<Monster> stateMachine = null;
    MonsterStateData stateData = null;

    float stiffnessTimer;
    float delayTime = -1f;

    IDamageable target;
    LayerMask mask;

    int penetrationDefence;

    public float HP { get; set; }

    public bool isDead { get { return HP <= 0; } }

    public int ID => abilityData.ID;

    public event System.Action<Monster> OnDieEvent = null;

    private void FixedUpdate()
    {
        rigid2d.velocity = velocity;
    }

    private void Awake()
    {
        rigid2d = GetComponent<Rigidbody2D>();
        if (rigid2d)
        {
            rigid2d.freezeRotation = true;
            rigid2d.gravityScale = 0;
        }

        mask = LayerMask.GetMask("DefenceLine");
    }

    public override void Initialize()
    {
        Stop();
        dir = new Vector2(-1, 0);

        anim.Initialize();
        speed = abilityData.MoveSpeed;

        stateData = ScriptableObject.CreateInstance<MonsterStateData>();
        stateData.SetData(idle, move, attack, damaged, die);
        SetData();
    }

    public void SetPosition(Vector2 pos)
    {
        transform.position = pos;
    }

    public bool SetData()
    {
        if(null == stateData)
        {
            ErrorMessage("Monster State Data가 Null 입니다.");
            return false;
        }

        if (null == stateMachine)
            stateMachine = new FSM<Monster>(this);

        if(!stateMachine.SetCurrState(stateData.IdleState))
        {
            ErrorMessage("Current State가 Null 입니다.");
            return false;
        }

        HP = abilityData.Health;
        abledCollider();
        targetPos.SetActive(true);
        SoundMgr.Instance.SpawnSoundPlay(ID);

        StartCoroutine(OnUpdate());
        return true;
    }

    IEnumerator OnUpdate()
    {
        while(true)
        {
            CheckDelayTime();
            stateMachine.Update();
            yield return new WaitForSeconds(updateTime);
        }
    }

    public bool CheckAttackRange()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, abilityData.AttackRange, mask);

        if (hit)
        {
            target = hit.transform.GetComponentInParent<IDamageable>();
            return true;
        }
        return false;
    }

    public void MoveAnim()
    {
        anim.Move();
    }

    public void StopAnim()
    {
        anim.Stop();
    }

    public void Attack()
    {
        SoundMgr.Instance.AttackSoundPlay(ID);
        target.OnDamage(abilityData.Damage);
        attackTimer = abilityData.AttackSpeed;
    }

    public void AttackAnim()
    {
        anim.Attack();
    }

    public void DamagedAnim()
    {
        anim.Damaged();
        stiffnessTimer = abilityData.Stiffness;
    }

    public bool CheckStiffnessTime()
    {
        if(stiffnessTimer >= 0)
        {
            stiffnessTimer -= updateTime;
            
            return true;
        }
        stateMachine.ChangePrevState();
        return false;
    }

    public void DieAnim()
    {
        anim.Die();
    }

    public void Die()
    {
        GameMgr.Instance.MonsterCount--;
        targetPos.SetActive(false);
        DisabledCollider();
        StartCoroutine(OnDie());
        SoundMgr.Instance.DieSoundPlay(ID);
        ItemMgr.Instance.DropStuff(abilityData.DropBonePiece, abilityData.DropIronPiece, abilityData.DropDarkMaterial);
    }

    IEnumerator OnDie()
    {
        yield return new WaitForSeconds(1.0f);

        EffectMgr.Instance.PlayDieEffect(effectPos.position);

        OnDieEvent?.Invoke(this);
        OnDieEvent = null;
    }

    void DisabledCollider()
    {
        if (head != null)
            head.enabled = false;

        if (body != null)
            body.enabled = false;

        if (leg != null)
            leg.enabled = false;

        if (hit != null)
            hit.enabled = false;
    }

    void abledCollider()
    {
        if (head != null)
            head.enabled = true;

        if (body != null)
            body.enabled = true;

        if (leg != null)
            leg.enabled = true;

        if (hit != null)
            hit.enabled = true;
    }

    bool CheckDelayTime()
    {
        if (delayTime >= 0.0f)
        {
            delayTime -= updateTime;
            return false;
        }

        return true;
    }

    public int OnDamage(float atk , string HitPart)
    {
        if (!CheckDelayTime())
            return 0;

        float damage = 0;

        switch (HitPart)
        {
            case "Head":
                damage = atk * 2;
                penetrationDefence = abilityData.Head;
                break;
            case "Body":
                damage = atk;
                penetrationDefence = abilityData.Body;
                break;
            case "Leg":
                damage = atk;
                penetrationDefence = abilityData.Leg;
                break;
        }

        HP = Mathf.Max(HP - damage, 0);
        delayTime = abilityData.DelayTime;

        if (isDead)
        {
            OnDieState();
            return penetrationDefence;
        }
        else
            OnDamagedState();

        return penetrationDefence;
    }

    public void RestoreHealth(float value)
    {

    }

    public void ReturnBack()
    {
        OnDieEvent = null;
    }

    public void OnIdleState()
    {
        if (!stateMachine.ChangeState(stateData.IdleState))
        {
            ErrorMessage("Idle State가 Null 입니다.");
        }
    }

    public void OnMoveState()
    {
        stateMachine.PrevState = stateData.MoveState;
        if (!stateMachine.ChangeState(stateData.MoveState))
        {
            ErrorMessage("Move State가 Null 입니다.");
        }
    }

    public void OnAttackState()
    {
        stateMachine.PrevState = stateData.AttackState;
        if (!stateMachine.ChangeState(stateData.AttackState))
        {
            ErrorMessage("Attack State가 Null 입니다.");
        }
    }

    public void OnDamagedState()
    {
        if (!stateMachine.ChangeState(stateData.DamagedState))
        {
            ErrorMessage("Damaged State가 Null 입니다.");
        }
    }

    public void OnDieState()
    {
        if (!stateMachine.ChangeState(stateData.DieState))
        {
            ErrorMessage("Die State가 Null 입니다.");
        }
    }
}