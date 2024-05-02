using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEXPUp : Passive
{
    public override void AbilityUp()
    {
        GameManager.Instance.EXPUp = value;
    }
}
