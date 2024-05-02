using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelectAnimEvent : MonoBehaviour
{
    [SerializeField] Image image;

    CharacterData.CharacterType type;
    RectTransform image_rect;

    public void Init() { image_rect = image.GetComponent<RectTransform>(); }

    public void SetType(CharacterData.CharacterType type) { this.type = type; }

    public void PlaySFX()
    {
        string clip_name = "";
        switch(type)
        {
            case CharacterData.CharacterType.Warrior:
                clip_name = "Warrior_Select";
                break;
            case CharacterData.CharacterType.Wizard:
                clip_name = "Wizard_Select";
                break;
            case CharacterData.CharacterType.Samurai:
                clip_name = "Samurai_Select";
                break;
            case CharacterData.CharacterType.Shaman:
                clip_name = "Shaman_Select";
                break;
        }
        AudioManager.Instance.PlayCharacterSelectSFX(clip_name);
    }

    public void StopSFX() { AudioManager.Instance.StopCharacterSelectSFX(); }

    public void Hide() { image_rect.localScale = Vector3.zero; }

    public void Show() { image_rect.localScale = Vector3.one; }
}
