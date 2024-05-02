using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpeedUp : Passive
{
    public override void AbilityUp()
    {
        GameManager.Instance.Player.SpeedUp(value);
    }
}
