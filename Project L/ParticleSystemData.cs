using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Effect Data", menuName = "ScriptableObject/Effect/Effect", order = 0)]
public class ParticleSystemData : ScriptableObject
{
    [SerializeField] string effectName;
    [SerializeField] ParticleSystem effect;
    public event System.Action<ParticleSystem> OnEvent = null;

    public string EffectName => effectName;
    public ParticleSystem Effect => effect;
}
