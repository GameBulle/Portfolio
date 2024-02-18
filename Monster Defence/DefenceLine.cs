using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DefenceLine : MonoBehaviour, IDamageable
{
    public int MaxHP { get; set; }
    public int Level { get; set; }
    public float HP { get; set; }

    public float startHP { get; set; }
    public float lostHP { get; set; }

    public bool isDead { get { return HP <= 0; } }

    public void Initialize()
    {
        Level = 1;
        MaxHP = 100 + (Level - 1) * 50;
        HP = MaxHP;
    }

    public virtual int OnDamage(float atk, string HitPart)
    {
        HP = Mathf.Max(HP - atk, 0);
        InterfaceMgr.Instance.HpSlider();

        if (isDead)
            GameMgr.Instance.GameOver();

        return 0;
    }

    public virtual void RestoreHealth(float value)
    {
        HP = Mathf.Clamp(HP + value, 0, MaxHP);
    }

    public void LevelUp()
    {
        Level++;
        MaxHP = 100 + (Level - 1) * 50;
        HP = MaxHP;
        InterfaceMgr.Instance.SetHpSlider();
    }

    public void WaveStart()
    {
        startHP = HP;
        lostHP = 0;
        InterfaceMgr.Instance.SetHpSlider();
    }

    public void WaveClear()
    {
        lostHP = startHP - HP;
    }
}
