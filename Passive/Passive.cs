using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Passive : MonoBehaviour
{
    protected int id;
    protected PassiveData.ApplicationType type;
    protected int level;
    protected float value;

    public int ID => id;
    public int Level => level;

    public void Initialize(PassiveData data)
    {
        level = 0;
        type = data.Apply;
        name = data.SkillName;
        value = data.LevelPerValue(level);
        transform.parent = GameManager.Instance.Player.transform.Find("Passive").transform;

        id = (int)data.ID;
        LevelUp(value);
    }

    public void LevelUp(float value) 
    { 
        level++; 
        this.value = value;
        AbilityUp();
    }
    public virtual void AbilityUp() { }
}
