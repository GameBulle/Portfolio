using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class InteractionUI : MonoBehaviour, IPoolingObject
{
    [Header("Interaction Name")]
    [SerializeField] TextMeshProUGUI interactionName;

    [Header("Interaction Icon")]
    [SerializeField] Image interactionIcon;

    [Header("Selected Image")]
    [SerializeField] Image selectedImage;

    public RectTransform RectTr { get; private set; }

    public void Initialize(string interactionName, Sprite interactionIcon)
    {
        RectTr = GetComponent<RectTransform>();

        this.interactionName.text = interactionName;
        this.interactionIcon.sprite = interactionIcon;
        selectedImage.gameObject.SetActive(false);
    }

    public void SetPosition(Vector3 pos)
    {
        RectTr.anchoredPosition = new Vector2(0, pos.y);
    }

    public void SetAngle(Vector3 angle)
    {

    }

    public void ReSetPosition(float y, bool up)
    {
        if (up)
            y = -y;
        RectTr.anchoredPosition = new Vector2(0, RectTr.anchoredPosition.y + y);
    }

    public void Selected()
    {
        selectedImage.gameObject.SetActive(true);
    }

    public void NotSelected()
    {
        selectedImage.gameObject.SetActive(false);
    }

    public void Opened()
    {
        interactionName.color = new Color32(85, 85, 85, 255);
    }

    public void DeleteUI()
    {
        Destroy(gameObject);
    }
}
