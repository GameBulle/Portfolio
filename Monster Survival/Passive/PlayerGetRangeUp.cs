using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGetRangeUp : Passive
{
    public override void AbilityUp()
    {
        GameManager.Instance.Player.Scanner.ItemRangeUp(value);
    }
}
