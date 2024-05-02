using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : Character
{
    [SerializeField] BossData data;

    float timer;
    protected Collider2D coll;
    protected LineRenderer lineRenderer;
    protected bool isDie = false;
    protected bool isAttack = false;
    protected Coroutine coroutine;
    protected Vector3 attack_dir;
    protected bool isSpawn = false;

    public bool IsSpawn { get { return isSpawn; } set { isSpawn = value; } }

    public bool IsAttack { set { isAttack = value; } }

    protected override void Awake()
    {
        base.Awake();
        coll = GetComponent<Collider2D>();
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.sortingOrder = 5;
        lineRenderer.startColor = Color.red;
        lineRenderer.endColor = Color.red;
    }

    private void Update()
    {
        if (!isDie)
        {
            hit_delay_timer += Time.deltaTime;
            if (!isAttack)
                timer += Time.deltaTime;

            if (timer >= data.AttackTimer)
                Attack();
        }
    }

    void FixedUpdate()
    {
        if(!isDie)
        {
            if (!isAttack)
            {
                dir = (GameManager.Instance.Player.transform.position - transform.position).normalized;
                Move();
                if (GameManager.Instance.Player.transform.position.x < transform.position.x)
                    transform.localScale = new Vector3(-1, 1, 1);
                else
                    transform.localScale = new Vector3(1, 1, 1);
            }
        }
    }

    void Attack()
    {
        isAttack = true;
        timer = 0f;
        int random = Random.Range(0, 2);
        switch (random)
        {
            case 0:
                coroutine = StartCoroutine(OnAttack1());
                break;
            case 1:
                coroutine = StartCoroutine(OnAttack2());
                break;
        }
    }

    public void Initialize()
    {
        transform.position = GameManager.Instance.Player.transform.position + Vector3.up * 20f;
        gameObject.SetActive(true);
        coll.enabled = true;
        hp = data.HP;
        isDie = false;
        isAttack = false;
        timer = 0f;
        hit_delay_timer = 0f;
        coroutine = null;
        damage = data.Damage;
        move_speed = data.Speed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bullet") || collision.CompareTag("Bullet_Melee"))
        {
            if (hit_delay_timer < 0.2f)
                return;
            hit_delay_timer = 0f;
            hp -= collision.GetComponent<Bullet>().Damage;
            AudioManager.Instance.PlaySFX("Hit");
            if (hp <= 0)
                Die();
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Bullet") || collision.CompareTag("Bullet_Melee"))
        {
            if (hit_delay_timer < 0.2f)
                return;
            hit_delay_timer = 0f;
            hp -= collision.GetComponent<Bullet>().Damage;
            AudioManager.Instance.PlaySFX("Hit");
            if (hp <= 0)
                Die();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        rigid.velocity = Vector2.zero;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        rigid.velocity = Vector2.zero;
    }

    protected override void Move()
    {
        if(!isDie)
            base.Move();
    }

    protected virtual IEnumerator OnAttack1() { return null; }
    protected virtual void Attack1Ready() { }
    protected virtual void Attack1() { }
    protected virtual IEnumerator OnAttack2() { return null; }
    protected virtual void Attack2Ready() { }
    protected virtual void Attack2() { }
    public virtual bool CheckSpawn() { return false; }
}
