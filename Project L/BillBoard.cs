using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillBoard : MonoBehaviour
{
    private void FixedUpdate()
    {
        transform.LookAt(
            transform.position + GameMgr.Instance.mainCam.transform.rotation * Vector3.forward,
            GameMgr.Instance.mainCam.transform.rotation * Vector3.up);
    }
}
