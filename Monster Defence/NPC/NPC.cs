using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public class NPC : Archer,IPoolingObject
{
    [SerializeField] NPCData data;

    [Header("NPC State")]
    [SerializeField] NPCIdleState idle;
    [SerializeField] NPCMoveState move;
    [SerializeField] NPCShotState shot;
    [SerializeField] NPCChargeState charge;

    FSM<NPC> stateMachine = null;
    NPCStateData stateData = null;
    
    float moveY = 0.0f;
    
    public int Number { get; set; }

    public event System.Action<NPC> OnReturnBackEvent = null;

    public void NPCInitialize()
    {
        gameObject.SetActive(true);
        Initialize();
        speed = data.MoveSpeed;

        stateData = ScriptableObject.CreateInstance<NPCStateData>();
        stateData.SetData(idle, move, shot, charge);
    }

    public void SetPosition(Vector2 pos)
    {
        transform.position = pos;
    }

    public bool CheckShot()
    {
        return bow.NPCShot;
    }

    public bool SetData(int num = 0)
    {
        if (null == stateData)
        {
            ErrorMessage("Player State Data가 Null 입니다.");
            return false;
        }

        if (null == stateMachine)
            stateMachine = new FSM<NPC>(this);

        if (!stateMachine.SetCurrState(stateData.IdleState))
        {
            ErrorMessage("Current State가 Null 입니다.");
            return false;
        }

        bow.WaveStart();
        StartCoroutine(OnUpdate());
        Number = num;
        useArrowIndex = 0;
        return true;
    }

    IEnumerator OnUpdate()
    {
        while (true)
        {
            stateMachine.Update();
            yield return new WaitForSeconds(updateTime);
        }
    }

    public bool FindTarget()
    {
        Collider2D[] targets = Physics2D.OverlapCircleAll(transform.position, data.SearchRange, data.TargetLayer);
        float minDistance = 1000f;
        float targetDistance = 0f;
        if (null != targets && 0 < targets.Length)
        {
            for (int i = 0; i < targets.Length; i++)
            {
                targetDistance = Vector2.SqrMagnitude(targets[i].transform.position - transform.position);
                if (minDistance > targetDistance)
                {
                    minDistance = targetDistance;
                    targetPos = targets[i].transform.position;
                }
            }
            GetDirToMonter();
            return true;
        }
        return false;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, data.SearchRange);
        Gizmos.DrawLine(transform.position, targetPos);
    }

    public bool CheckShotAngle()
    { 
        GetDirVector();
        float limitAngle = bowData.ShotAngle;
        return (angle < limitAngle);
    }

    private void GetDirToMonter()
    {
        if (targetPos.y > transform.position.y)
        {
            moveY = targetPos.y - transform.position.y;
            dir = new Vector2(0, 1);
        }
        else
        {
            moveY = transform.position.y - targetPos.y;
            dir = new Vector2(0, -1);
        }
    }

    public bool CheckMove()
    {
        if (bow.ShotPos.y >= targetPos.y - 0.2 && bow.ShotPos.y <= targetPos.y + 0.2)
        {
            dir = Vector2.zero;
            return true;
        }

        return false;
    }

    public void ReturnBack()
    {
        targetPos = Vector2.zero;
        OnReturnBackEvent?.Invoke(this);
        OnReturnBackEvent = null;
    }

    public override void OnIdleState()
    {
        act = Act.Idle;
        if(!stateMachine.ChangeState(stateData.IdleState))
        {
            ErrorMessage("Idle State가 Null 입니다.");
        }
    }

    public override void OnMoveState()
    {
        act = Act.Move;
        if(!stateMachine.ChangeState(stateData.MoveState))
        {
            ErrorMessage("Move State가 Null 입니다.");
        }
    }

    public override void OnChargeState()
    {
        act = Act.Charge;
        if(!stateMachine.ChangeState(stateData.ChargeState))
        {
            ErrorMessage("Charge State가 Null 입니다.");
        }
    }

    public override void OnShotState()
    {
        act = Act.Shot;
        if(!stateMachine.ChangeState(stateData.ShotState))
        {
            ErrorMessage("Shot State가 Null 입니다.");
        }
    }
}
