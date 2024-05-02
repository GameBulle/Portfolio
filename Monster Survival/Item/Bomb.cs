using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : Item
{
    protected override void Use()
    {
        GameManager.Instance.OnCleaner();
        AudioManager.Instance.PlaySFX("Bomb");
    }
}
