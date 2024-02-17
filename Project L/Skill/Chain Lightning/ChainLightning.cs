using Cinemachine.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChainLightning : MonoBehaviour
{
    Rigidbody rigid;
    float damage;
    float range;
    float speed;
    int chainCount;
    float chainDelay;
    Collider nextTarget;
    Dictionary<GameObject, bool> hitCheck;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        rigid.useGravity = false;
        hitCheck = new();
    }

    public void SetPosition(Vector3 pos)
    {
        transform.position = pos;
    }

    public void Shot(Vector3 dir, float damage, float speed, float range, int chainCount, float chainDelay)
    {
        this.damage = damage;
        this.range = range;
        this.speed = speed;
        this.chainCount = chainCount;
        this.chainDelay = chainDelay;

        gameObject.SetActive(true);
        rigid = GetComponent<Rigidbody>();
        rigid.velocity = Vector3.zero;
        rigid.AddForce(dir.normalized * this.speed);
    }

    private void OnTriggerEnter(Collider collision)
    {
        switch (collision.tag)
        {
            case "Obstacle":
                //gameObject.SetActive(false);
                break;
            case "Monster":
                nextTarget = collision;
                StartCoroutine(chainAttack());
                break;
        }
    }

    IEnumerator chainAttack()
    {
        bool isAllHit;
        float minDistance;
        float targetDistance;
        Collider target = null;

        while (chainCount != 0)
        {
            Collider[] targets = Physics.OverlapSphere(nextTarget.transform.position, range, 1 << 9);
            minDistance = float.MaxValue;
            isAllHit = true;

            if (null != targets && 0 < targets.Length)
            {
                for (int i = 0; i < targets.Length; i++)
                {
                    if (!hitCheck.ContainsKey(targets[i].gameObject))
                    {
                        isAllHit = false;
                        targetDistance = Vector3.SqrMagnitude(targets[i].transform.position - nextTarget.transform.position);
                        if (minDistance > targetDistance)
                        {
                            minDistance = targetDistance;
                            target = targets[i];
                        }
                    }
                }
                nextTarget = target;

                if (isAllHit)
                {
                    gameObject.SetActive(false);
                    yield return null;
                }
                    

                hitCheck.Add(nextTarget.gameObject, true);
                chainCount--;
            }
            else
            {
                gameObject.SetActive(false);
                yield return null;
            }

            SoundMgr.Instance.PlaySFXAudio("ChainLightning");
            EffectMgr.Instance.PlayParticleSystem("ChainLightningHit", new Vector3(nextTarget.transform.position.x, this.transform.position.y, nextTarget.transform.position.z));
            IFightable attackTarget = nextTarget.GetComponentInParent<IFightable>();
            attackTarget.OnDamage(damage);

            yield return new WaitForSeconds(chainDelay);
        }
        gameObject.SetActive(false);
        yield return null;
    }
}
