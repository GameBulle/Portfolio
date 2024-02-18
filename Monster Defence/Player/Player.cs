using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Playables;
using UnityEngine.UI;

public class Player : Archer
{
    [SerializeField] float speedValue = 4.0f;

    [Header("Player State")]
    [SerializeField] PlayerIdleState idle;
    [SerializeField] PlayerMoveState move;
    [SerializeField] PlayerChargeState charge;
    [SerializeField] PlayerShotState shot;

    FSM<Player> stateMachine = null;
    PlayerStateData stateData = null;
    LineRenderer ArrowLineRenderer = null;
    Camera cam = null;

    private void FixedUpdate()
    {
        ApplyVelocity();

        if (act == Act.Charge)
            DrawTrajectory();
    }

    protected void Awake()
    {
        base.Awake();

        if(TryGetComponent(out ArrowLineRenderer))
        {
            ArrowLineRenderer.positionCount = 2;
            ArrowLineRenderer.enabled = false;
        }
        cam = Camera.main;
    }

    void DrawTrajectory()
    {
        targetPos = Input.mousePosition;
        targetPos = cam.ScreenToWorldPoint(targetPos);

        GetDirVector();

        float limitAngle = bowData.ShotAngle;
        if (angle > limitAngle)
        {
            limitAngle *= Mathf.Deg2Rad;

            if (arrowDir.y < 0)
                limitAngle = -limitAngle;

            arrowDir = new Vector2(Mathf.Cos(limitAngle), Mathf.Sin(limitAngle));
        }
        targetPos = start + arrowDir * 100;

        ArrowLineRenderer.SetPosition(0, start);
        ArrowLineRenderer.SetPosition(1, targetPos);
        ArrowLineRenderer.enabled = true;
    }

    public void PlayerInitialize() 
    {
        gameObject.SetActive(true);
        Initialize();
        speed = speedValue;
        
        stateData = ScriptableObject.CreateInstance<PlayerStateData>();
        stateData.SetData(idle, move, charge, shot);
        SetData();
    }

    public void WaveStart()
    {
        gameObject.SetActive(true);
        SetData();
        useArrowIndex = 2;
        transform.position = new Vector2(-10.0f, -1.0f);
        bow.WaveStart();
    }

    public void WaveClear()
    {
        gameObject.SetActive(false);
    }

    bool SetData()
    {
        if(null == stateData)
        {
            ErrorMessage("Player State Data가 Null 입니다.");
            return false;
        }

        if (null == stateMachine)
            stateMachine = new FSM<Player>(this);

        if(!stateMachine.SetCurrState(stateData.IdleState))
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
            stateMachine.Update();
            yield return new WaitForSeconds(updateTime);
        }
    }

    public void MoveAction(InputAction.CallbackContext context)
    {
        Debug.Log("Move + "+velocity);

        if ((act == Act.Idle || act == Act.Move) && !GameMgr.Instance.IsPause)
        {
            dir = context.ReadValue<Vector2>();
            OnMoveState();
        }
    }

    public void StopAction(InputAction.CallbackContext context)
    {
        if (act == Act.Move && !GameMgr.Instance.IsPause)
            OnIdleState();
    }

    public void ChargeAction(InputAction.CallbackContext context)
    {
        if(act != Act.Move && !GameMgr.Instance.IsPause)
            OnChargeState();
    }

    public void ShotAction(InputAction.CallbackContext context)
    {
        if(act == Act.Charge && !GameMgr.Instance.IsPause)
            OnShotState();
    }

    public void MountArrow2(ArrowData arrowData, int count = 0)
    {
        if (arrowData)
        {
            ItemMgr.Instance.PlusItem(Arrows[1].ID, ArrowCount[1]);
            ArrowCount[1] = count;
            Arrows[1] = arrowData;
        }
    }

    public void ChangeArrow1Action(InputAction.CallbackContext context)
    {
        if (act == Act.Idle || act == Act.Move)
        {
            if (Arrows[useArrowIndex] != Arrows[0] && ArrowCount[0] > 0)
            {
                useArrowIndex = 0;
                InterfaceMgr.Instance.UseArrow1();
            }
            else
            {
                useArrowIndex = 2;
                InterfaceMgr.Instance.UseNormalArrow();
            }
        }
    }

    public void ChangeArrow2Action(InputAction.CallbackContext context)
    {
        if(act == Act.Idle || act == Act.Move)
        {
            if (Arrows[useArrowIndex] != Arrows[1] && ArrowCount[1] > 0)
            {
                useArrowIndex = 1;
                InterfaceMgr.Instance.UseArrow2();
            }
            else
            {
                useArrowIndex = 2;
                InterfaceMgr.Instance.UseNormalArrow();
            }
                
        }
    }

    public void LineRendererEnable()
    {
        ArrowLineRenderer.enabled = false;
    }

    public override void OnIdleState()
    {
        act = Act.Idle;
        if (!stateMachine.ChangeState(stateData.IdleState))
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

    public void GameOver()
    {
        gameObject.SetActive(false);
    }
}