using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Bow Data", menuName = "ScriptableObject/Bow Data",order = 1)]
public class BowData : ItemData
{
    [Header("Stats")]
    [SerializeField] float timeBetShot = 2.0f;
    [SerializeField] float chargeSpeed = 1f;
    [SerializeField] float shotAngle = 15f;
    [SerializeField] float speed = 0f;

    [Header("Icon")]
    [SerializeField] Sprite icon;

    public float TimeBetShot => timeBetShot;
    public float ChargeSpeed => chargeSpeed;
    public float ShotAngle => shotAngle;
    public float Speed => speed;
    public Sprite Icon => icon;
}
