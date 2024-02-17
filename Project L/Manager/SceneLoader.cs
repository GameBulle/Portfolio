using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using TMPro;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] GameObject sceneLoadUI;
    [SerializeField] CanvasGroup canvasGroup;
    [SerializeField] Image progressBar;
    [SerializeField] TextMeshProUGUI percent;
    [SerializeField] PortalData startData;
    [SerializeField] Vector3 startPos;
    [SerializeField] Vector3 startAngle;

    PortalData data;
    PlayerSaveData saveData;
    bool isGameStart;
    bool loadMaintTitle;

    static SceneLoader instance = null;

    public static SceneLoader Instance
    {
        get
        {
            if(instance == null)
            {
                instance = FindObjectOfType<SceneLoader>();
                if(!instance)
                    instance = new GameObject("SceneLoader").AddComponent<SceneLoader>();
                DontDestroyOnLoad(instance.gameObject);
            }
            return instance;
        }
    }

    private void Awake()
    {
        if(this != Instance)
            Destroy(gameObject);
        isGameStart = false;
        loadMaintTitle = false;
        sceneLoadUI.SetActive(false);
    }

    public void LoadMainTitle()
    {
        sceneLoadUI.SetActive(true);
        loadMaintTitle = true;
        SceneManager.sceneLoaded += OnSceneLoaded;
        GameMgr.Instance.DestroyPlayer();
        StartCoroutine(OnLoadSceneProcess());
    }

    public void LoadScene(PortalData data)
    {
        sceneLoadUI.SetActive(true);
        isGameStart = false;
        ItemBoxMgr.Instance.AllRetunPool();
        MonsterMgr.Instance.AllRetunPool();
        SceneManager.sceneLoaded += OnSceneLoaded;
        this.data = data;
        StartCoroutine(OnLoadSceneProcess());
    }


    public void StartGame(PlayerSaveData saveData = null)
    {
        sceneLoadUI.SetActive(true);
        if(saveData != null)
            this.saveData = saveData;
        isGameStart = true;
        SceneManager.sceneLoaded += OnSceneLoaded;
        StartCoroutine(OnLoadSceneProcess());
    }

    IEnumerator OnLoadSceneProcess()
    {
        progressBar.fillAmount = 0f;
        percent.text = "0";
        yield return StartCoroutine(Fade(true));

        AsyncOperation op;

        if (loadMaintTitle)
            op = SceneManager.LoadSceneAsync("Main");
        else if (!isGameStart)
            op = SceneManager.LoadSceneAsync(MapMgr.Instance.GetMapData(data.NextMapID).MapName);
        else if(saveData == null)
            op = SceneManager.LoadSceneAsync("할슈타트 마을");
        else
            op = SceneManager.LoadSceneAsync(MapMgr.Instance.GetMapData(saveData.MapID).MapName);

        op.allowSceneActivation = false;

        float timer = 0f;

        while (op.progress < 0.9f)
        {
            yield return null;

            timer += Time.deltaTime;
            progressBar.fillAmount = Mathf.Lerp(0f, 0.9f, timer);
            percent.text = String.Format("{0:P}", progressBar.fillAmount);
        }

        progressBar.fillAmount = 1f;
        percent.text = String.Format("{0:P}", progressBar.fillAmount);

        yield return new WaitForSecondsRealtime(0.5f);

        op.allowSceneActivation = true;

        //while(!op.isDone)
        //{
        //    yield return null;
        //    if (op.progress < 0.9f)
        //        progressBar.fillAmount = op.progress;
        //    else
        //    {
        //        timer += Time.unscaledDeltaTime;
        //        progressBar.fillAmount = Mathf.Lerp(0.9f, 1f, timer);
        //        if (progressBar.fillAmount >= 1f)
        //        {
        //            op.allowSceneActivation = true;
        //            yield break;
        //        }
        //    }
        //    percent.text = String.Format("{0:P}", progressBar.fillAmount);
        //}
    }

    private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        if(isGameStart)
        {
            StartCoroutine(Fade(false));
            SceneManager.sceneLoaded -= OnSceneLoaded;
            GameMgr.Instance.Initialize(saveData);
            if(saveData == null)
            {
                GameMgr.Instance.SetPlayerPos(startPos, startAngle);
            }
            else
            {
                MonsterMgr.Instance.SetMonsters(saveData.MapID);
                InterfaceMgr.Instance.ChangeCurrMap(saveData.MapID);
                GameMgr.Instance.SetPlayerPos(saveData.PlayerPos, saveData.PlayerAngle);
            }
            
            isGameStart = false;
        }
        else if(loadMaintTitle)
        {
            StartCoroutine(Fade(false));
            SceneManager.sceneLoaded -= OnSceneLoaded;
            loadMaintTitle = false;
            InterfaceMgr.Instance.LoadMainTile();
        }
        else
        {
            if (arg0.name.Equals(MapMgr.Instance.GetMapData(data.NextMapID).MapName))
            {
                StartCoroutine(Fade(false));
                SceneManager.sceneLoaded -= OnSceneLoaded;
                //TreasureBoxMgr.Instance.SetBoxes(data.NextMapID);
                MonsterMgr.Instance.SetMonsters(data.NextMapID);
                InterfaceMgr.Instance.ChangeCurrMap(data.NextMapID);
                GameMgr.Instance.SetPlayerPos(data.PlayerPos, data.PlayerAngle);
                GameMgr.Instance.ActivePlayer();

                InterfaceMgr.Instance.SetTextUI("자동 저장 중...", 3.0f);
                PlayerSaveData saveData = GameMgr.Instance.AutoSave();
                saveData.PlayerPos = data.PlayerPos;
                DataMgr.Instance.SavePlayerData(0, saveData);
            }
        }
    }

    IEnumerator Fade(bool isFadeIn)
    {
        float timer = 0f;
        while (timer <= 1f)
        {
            yield return null;
            timer += Time.unscaledDeltaTime * 3f;
            canvasGroup.alpha = isFadeIn ? Mathf.Lerp(0f, 1f, timer) : Mathf.Lerp(1f, 0f, timer);
        }

        if (!isFadeIn)
            sceneLoadUI.SetActive(false);
    }
}
