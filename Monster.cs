using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Monster : Character
{
    [SerializeField] MonsterData data;

    Transform target;
    Collider2D coll;

    // Mushroom
    float attck_timer;
    bool isAttack;
    bool aliveBoss;

    public MonsterData.MonsterID ID => data.Monster;
    public bool IsAttack { set { isAttack = value; } }

    protected override void Awake()
    {
        base.Awake();
        coll = GetComponent<Collider2D>();
    }

    private void Update()
    {
        hit_delay_timer += Time.deltaTime;
    }

    private void FixedUpdate()
    {
        if(!GameManager.Instance.GameOver)
        {
            switch (data.Monster)
            {
                case MonsterData.MonsterID.Flying_Eye:
                    Move();
                    if (Vector3.Distance(transform.position, GameManager.Instance.Player.transform.position) >= 30f)
                        gameObject.SetActive(false);
                    break;
                case MonsterData.MonsterID.Mushroom:
                    attck_timer += Time.fixedDeltaTime;
                    if (Vector3.Distance(transform.position, GameManager.Instance.Player.transform.position) <= 10f)
                    {
                        if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Hit"))
                            rigid.velocity = Vector2.zero;
                        if (attck_timer >= data.AttackSpeed)
                        {
                            attck_timer = 0;
                            AttackAnim();
                        }
                        else
                            anim.SetBool("Run", false);
                    }
                    else if (!isAttack)
                    {
                        dir = (target.transform.position - transform.position).normalized;
                        Move();
                        anim.SetBool("Run", true);
                    }
                    break;
                default:
                    dir = (target.transform.position - transform.position).normalized;
                    Move();
                    break;
            }
        }
    }

    private void LateUpdate()
    {
        if (data.Monster != MonsterData.MonsterID.Flying_Eye)
            spriteRenderer.flipX = target.position.x < transform.position.x;
    }

    protected override void Move()
    {
        if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Hit"))
            base.Move();
    }

    public void Initialize()
    {
        target = GameManager.Instance.Player.transform;
        switch (data.Monster)
        {
            case MonsterData.MonsterID.Flying_Eye:
                dir = (target.transform.position - transform.position).normalized;
                spriteRenderer.flipX = target.position.x < transform.position.x;
                break;
            default:
                attck_timer = 0f;
                isAttack = false;
                break;
        }
        hp = data.Health + Mathf.Floor(GameManager.Instance.GameTime) / 15f * 0.5f;
        damage = data.Damage;
        aliveBoss = GameManager.Instance.IsSpawnBoss;
        move_speed = data.Speed;
    }

    void AttackAnim()
    {
        isAttack = true;
        anim.SetTrigger("Attack");
    }

    public void Fire()
    {
        Vector3 dir = (GameManager.Instance.Player.transform.position - transform.position).normalized;
        Bullet bullet = GameManager.Instance.Pool.GetBullet(12);
        bullet.transform.position = transform.position;
        bullet.transform.rotation = Quaternion.FromToRotation(Vector3.up, dir);
        bullet.gameObject.SetActive(true);
        bullet.Initialize(data.Damage, 0, 0, 10f, dir);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bullet") || collision.CompareTag("Bullet_Melee"))
        {
            if (hit_delay_timer < 0.2f)
                return;

            Bullet hit = collision.GetComponent<Bullet>();
            hp -= hit.Damage;
            float knockback = hit.Knockback;
            hit_delay_timer = 0f;
            AudioManager.Instance.PlaySFX("Hit");

            if (hp > 0)
            {
                anim.SetTrigger("Hit");
                if (knockback != 0)
                {
                    StartCoroutine(Knockback(knockback));
                }

            }
            else
            {
                Die();
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Bullet") || collision.CompareTag("Bullet_Melee"))
        {
            if (hit_delay_timer < 0.2f)
                return;

            Bullet hit = collision.GetComponent<Bullet>();
            hp -= hit.Damage;
            float knockback = hit.Knockback;
            hit_delay_timer = 0f;
            AudioManager.Instance.PlaySFX("Hit");

            if (hp > 0)
            {
                anim.SetTrigger("Hit");
                if (knockback != 0)
                {
                    StartCoroutine(Knockback(knockback));
                }

            }
            else
            {
                Die();
            }
        }
    }

    public override void Die()
    {
        DropItem();
        gameObject.SetActive(false);
    }

    IEnumerator Knockback(float power)
    {
        yield return new WaitForFixedUpdate();
        Vector3 player_pos = GameManager.Instance.Player.transform.position;
        Vector3 dir = (transform.position - player_pos).normalized;
        rigid.AddForce(dir * power, ForceMode2D.Impulse);
    }

    void DropItem()
    {
        if (aliveBoss)
            return;
        Item exp = GameManager.Instance.Pool.GetItem((int)Item.ItemID.EXP);
        exp.transform.position = transform.position;
        exp.gameObject.SetActive(true);

        float random = Random.Range(0f, 100f);
        Item item = null;
        if (random > 90)
            item = GameManager.Instance.Pool.GetItem((int)Item.ItemID.Gold);
        else if (random > 85)
            item = GameManager.Instance.Pool.GetItem((int)Item.ItemID.HP);
        else if (random > 80)
            item = GameManager.Instance.Pool.GetItem((int)Item.ItemID.MP);
        else if (random > 78)
            item = GameManager.Instance.Pool.GetItem((int)Item.ItemID.Magent);
        else if (random > 77.5)
            item = GameManager.Instance.Pool.GetItem((int)Item.ItemID.Bomb);
        else
            return;

        item.transform.position = transform.position;
        item.gameObject.SetActive(true);
    }
}
