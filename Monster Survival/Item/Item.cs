using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField] float alive_time;

    public enum ItemID { EXP, Gold, HP, MP, Magent, Bomb }

    float alive_timer;

    private void Update()
    {
        alive_timer += Time.deltaTime;
        if(alive_timer >= alive_time)
            gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Use();
            AudioManager.Instance.PlaySFX("GetItem");
            gameObject.SetActive(false);
        }
    }

    protected virtual void Use() { }

    private void OnEnable()
    {
        alive_timer = 0f;
    }
}
