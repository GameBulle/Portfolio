using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MPPortion : Item
{
    [SerializeField] float value;

    protected override void Use()
    {
        GameManager.Instance.CurrMP = value;
    }
}
