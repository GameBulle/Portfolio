using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordScaleUp : Passive
{
    public override void AbilityUp()
    {
        GameManager.Instance.Weapon.WeaponScaleUp(type, value);
    }
}
