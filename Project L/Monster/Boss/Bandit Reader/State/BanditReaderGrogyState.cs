using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Grogy State", menuName = "ScriptableObject/Boss FSM State/Bandit Reader/Grogy", order = 9)]
public class BanditReaderGrogyState : ScriptableObject, IState<BanditReader>
{
    public void Enter(BanditReader owner)
    {
        Debug.Log("Grogy State!");
        owner.Grogy();
    }

    public void Excute(BanditReader owner)
    {

    }

    public void Exit(BanditReader owner)
    {

    }
}
