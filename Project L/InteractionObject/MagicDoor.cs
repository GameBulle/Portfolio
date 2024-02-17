using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicDoor : MonoBehaviour
{
    //  0 : red, 1 : blue, 2 : green
    [SerializeField] GameObject[] circles;
    [SerializeField] GameObject portal;

    private void Awake()
    {
        portal.SetActive(false);
    }

    public void ReleaseBarrier(string barrierName)
    {
        switch(barrierName)
        {
            case "빨강 마법진":
                circles[0].SetActive(false);
                break;
            case "파랑 마법진":
                circles[1].SetActive(false);
                break;
            case "초록 마법진":
                circles[2].SetActive(false);
                break;
        }

        CheckBarrier();
    }

    void CheckBarrier()
    {
        bool isRelease = true;
        for(int i=0;i<circles.Length;i++)
        {
            if(circles[i].activeSelf == true)
            {
                isRelease = false;
                break;
            }
        }

        if (isRelease)
            portal.SetActive(true);
    }
}
