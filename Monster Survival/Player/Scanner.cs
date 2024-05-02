using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scanner : MonoBehaviour
{
    [SerializeField] float scan_range;
    [SerializeField] LayerMask target_layer;
    [SerializeField] float get_range;
    [SerializeField] LayerMask item_layer;

    RaycastHit2D[] targets;
    RaycastHit2D[] items;
    float range;
    public Transform NearestTarget => GetNearestTarget();

    private void Awake()
    {
        range = get_range;
    }

    private void FixedUpdate()
    {
        targets = Physics2D.CircleCastAll(transform.position, scan_range, Vector2.zero, 0f, target_layer);
        GetExp();
    }

    void GetExp()
    {
        items = Physics2D.CircleCastAll(transform.position, range, Vector2.zero, 0f, item_layer);

        for (int i = 0; i < items.Length; i++)
        {
            EXP e = items[i].transform.GetComponent<EXP>();
            if (!e.IsMove)
                e.IsMove = true;
        }
    }

    Transform GetNearestTarget()
    {
        Transform target = null;
        float distance = 1000f;

        foreach (RaycastHit2D hit in targets)
        {
            Vector3 player_pos = transform.position;
            Vector3 target_pos = hit.transform.position;
            float cur_distance = Vector3.Distance(player_pos, target_pos);

            if (cur_distance < distance)
            {
                distance = cur_distance;
                target = hit.transform;
            }
        }

        return target;
    }

    public void ItemRangeUp(float value)
    {
        range = get_range + get_range * value;
    }

    public void GetMagnet()
    {
        RaycastHit2D[] exps = Physics2D.CircleCastAll(transform.position, 1000, Vector2.zero, 0f, item_layer);
        for (int i = 0; i < exps.Length; i++)
            exps[i].transform.GetComponent<EXP>().IsMove = true;
    }
}
