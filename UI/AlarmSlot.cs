using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AlarmSlot : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI character_name_text;
    Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    public void SetAlarmData(CharacterData data)
    {
        anim.runtimeAnimatorController = data.Anim;
        anim.SetTrigger("Alarm");
        character_name_text.text = data.CharacterName;
    }
}
