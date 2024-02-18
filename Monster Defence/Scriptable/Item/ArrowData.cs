using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Arrow Data", menuName = "ScriptableObject/Arrow Data",order = 1)]
public class ArrowData : ItemData
{
    [Header("Stats")]
    [SerializeField] float damage = 1f;
    [SerializeField] float penetration = 1f;
    [SerializeField] float maxPenetration = 2f;

    [Header("Icon")]
    [SerializeField] Sprite icon;

    public float Damage => damage;
    public float Penetration => penetration;
    public float MaxPenetration => maxPenetration;
    public Sprite Icon => icon;
}
