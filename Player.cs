using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : Character
{
    float max_hp;
    float base_max_hp;
    float base_speed;
    bool isRun = false;
    Scanner scanner;
    bool useSkill;

    public Vector2 Direction => dir;
    public Scanner Scanner => scanner;

    public bool UseSkill
    {
        get { return useSkill; }
        set
        {
            useSkill = value;
            if(useSkill)
            {
                rigid.velocity = Vector2.zero;
                anim.SetTrigger("Skill");
            }    
        }
    }


    protected override void Awake()
    {
        base.Awake();
        scanner = GetComponent<Scanner>();
        base_speed = move_speed;
    }

    private void Update()
    {
        hit_delay_timer += Time.deltaTime;
    }

    private void FixedUpdate()
    {
        if (!GameManager.Instance.GameOver && !useSkill)
        {
            Move();
            anim.SetBool("Run", isRun);
        }
    }

    public void Initialize(CharacterData data)
    {
        gameObject.SetActive(true);
        anim.runtimeAnimatorController = data.Anim;
        move_speed = data.MoveSpeed * CharacterAbility.Samurai;
        base_speed = move_speed;
        max_hp = data.HP;
        base_max_hp = data.HP;
        hp = max_hp;
        useSkill = false;
        if (data.ID == CharacterData.CharacterType.Shaman)
            spriteRenderer.flipX = true;
    }

    void OnMove(InputValue value)
    {
        if (GameManager.Instance.GameOver)
            return;

        dir = value.Get<Vector2>();
        if (dir.x != 0)
            spriteRenderer.flipX = dir.x < 0;
        isRun = dir.x != 0 || dir.y != 0;
    }

    public void SpeedUp(float value) { move_speed = base_speed + base_speed * value; }

    public void HPRecovery(float value)
    {
        hp += value;
        hp = Mathf.Min(hp, max_hp);
        InterfaceManager.Instance.UpdateHP(hp, max_hp);
    }

    public void HPUp(float value) { max_hp = base_max_hp + base_max_hp * value; }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Monster Bullet") && !GameManager.Instance.GameOver)
        {
            if (hit_delay_timer < 0.2f)
                return;

            hp -= collision.GetComponent<Bullet>().Damage;
            hit_delay_timer = 0f;
            InterfaceManager.Instance.UpdateHP(hp, max_hp);
            AudioManager.Instance.PlaySFX("Hit");

            if (hp <= 0f)
                Die();
            else
                anim.SetTrigger("Hit");
        }
    }

    public override void Die()
    {
        anim.SetTrigger("Die");
        GameManager.Instance.GameDefeat();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.rigidbody.CompareTag("Monster") && !GameManager.Instance.GameOver)
        {
            if (hit_delay_timer < 0.2f)
                return;

            hp -= collision.rigidbody.GetComponent<Monster>().Damage;
            hit_delay_timer = 0f;
            InterfaceManager.Instance.UpdateHP(hp, max_hp);
            AudioManager.Instance.PlaySFX("Hit");

            if (hp <= 0f)
                Die();
            else
                anim.SetTrigger("Hit");
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.rigidbody.CompareTag("Monster") && !GameManager.Instance.GameOver)
        {
            if (hit_delay_timer < 0.2f)
                return;

            hp -= collision.rigidbody.GetComponent<Character>().Damage;
            hit_delay_timer = 0f;
            InterfaceManager.Instance.UpdateHP(hp, max_hp);
            AudioManager.Instance.PlaySFX("Hit");

            if (hp <= 0f)
                Die();
            else
                anim.SetTrigger("Hit");
        }
    }
}
