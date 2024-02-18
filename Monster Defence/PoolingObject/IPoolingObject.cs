using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPoolingObject
{
    void SetPosition(Vector2 pos);

    void ReturnBack();
}
