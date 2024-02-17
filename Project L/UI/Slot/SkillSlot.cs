using UnityEngine;

public class SkillSlot : MonoBehaviour
{
    int[] skillIDs;
    Player player;
    int capacity;

    public SkillSlot(Player player, int[] skillIDs = null)
    {
        this.player = player;
        InterfaceMgr.Instance.SetSkillSlot(this);
        
        if (skillIDs == null)
        {
            capacity = InterfaceMgr.Instance.GetSkillSlotSize();
            this.skillIDs = new int[capacity];
            InitSkills();
        }
        else
        {
            this.skillIDs= skillIDs;
            capacity = skillIDs.Length;
            for (int i=0;i<skillIDs.Length;i++)
                InterfaceMgr.Instance.SetSkill(skillIDs[i], i);
        } 
    }

    public void DragSkillUIToQuick(int id, int index)
    {
        skillIDs[index] = id;
    }

    public void DragQuickToQuick(int startIndex, int endIndex)
    {
        skillIDs[endIndex] = skillIDs[startIndex];
        skillIDs[startIndex] = -1;
    }

    public bool Use(int index)
    {
        if (skillIDs[index] == -1)
            return false;
        return SkillMgr.Instance.GetSkill(skillIDs[index]).Use(player);
    }

    public void InitSkills()
    {
        for (int i = 0; i < skillIDs.Length; i++)
            skillIDs[i] = -1;
        
    }

    public int GetSkillID(int index) { return skillIDs[index]; }
    public int[] GetAllSkills() { return skillIDs; }
}
