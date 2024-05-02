using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Alarm : MonoBehaviour
{
    [SerializeField] AlarmSlot slot;
    Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    public void SetAlarmData(CharacterData data)
    {
        AudioManager.Instance.PlaySFX("Alarm");
        gameObject.SetActive(true);
        slot.SetAlarmData(data);
        anim.Rebind();
    }

    public void OffGameObject() { gameObject.SetActive(false); }
}
