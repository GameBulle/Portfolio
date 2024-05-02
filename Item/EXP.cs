using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EXP : Item
{
    bool isMove;
    public bool IsMove { get { return isMove; } set { isMove = value; } }

    private void FixedUpdate()
    {
        if(isMove)
            transform.position = Vector3.Lerp(transform.position, GameManager.Instance.Player.transform.position, 8f * Time.deltaTime);
    }

    private void OnEnable()
    {
        isMove = false;
    }

    private void OnDisable()
    {
        isMove = false;
    }

    protected override void Use()
    {
        GameManager.Instance.GetExp();
    }
}
