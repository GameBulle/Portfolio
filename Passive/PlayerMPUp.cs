using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMPUp : Passive
{
    public override void AbilityUp()
    {
        GameManager.Instance.MP_Plus = value;
    }
}
