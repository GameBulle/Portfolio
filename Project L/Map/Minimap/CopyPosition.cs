using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CopyPosition : MonoBehaviour
{
    [SerializeField] bool x, y, z;
    Transform target;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        if (!target)
            return;

        transform.position = new Vector3(
            (x ? target.position.x : transform.position.x),
            (y ? target.position.y : transform.position.y),
            (z ? target.position.z : transform.position.z));
    }

    public void Initialize(Transform target)
    {
        this.target = target;
        if (target != null)
        {
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, 360f + target.eulerAngles.y, transform.eulerAngles.z);
        }   
    }
}
