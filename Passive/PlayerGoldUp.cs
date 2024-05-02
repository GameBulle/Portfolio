using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGoldUp : Passive
{
    public override void AbilityUp()
    {
        GameManager.Instance.GoldUp = (int)value;
    }
}
