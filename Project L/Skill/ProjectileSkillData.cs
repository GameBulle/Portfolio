using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ProjectileSkillData : SkillData
{
    [SerializeField] float speed = 0f;
    [SerializeField] float range = 0f;

    public float Speed => speed;
    public float Range => range;

    override public bool Use(Player player) { return true; }
}
