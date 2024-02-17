using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Skill State", menuName = "ScriptableObject/Player FSM State/Skill", order = 9)]
public class PlayerSkillState : ScriptableObject, IState<Player>
{
    public void Enter(Player owner)
    {
        owner.act = Player.Act.skill;
        owner.Stop();
        owner.SkillAnim();
    }

    public void Excute(Player owner)
    {

    }

    public void Exit(Player owner)
    {

    }
}
