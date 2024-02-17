using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Portal Data", menuName = "ScriptableObject/Portal", order = 0)]
public class PortalData : ScriptableObject
{
    [SerializeField] protected int nextMapID;
    [SerializeField] protected Vector3 playerPos;
    [SerializeField] protected Vector3 playerAngle;

    public int NextMapID => nextMapID;
    public Vector3 PlayerPos => playerPos;
    public Vector3 PlayerAngle => playerAngle;
}
