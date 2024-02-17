using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MoveableUI : MonoBehaviour, IPointerDownHandler, IDragHandler
{
    [Header("MoveTarget UI")]
    [SerializeField] Transform moveTarget;

    Vector2 beginPoint;
    Vector2 movePoint;

    private void Awake()
    {
        if (moveTarget == null)
            moveTarget = GetComponentInParent<Transform>();
    }

    void IPointerDownHandler.OnPointerDown(UnityEngine.EventSystems.PointerEventData eventData)
    {
        beginPoint = moveTarget.position;
        movePoint = eventData.position;
    }

    void IDragHandler.OnDrag(UnityEngine.EventSystems.PointerEventData eventData)
    {
        moveTarget.position = beginPoint + (eventData.position - movePoint);
    }
}
