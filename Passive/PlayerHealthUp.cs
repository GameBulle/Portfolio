using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthUp : Passive
{
    public override void AbilityUp()
    {
        GameManager.Instance.HPUp(value);
    }
}
