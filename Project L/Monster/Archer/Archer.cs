using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Archer : Monster
{
    [Header("ShotPos")]
    [SerializeField] Transform shotPos;

    [Header("Test Arrow")]
    [SerializeField] Arrow arrowPrefab;

    LineRenderer arrowLineRenderer;

    bool IsBackWalk;

    protected override void Awake()
    {
        base.Awake();
        if(TryGetComponent(out arrowLineRenderer))
        {
            arrowLineRenderer.positionCount = 2;
            arrowLineRenderer.material.color = Color.red;
            arrowLineRenderer.widthMultiplier = 0.5f;
            arrowLineRenderer.enabled = false;
        }
    }

    public void Init()
    {
        Initialize();
    }


    public override void AttackReady()
    {
        Stop();
        IsBackWalk = false;
        anim.Walk_BackStop();
        anim.Charge();
    }

    public void AttackReady2()
    {
        if (coroutine != null)
            StopCoroutine(coroutine);
        StartCoroutine(OnAttackReady());
    }

    IEnumerator OnAttackReady()
    {
        DrawTrajectory();
        IsAttackReady = true;

        yield return new WaitForSeconds(data.AttackReadyTime);

        OffTrajectory();
        IsAttackReady = false;
        OnAttackState();
    }

    void DrawTrajectory()
    {
        Vector3 startPos = shotPos.position;
        Vector3 targetPos = chaseTarget.position;
        targetPos.y = startPos.y;
        Vector3 arrowDir = (targetPos - startPos).normalized;

        arrowLineRenderer.SetPosition(0, startPos);
        arrowLineRenderer.SetPosition(1, startPos + arrowDir * data.AttackRange * 2f);
        arrowLineRenderer.enabled = true;
    }

    void OffTrajectory()
    {
        arrowLineRenderer.enabled = false;
    }

    public override bool IsAttackBound()
    {
        float dist = DistanceToTarget();
        if (0 > dist)
            return false;

        if (dist <= agent.stoppingDistance)
        {
            if (dist <= data.BackWalkRange)
            {
                if (!IsBackWalk)
                    WalkBack();
                dir = (this.transform.position - chaseTarget.transform.position).normalized;
            }
            else if(IsBackWalk)
                WalkBackEnd();

            return true;
        }

        return false;
    }

    void WalkBack()
    {
        anim.Walk_Back();
        IsBackWalk = true;
    }

    void WalkBackEnd()
    {
        Stop();
        anim.Stop();
        anim.Walk_BackStop();
        IsBackWalk = true;
    }

    public override void Attack()
    {
        base.Attack();
        anim.Attack();
        IsAttackAnimEnd = false;

        Vector3 pos = chaseTarget.position;
        pos.y = shotPos.position.y;

        Arrow arrow = MakeArrow();
        arrow.SetPosition(shotPos.position);
        arrow.Shot(pos, data.Damage, 5000);
        monsterAudio.PlaySFX(data.MonsterName + " Attack");
    }

    Arrow MakeArrow()
    {
        Arrow arrow = Instantiate(arrowPrefab);
        return arrow;
    }
}
