using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MapListUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI mapName;
    [SerializeField] Image seletedImage;
    [SerializeField] Image blindImage;

    public int Index { get; set; }
    public RectTransform RectTr { get; private set; }
    public event System.Action<MapListUI> OnClickEvent = null;

    public void Initialize(string mapName)
    {
        RectTr = GetComponent<RectTransform>();

        this.mapName.text = mapName;
        seletedImage.gameObject.SetActive(false);
    }

    public void SetPosition(Vector3 pos)
    {
        RectTr.anchoredPosition = new Vector2(0f, pos.y);
    }

    public void SelectMap()
    {
        if (blindImage.gameObject.activeSelf)
            return;
        seletedImage.gameObject.SetActive(true);
        OnClickEvent?.Invoke(this);
    }

    public void UnSelectMap()
    {
        seletedImage.gameObject.SetActive(false);
    }

    public void RemoveBlind()
    {
        blindImage.gameObject.SetActive(false);
    }

    public bool CheckBlind()
    {
        return (blindImage.gameObject.activeSelf);
    }
}
