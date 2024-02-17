using System.Collections;
using System.Collections.Generic;
using System.Runtime.ExceptionServices;
using Unity.VisualScripting;
using UnityEngine;

public class InteractionListUI : MonoBehaviour
{
    [Header("Interaction Object")]
    [SerializeField] InteractionUI interactionUI;

    [Header("Margin")]
    [SerializeField] float heightMargin;
    [SerializeField] float widthMargin;

    [Header("UI Size")]
    [SerializeField] float height;

    [Header("View Port")]
    [SerializeField] GameObject viewPort;

    [Header("Content")]
    [SerializeField] GameObject content;

    [Header("Standard")]
    [SerializeField] GameObject standard;

    Dictionary<GameObject, InteractionUI> interactionDic = new();
    List<GameObject> interactionIndex = new();

    Player player;
    IInteractionable selectedInteraction;
    RectTransform viewPortRect;
    RectTransform standardRect;
    int index;
    bool isUp;

    public void Initialize(Player player)
    {
        this.player = player;
        viewPortRect = viewPort.GetComponent<RectTransform>();
        standardRect = standard.GetComponent<RectTransform>();
        index = 0;
        isUp = false;

        gameObject.SetActive(false);
    }

    public void AddInteraction(GameObject gameObject)
    {
        if(!interactionDic.ContainsKey(gameObject))
        {
            this.gameObject.SetActive(true);
            InterfaceMgr.Instance.InteractionUIActive();

            IInteractionable interactionable = gameObject.GetComponent<IInteractionable>();
            InteractionUI ui = Instantiate(interactionUI, content.transform);
            ui.Initialize(interactionable.InteractionName, interactionable.InteractionIcon);

            if (interactionDic.Count == 0)
                ui.SetPosition(new Vector3(0, -heightMargin));
            else
                ui.SetPosition(new Vector3(0, interactionDic[interactionIndex[interactionDic.Count - 1]].RectTr.anchoredPosition.y - height));

            interactionDic.Add(gameObject, ui);
            interactionIndex.Add(gameObject);

            ReSizeViewPort();
            ReLocationStandard();
            ReSizeContent();
            interactionDic[interactionIndex[index]].Selected();
        }
    }

    public void RemoveInteraction(GameObject gameObject)
    {
        if (interactionDic.ContainsKey(gameObject))
        {
            InterfaceMgr.Instance.InteractionUIExit();
            RectTransform deleteRect = interactionDic[gameObject].RectTr;
            int deleteIndex = interactionIndex.FindIndex(a => a == gameObject);

            if (deleteIndex != interactionDic.Count - 1)
            {
                interactionDic[interactionIndex[deleteIndex + 1]].SetPosition(new Vector3(0, deleteRect.anchoredPosition.y));
                for (int i = deleteIndex + 2; i < interactionDic.Count; i++)
                    interactionDic[interactionIndex[i]].SetPosition(new Vector3(0, interactionDic[interactionIndex[i - 1]].RectTr.anchoredPosition.y - height));
            }

            interactionDic[gameObject].DeleteUI();
            interactionDic.Remove(gameObject);
            interactionIndex.Remove(gameObject);

            if (index >= deleteIndex)
            {
                index--;
                if (index < 0)
                    index = 0;
            }

            if (interactionDic.Count == 0)
                this.gameObject.SetActive(false);
            else
            {
                ReSizeViewPort();
                ReLocationStandard();
                ReSizeContent();
                interactionDic[interactionIndex[index]].Selected();
            }
        }
    }

    void ReLocationStandard()
    {
        switch(index)
        {
            case 0:
                standardRect.anchoredPosition = new Vector2(0, -heightMargin);
                break;
            case 1:
                if(index == interactionDic.Count - 1)
                    standardRect.anchoredPosition = new Vector2(0, -(heightMargin + height));
                else
                    standardRect.anchoredPosition = new Vector2(0, -(heightMargin + height * 0.75f));
                break;
            default:
                if (index == interactionDic.Count - 1)
                    standardRect.anchoredPosition = new Vector2(0, -(heightMargin + height * 1.5f));
                else
                    standardRect.anchoredPosition = new Vector2(0, -(heightMargin + height));
                break;
        }
    }

    void ReSizeContent()
    {
        float moveY = standardRect.anchoredPosition.y - interactionDic[interactionIndex[index]].RectTr.anchoredPosition.y;
        for (int i = 0; i < interactionDic.Count; i++)
            interactionDic[interactionIndex[i]].SetPosition(new Vector3(0, interactionDic[interactionIndex[i]].RectTr.anchoredPosition.y + moveY));
    }

    void ReSizeViewPort()
    {
        switch (interactionDic.Count)
        {
            case 1:
            case 2:
                viewPortRect.sizeDelta = new Vector2(0, height * interactionDic.Count);
                break;
            case 3:
                viewPortRect.sizeDelta = new Vector2(0, height * 2.5f);
                break;
        }
    }

    void MoveContent()
    {
        float y = 0f;
        if (interactionDic.Count == 3)
            y = height * 0.25f;
        else if(interactionDic.Count >= 4)
        {
            if ((index == 1 && !isUp) || index == 0 || (index == interactionDic.Count - 2 && isUp) || index == interactionDic.Count - 1) 
                y = height * 0.25f;
            else
                y = height;
        }

        for (int i = 0; i < interactionDic.Count; i++)
            interactionDic[interactionIndex[i]].ReSetPosition(y, isUp);
    }

    public void WheelUp()
    {
        if (index - 1 >= 0)
        {
            isUp = true;
            interactionDic[interactionIndex[index]].NotSelected();
            index--;
            MoveContent();
            interactionDic[interactionIndex[index]].Selected();
        }
    }

    public void WheelDown()
    {
        if (index + 1 < interactionDic.Count)
        {
            isUp = false;
            interactionDic[interactionIndex[index]].NotSelected();
            index++;
            MoveContent();
            interactionDic[interactionIndex[index]].Selected();
        }
    }

    public void SelectInteraction()
    {
        if (interactionDic.Count <= 0)
            return;
        interactionDic[interactionIndex[index]].Opened();
        selectedInteraction = interactionIndex[index].GetComponent<IInteractionable>();
        selectedInteraction.Interaction(player);
    }

    public void InteractionContent()
    {
        selectedInteraction.InteractionGetItem(player);
    }
}
