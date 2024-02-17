using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadUI : MonoBehaviour
{
    [SerializeField] LoadSlot[] loadSlots;

    public void Initalize()
    {
        for (int i = 0; i < loadSlots.Length; i++)
            loadSlots[i].Initilize(i);
        gameObject.SetActive(false);
    }
}