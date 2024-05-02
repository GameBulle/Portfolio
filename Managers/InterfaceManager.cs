using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InterfaceManager : MonoBehaviour
{
    static InterfaceManager instance;
    public static InterfaceManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<InterfaceManager>();
                if (!instance)
                    instance = new GameObject("InterfaceManager").AddComponent<InterfaceManager>();
            }
            return instance;
        }
    }

    [SerializeField] HUD hud;
    [SerializeField] LevelUp levelUp;
    [SerializeField] OwnSkill ownSkill;
    [SerializeField] Result[] results;
    [SerializeField] CharacterSelect select;
    [SerializeField] Alarm alarm;
    [SerializeField] Option option;

    private void Awake()
    {
        if (this != Instance)
            Destroy(gameObject);
    }

    public void InitializeOption(OptionData data )
    {
        option.gameObject.SetActive(true);
        option.Initialize(data);
        option.gameObject.SetActive(false);
    }

    public void InitializeHUD(float next, float hp, float mp)
    {
        hud.Initialize(next, hp, mp);
        ownSkill.Initialize();
        hud.gameObject.SetActive(true);
        levelUp.gameObject.SetActive(false);
        for (int i = 0; i < results.Length; i++)
            results[i].gameObject.SetActive(false);
    }

    public void GameVictory() { results[0].gameObject.SetActive(true); }
    public void GameDefeat() { results[1].gameObject.SetActive(true); }
    public void SetOwnSkill(SkillData data) { ownSkill.SetOwnSkill(data); }
    public void UpdateOwnSkillLevel(int id, bool isPassive = false) { ownSkill.UpdateOwnSkillLevel(id, isPassive); }
    public void LevelUp(WeaponData data, int level) { levelUp.SetSkill(data, level); }
    public void LevelUp(PassiveData data, int level) { levelUp.SetSkill(data, level); }
    public void UpdateEXP(float curr, float next) { hud.UpdateEXP(curr, next); }
    public void UpdateLevel(int level) { hud.UpdateLevel(level); }
    public void UpdateGold(int gold) { hud.UpdateGold(gold); }
    public void UpdateHP(float hp, float max) { hud.UpdateHP(hp, max); }
    public void SetAlarmData(CharacterData data) { alarm.SetAlarmData(data); }
}
