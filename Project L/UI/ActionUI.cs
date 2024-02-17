using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ActionUI : MonoBehaviour
{
    [SerializeField] Slider slider;
    [SerializeField] TextMeshProUGUI actionText;
    [SerializeField] TextMeshProUGUI actionTime;
    Coroutine coroutine;
    ActionObject actionObject;

    public void ActionInfo(ActionObject action)
    {
        gameObject.SetActive(true);
        actionObject = action;
        slider.maxValue = actionObject.ActionTime;
        slider.value = 0f;
        this.actionText.text = actionObject.ActionText;
        if (coroutine != null)
            StopCoroutine(coroutine);
        StartCoroutine(OnActionProcess());
    }

    IEnumerator OnActionProcess()
    {
        float t = 1 / actionObject.ActionTime;
        float time = 0;
        while(true)
        {
            slider.value = Mathf.Lerp(0f, slider.maxValue, time);
            slider.value = Mathf.Clamp(slider.value, 0f, slider.maxValue);
            this.actionTime.text = string.Format("{0:F2}", (slider.maxValue - slider.value));
            time += t*Time.deltaTime;
            
            if(slider.value == slider.maxValue)
            {
                actionObject.Action();
                break;
            }

            yield return null;
        }

        gameObject.SetActive(false);
    }
}
