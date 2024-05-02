using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordSpeedUp : Passive
{
    public override void AbilityUp()
    {
        GameManager.Instance.Weapon.WeaponSpeedUp(type, value);
    }
}
