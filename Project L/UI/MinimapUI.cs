using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MinimapUI : MonoBehaviour
{
    [SerializeField] Camera minimapCamera;
    [SerializeField] float zoomMin = 90f;
    [SerializeField] float zoomMax = 210f;
    [SerializeField] float zoomOneStep = 30f;
    [SerializeField] TextMeshProUGUI mapName;

    private void Start()
    {
        this.mapName.text = "할슈타트 마을";
    }

    public void ZoomIn()
    {
        minimapCamera.orthographicSize = Mathf.Max(minimapCamera.orthographicSize - zoomOneStep, zoomMin);
    }

    public void ZoomOut()
    {
        minimapCamera.orthographicSize = Mathf.Min(minimapCamera.orthographicSize + zoomOneStep, zoomMax);
    }

    public void ChangeCurrMapName(int currMapID)
    {
        this.mapName.text = MapMgr.Instance.GetMapData(currMapID).MapName;
    }
}
