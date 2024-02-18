using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NPC Data", menuName = "ScriptableObject/NPC Data", order = 1)] 
public class NPCData : ScriptableObject
{
    [Header("NPC Stats")]
    [SerializeField] LayerMask targetLayer;
    [SerializeField] float searchRange = 0.0f;
    [SerializeField] float moveSpeed = 0.0f;

    public LayerMask TargetLayer => targetLayer;
    public float SearchRange => searchRange;
    public float MoveSpeed => moveSpeed;
}

public class NPCStateData : ScriptableObject
{
    public IState<NPC> IdleState { get; private set; }
    public IState<NPC> MoveState { get; private set; }
    public IState<NPC> ShotState { get; private set; }
    public IState<NPC> ChargeState { get; private set; }

    public void SetData(IState<NPC> idle, IState<NPC> move, IState<NPC> shot, IState<NPC> charge)
    {
        IdleState = idle;
        MoveState = move;
        ShotState = shot;
        ChargeState = charge;
    }
}