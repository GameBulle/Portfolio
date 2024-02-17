using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class Character : MonoBehaviour
{
    protected float accel = 0f;
    protected Vector3 velocity;
    protected Vector3 dir;
    protected Rigidbody rigid;
    public static readonly float updateTime = 0.04f;

    public virtual void Initialize() { }

    protected virtual void Awake()
    {
        rigid = GetComponent<Rigidbody>();
    }

    public virtual void Stop()
    {
        dir = Vector3.zero;
        velocity = Vector3.zero;
        if(rigid == null)
            rigid = GetComponent<Rigidbody>();
        rigid.velocity = velocity;
    }

    public virtual void Move() { }

    protected void ErrorMessage(string msg)
    {
        Debug.LogError(msg);
        gameObject.SetActive(false);
    }
}
