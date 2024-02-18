using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectMgr : MonoBehaviour
{
    static EffectMgr instance = null;

    public static EffectMgr Instance
    {
        get
        {
            if(null == instance)
            {
                instance = FindAnyObjectByType<EffectMgr>();
                if (!instance)
                    instance = new GameObject("EffectMgr").AddComponent<EffectMgr>();

                DontDestroyOnLoad(instance.gameObject);
            }

            return instance;
        }
    }

    [Header("Monster Die Effect")]
    [SerializeField] ParticleSystem dieEffect;

    Transform effectConatiner;
    List<ParticleSystem> objectPool;

    private void Awake()
    {
        if (this != Instance)
            Destroy(gameObject);

        effectConatiner = new GameObject("Effect Pool Container : Smoke").transform;
        objectPool = new List<ParticleSystem>();
    }

    ParticleSystem MakeEffect()
    {
        ParticleSystem effect = Instantiate(dieEffect, effectConatiner);
        objectPool.Add(effect);
        return effect;
    }

    ParticleSystem GetEffect()
    {
        for (int i = 0; i < objectPool.Count; i++)
        {
            if (!objectPool[i].isPlaying)
                return objectPool[i];
        }

        return MakeEffect();
    }

    public void PlayDieEffect(Vector2 pos)
    {
        ParticleSystem effect = GetEffect();
        effect.gameObject.SetActive(true);
        effect.transform.position = pos;
        effect.Play();
        SoundMgr.Instance.EffectSoundPlay();
    }
}
