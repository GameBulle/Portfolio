using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class SkillData : ScriptableObject
{
    [SerializeField] int id;
    [SerializeField] string skillName;
    [SerializeField] int level;
    [SerializeField] Sprite icon;
    [SerializeField] string toolTip;
    [SerializeField] float coolTime;

    [Header("Level per Damage")]
    [SerializeField] float damage;

    [SerializeField] float useMP;

    public int Id => id;
    public string SkillName => skillName;
    public int Level => level;
    public Sprite Icon => icon;
    public string ToolTip => toolTip;
    public float CoolTime => coolTime;
    public float Damage => damage;
    public float UseMP => useMP;

    public bool IsActive => level > 0;

    public void SkillLevelUp() { level++; }
    public void SkillLevelDown() { if (IsActive) level--; }
    public void SkillInit(int level = 0) { this.level = level; }

    abstract public bool Use(Player player);
}