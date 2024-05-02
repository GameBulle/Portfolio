using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magnet : Item
{
    protected override void Use()
    {
        GameManager.Instance.Player.Scanner.GetMagnet();
        AudioManager.Instance.PlaySFX("Magnet");
    }
}
