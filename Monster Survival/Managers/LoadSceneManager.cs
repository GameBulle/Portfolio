using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadSceneManager : MonoBehaviour
{
    static LoadSceneManager instance;
    public static LoadSceneManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<LoadSceneManager>();
                if (!instance)
                    instance = new GameObject("LoadSceneManager").AddComponent<LoadSceneManager>();
            }
            return instance;
        }
    }

    [SerializeField] Slider loadingSlider;
    [SerializeField] TextMeshProUGUI percent;
    [SerializeField] GameObject loadingUI;

    string loadSceneName;

    private void Awake()
    {
        if (Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
    }

    public void LoadScene(string sceneName)
    {
        loadingSlider.maxValue = 1f;
        loadingSlider.value = 0f;
        percent.text = "0%";
        loadingUI.SetActive(true);
        SceneManager.sceneLoaded += OnSceneLoaded;
        loadSceneName = sceneName;
        StartCoroutine(LoadSceneProcess());
    }

    IEnumerator LoadSceneProcess()
    {
        AsyncOperation op = SceneManager.LoadSceneAsync(loadSceneName);
        op.allowSceneActivation = false;

        float timer = 0f;
        while(!op.isDone)
        {
            yield return null;
            if (op.progress < 0.9f)
            {
                loadingSlider.value = op.progress;
                percent.text = string.Format("{0:P0}", loadingSlider.value);
            }    
            else
            {
                timer += Time.unscaledDeltaTime;
                loadingSlider.value = Mathf.Lerp(0.9f, 1f, timer);
                percent.text = string.Format("{0:P0}", loadingSlider.value);
                if (loadingSlider.value >= 1f)
                {
                    op.allowSceneActivation = true;
                    yield break;
                }
            }
        }
    }

    private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        if(arg0.name == loadSceneName)
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
            loadingUI.SetActive(false);
        }
    }
}
