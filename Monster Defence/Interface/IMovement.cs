using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMovement
{
    public Vector2 velocity { get; set; }
    public void Move();
    public void Stop();
}