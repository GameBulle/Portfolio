using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillMgr : MonoBehaviour
{
    [Header("Skills")]
    [SerializeField] SkillData[] skills;

    public int SkillCount => skills.Length;

    static SkillMgr instance = null;
    public static SkillMgr Instance
    {
        get
        {
            if(instance == null)
            {
                instance = FindObjectOfType<SkillMgr>();
                if(!instance)
                    instance = new GameObject("SkillManager").AddComponent<SkillMgr>();
                DontDestroyOnLoad(instance.gameObject);
            }
            return instance;
        }
    }

    private void Awake()
    {
        if (this != Instance)
            Destroy(gameObject);
    }

    public void Initialize(PlayerSaveData data = null)
    {
        Array.Sort(skills, (a, b) => (a.Id < b.Id) ? -1 : 1);

        for(int i=0;i<skills.Length;i++)
        {
            if (data == null)
                SkillInit(i, 0);
            else
                SkillInit(i, data.SkillLevels[i]);
        }
    }

    public void SkillLevelUP(int id)
    {
        int index = Array.FindIndex(skills, a => a.Id == id);
        skills[index].SkillLevelUp();
    }

    public void SkillLevelDown(int id)
    {
        int index = Array.FindIndex(skills, a => a.Id == id);
        skills[index].SkillLevelDown();
    }

    public void SkillInit(int id, int level)
    {
        skills[id].SkillInit(level);
    }

    public SkillData GetSkill(int key) { return skills[key]; }
}
