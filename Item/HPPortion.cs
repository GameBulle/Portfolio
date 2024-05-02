using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPPortion : Item
{
    [SerializeField] float value;

    protected override void Use()
    {
        GameManager.Instance.HPRecovery(value);
        AudioManager.Instance.PlaySFX("Heal");
    }
}
