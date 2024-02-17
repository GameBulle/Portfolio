using System;
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
            if(instance == null)
            {
                instance = FindObjectOfType<EffectMgr>();
                if (!instance)
                    instance = new GameObject("EffectManager").AddComponent<EffectMgr>();
                DontDestroyOnLoad(instance.gameObject);
            }
            return instance;
        }
    }

    private void Awake()
    {
        if (this != Instance)
            Destroy(gameObject);
    }

    [SerializeField] ParticleSystemData[] particleSystems;
    List<ParticleSystem> objectPool;

    public void Initialize()
    {
        objectPool = new();
    }

    ParticleSystem MakeParticleSystem(string effectName)
    {
        for(int i=0;i<particleSystems.Length;i++)
        {
            if (particleSystems[i].EffectName == effectName)
            {
                ParticleSystem effect = Instantiate(particleSystems[i].Effect);
                return effect;
            }
        }

        return null;
    }

    public void PlayParticleSystem(string effectName, Vector3 pos, Vector3 angle = default(Vector3))
    {

        ParticleSystem effect = MakeParticleSystem(effectName);
        effect.gameObject.SetActive(true);
        effect.transform.position = pos;
        if(angle != default(Vector3))
            effect.transform.eulerAngles = angle;
        effect.Play();
    }
}
