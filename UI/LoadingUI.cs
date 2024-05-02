using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingUI : MonoBehaviour
{
    [SerializeField] Slider loading_slider;
    [SerializeField] TextMeshProUGUI percent_text;

    public void ShowLoadingUI(CharacterData data)
    {
        loading_slider.maxValue = 1f;
        loading_slider.value = 0f;
        percent_text.text = "0%";
        gameObject.SetActive(true);

        StartCoroutine(OnLoading(data));
    }

    IEnumerator OnLoading(CharacterData data)
    {
        while(true)
        {
            loading_slider.value = Mathf.Lerp(loading_slider.value, loading_slider.maxValue, Time.deltaTime * 10f);
            percent_text.text = string.Format("{0:P0}", loading_slider.value);

            if (percent_text.text.Equals("100 %"))
                break;
            yield return new WaitForFixedUpdate();
        }
        
        if(data == null)
            SceneManager.LoadScene(0);
        else
        {
            yield return new WaitForSeconds(1.0f);
            GameManager.Instance.SetGame(data);
            gameObject.SetActive(false);
        }
    }
}
