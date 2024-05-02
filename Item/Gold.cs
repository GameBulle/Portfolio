using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gold : Item
{
    protected override void Use()
    {
        GameManager.Instance.GetGold();
    }
}
