using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordDamageUp : Passive
{
    public override void AbilityUp()
    {
        GameManager.Instance.Weapon.WeaponDamageUp(type, value);
    }
}
