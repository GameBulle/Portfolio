using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicScaleUp : Passive
{
    public override void AbilityUp()
    {
        GameManager.Instance.Weapon.WeaponScaleUp(type, value);
    }
}
