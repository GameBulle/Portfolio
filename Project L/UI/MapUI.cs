using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.UIElements;
using Unity.VisualScripting;
using System;
using System.Reflection;

public class MapUI : MonoBehaviour
{
    [SerializeField] UnityEngine.UI.Image mapImageArea;
    [SerializeField] RectTransform mapImageContent;
    [SerializeField] RectTransform mapContent;
    [SerializeField] Transform mapNameArea;
    [SerializeField] RectTransform viewPort;

    [SerializeField] MapListUI mapListUI;

    [SerializeField] UnityEngine.UI.Image playerIcon;

    [SerializeField] UnityEngine.UI.Slider zoomSlider;
    [SerializeField] TextMeshProUGUI zoomText;
    [SerializeField] float zoomStep;

    public enum iconType { Player, StoneStatue, PortionShop, EquipmentShop }

    MapListUI[] lists;
    float increaseHeight;
    Vector2 sizeDelta;
    float sliderValue;

    private void Update()
    {
        if (sliderValue != zoomSlider.value)
            UpdateMapSize();
    }

    public void Initialize(MapData[] mapDatas)
    {
        gameObject.SetActive(true);
        lists = new MapListUI[mapDatas.Length];
        increaseHeight = 70f;
        int size = mapDatas.Length;
        for(int i=0;i<size;i++)
        {
            MapListUI ui = Instantiate(mapListUI, mapNameArea.transform);
            ui.Initialize(mapDatas[i].MapName);
            ui.SetPosition(new Vector3(0, -increaseHeight * i));
            ui.Index = i;
            ui.OnClickEvent += (ui) => ClickMapName(ui.Index);
            lists[i] = ui;
        }

        lists[0].RemoveBlind();
        lists[0].SelectMap();
        sliderValue = zoomSlider.value;
        sizeDelta = new Vector2(mapImageContent.sizeDelta.x, mapImageContent.sizeDelta.y);
        UpdateMapSize();
        //PlayerIconToCenter();
        gameObject.SetActive(false);
    }

    void ClickMapName(int index)
    {
        if (lists[index].CheckBlind())
            return;

        for (int i=0;i<lists.Length;i++)
        {
            if (lists[i].Index == index)
                continue;

            lists[i].UnSelectMap();
        }

        if(!SceneManager.GetActiveScene().name.Equals(MapMgr.Instance.GetMapData(index).MapName))
            playerIcon.gameObject.SetActive(false);
        else
            playerIcon.gameObject.SetActive(true);

        mapImageArea.sprite = MapMgr.Instance.GetMapData(index).MapImange;
    }

    void UpdateMapSize()
    {
        zoomText.text = string.Format("{0:P0}", zoomSlider.value);
        mapImageContent.localScale = new Vector3(1f + zoomSlider.value, 1f + zoomSlider.value, 0f);
        mapContent.sizeDelta = new Vector2(this.sizeDelta.x * (1f + zoomSlider.value), this.sizeDelta.y * (1f + zoomSlider.value));
        sliderValue = zoomSlider.value;
    }

    public void LoadScene(int mapID)
    {
        lists[mapID].SelectMap();
    }

    public void OpenMapUI(Transform playerTr)
    {
        gameObject.SetActive(true);
        playerIcon.gameObject.SetActive(true);
        playerIcon.rectTransform.anchoredPosition = new Vector2(500f - playerTr.position.x, 500f - playerTr.position.z);
        playerIcon.rectTransform.eulerAngles = new Vector3(0f, 0f, 180f - playerTr.eulerAngles.y);

        //PlayerIconToCenter();
    }

    public void ChangeCurrMap(int currMapID)
    {
        playerIcon.gameObject.SetActive(false);
        for (int i = 0; i < lists.Length; i++)
        {
            if (lists[i].Index == currMapID)
            {
                lists[i].RemoveBlind();
                lists[i].SelectMap();
                continue;
            }

            lists[i].UnSelectMap();
        }

        mapImageArea.sprite = MapMgr.Instance.GetMapData(currMapID).MapImange;
    }

    void PlayerIconToCenter()
    {
        viewPort.anchoredPosition = new Vector2(-playerIcon.rectTransform.anchoredPosition.x,-playerIcon.rectTransform.anchoredPosition.y);
    }

    public void ZoomIn()
    {
        zoomSlider.value += zoomStep;
        zoomSlider.value = Mathf.Clamp(zoomSlider.value, 0f, 100f);
        UpdateMapSize();
    }

    public void ZoomOut()
    {
        zoomSlider.value -= zoomStep;
        zoomSlider.value = Mathf.Clamp(zoomSlider.value, 0f, 100f);
        UpdateMapSize();
    }

    private void OnEnable()
    {
        SoundMgr.Instance.PlaySFXAudio("UI_Open");
        UnityEngine.Cursor.visible = true;
        UnityEngine.Cursor.lockState = CursorLockMode.None;
        GameMgr.Instance.OpenUICount++;
    }

    private void OnDisable()
    {
        GameMgr.Instance.OpenUICount--;
        if (GameMgr.Instance.OpenUICount == 0)
        {
            UnityEngine.Cursor.visible = false;
            UnityEngine.Cursor.lockState = CursorLockMode.Locked;
        }
    }
}
