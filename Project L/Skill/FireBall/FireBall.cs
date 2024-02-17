using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : MonoBehaviour
{
    Rigidbody rigid;
    float damage;
    float range;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        rigid.useGravity = false;
    }

    public void SetPosition(Vector3 pos)
    {
        transform.position = pos;
    }

    public void Shot(Vector3 dir, float damage, float speed, float range)
    {
        this.damage = damage;
        this.range = range;

        gameObject.SetActive(true);
        rigid = GetComponent<Rigidbody>();
        rigid.velocity = Vector3.zero;
        rigid.AddForce(dir.normalized * speed);
    }

    private void OnTriggerEnter(Collider collision)
    {
        switch (collision.tag)
        {
            case "Obstacle":
                gameObject.SetActive(false);
                break;
            case "Monster":
                EffectMgr.Instance.PlayParticleSystem("FireBallHit", this.transform.position);
                Collider[] targets = Physics.OverlapSphere(collision.transform.position, range, 1<<9);

                for (int i = 0; i < targets.Length; i++)
                {
                    IFightable attackTarget = targets[i].GetComponentInParent<IFightable>();
                    attackTarget.OnDamage(damage);
                }
                SoundMgr.Instance.PlaySFXAudio("FireBall");
                gameObject.SetActive(false);
                break;
        }
    }
}
