using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI textUI;
    Coroutine coroutine;

    public void SetTextUI(string text, float time)
    {
        gameObject.SetActive(true);
        textUI.text = text;
        if(coroutine != null)
            StopCoroutine(coroutine);
        coroutine = StartCoroutine(OnTextUI(time));
    }

    IEnumerator OnTextUI(float time)
    {
        yield return new WaitForSeconds(time);

        gameObject.SetActive(false);
    }
}
