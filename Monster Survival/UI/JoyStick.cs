using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JoyStick : MonoBehaviour
{
    [SerializeField] Image controller;

    RectTransform rect;
    RectTransform controller_rect;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
        controller_rect = controller.GetComponent<RectTransform>();
    }

    public void Hide()
    {
        //rect.localScale = Vector3.zero;
    }

    public void Show()
    {
        //controller_rect.anchoredPosition = Vector3.zero;
        //rect.localScale = Vector3.one;
    }
}
