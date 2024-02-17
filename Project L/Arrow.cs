using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    Rigidbody rigid;
    float damage;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
    }

    public void SetPosition(Vector3 pos)
    {
        transform.position = pos;
    }

    public void Shot(Vector3 target, float damage, float speed)
    {
        this.damage = damage;

        transform.LookAt(target);

        gameObject.SetActive(true);
        rigid = GetComponent<Rigidbody>();
        rigid.velocity = Vector3.zero;
        rigid.AddForce(transform.forward * speed);
    }

    private void OnTriggerEnter(Collider collision)
    {
        switch(collision.tag)
        {
            case "Obstacle":
                gameObject.SetActive(false);
                break;
            case "Player":
                IFightable attackTarget = collision.GetComponentInParent<IFightable>();
                attackTarget.OnDamage(damage, transform.position);
                SoundMgr.Instance.PlaySFXAudio("±Ã¼ö Hit");
                gameObject.SetActive(false);
                break;
        }
    }
}
