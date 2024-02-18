using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour, IMovement
{
    protected Vector2 dir = Vector2.zero;
    protected float speed = 0.0f;
    protected float attackTimer = 0.0f;
    public static readonly float updateTime = 0.04f;
    protected Rigidbody2D rigid2d;

    public Vector2 velocity { get; set; }

    public abstract void Initialize();

    public virtual void Move()
    {
        velocity = speed * dir * Time.deltaTime;
    }

    public virtual void Stop()
    {
        velocity = new Vector2(0, 0);
        Debug.Log("Stop!");
    }

    public bool AttackDurationCheck()
    {
        if (0.0f <= attackTimer)
        {
            attackTimer -= updateTime;
            return true;
        }
        return false;
    }

    protected void ErrorMessage(string msg)
    {
        Debug.LogError(msg);
        gameObject.SetActive(false);
    }
}
