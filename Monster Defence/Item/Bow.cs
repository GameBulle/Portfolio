using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Bow : MonoBehaviour
{
    [Header("Bow")]
    [SerializeField] Transform shotPos;
    [SerializeField] Slider chargeSlider;
    float currPenetration = 0.0f;
    float maxPenetration = 0.0f;
    float animationChargeValue;
    bool maxCharge = false;

    enum State { Ready,Charge,Shot }
    State state;

    BowData bowData;
    ArrowData arrowData;

    public Vector2 ShotPos { get { return shotPos.position; } }

    public bool NPCShot { get { return (currPenetration >= maxPenetration); } }

    public void Initialize(BowData bowData)
    {
        this.bowData = bowData;
    }

    private void FixedUpdate()
    {
        if (state == State.Charge)
        {
            ChargeGauge();
        }
    }

    public void WaveStart()
    {
        state = State.Ready;
        animationChargeValue = 0f;
        chargeSlider.value = animationChargeValue;
        chargeSlider.gameObject.SetActive(false);
    }

    void ChargeGauge()
    {
        animationChargeValue += 0.01f * bowData.ChargeSpeed;

        if(animationChargeValue >= 1.0f && !maxCharge)
        {
            SoundMgr.Instance.PlusPullSoundPlay();
            currPenetration++;
            if (currPenetration == maxPenetration)
            {
                animationChargeValue = 1.0f;
                maxCharge = true;
            }
            else
                animationChargeValue = 0.0f;
        }

        chargeSlider.value = animationChargeValue;
    }

    public void Charge(ArrowData arrowData, int playType = 0)
    {
        if (state == State.Ready)
        {
            chargeSlider.gameObject.SetActive(true);

            SoundMgr.Instance.PullSoundPlay();
            this.arrowData = arrowData;
            currPenetration = arrowData.Penetration;
            if (playType == 0)
                maxPenetration = arrowData.MaxPenetration;
            else if(playType == 1)
                maxPenetration = UnityEngine.Random.Range((int)currPenetration, (int)arrowData.MaxPenetration + 1);
            animationChargeValue = 0.0f;

            state = State.Charge;
        }
    }

    public void Shot(Vector2 dir, float damage)
    {
        if (state == State.Charge)
        {
            SoundMgr.Instance.ShotSoundPlay();
            ArrowSpawner.Instance.SpawnArrow(dir, shotPos.position, damage, currPenetration, bowData.Speed);
            chargeSlider.gameObject.SetActive(false);
            maxCharge = false;
            state = State.Ready;
        }
    }
}
