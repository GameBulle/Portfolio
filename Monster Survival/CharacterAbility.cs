using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAbility : MonoBehaviour
{
    public static float Warrior
    {
        get { return GameManager.Instance.Character == CharacterData.CharacterType.Warrior ? 1.1f : 1f; }
    }

    public static float Wizard
    {
        get { return GameManager.Instance.Character == CharacterData.CharacterType.Wizard ? 1.1f : 1f; }
    }

    public static float Samurai
    {
        get { return GameManager.Instance.Character == CharacterData.CharacterType.Samurai ? 1.1f : 1f; }
    }

    public static float Shaman_Sword
    {
        get { return GameManager.Instance.Character == CharacterData.CharacterType.Shaman ? 1.05f : 1f; }
    }

    public static float Shaman_Magic
    {
        get { return GameManager.Instance.Character == CharacterData.CharacterType.Shaman ? 1.05f : 1f; }
    }
}
