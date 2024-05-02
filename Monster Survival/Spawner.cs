using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [Header("## Normal Spawn Data")]
    [SerializeField] float base_spawn_time;
    [SerializeField] float time_per_spwan;

    [Header("## Wave Spawn Data")]
    [SerializeField] float wave_spawn_time;
    [SerializeField] int wave_monster_count;
    [SerializeField] int plus_wave_count;

    [Header("## Circle Spawn Data")]
    [SerializeField] float circle_spawn_time;
    [SerializeField] int circle_monster_count;
    [SerializeField] int plus_circle_count;

    [SerializeField] Boss[] bosses;

    float range_timer;
    float normal_timer;
    float wave_timer;
    float circle_timer;
    float normal_spawn_time;
    float range_spawn_time;

    private void Awake()
    {
        range_spawn_time = base_spawn_time * 2.5f;
        normal_spawn_time = base_spawn_time;
        for (int i = 0; i < bosses.Length; i++)
            bosses[i].IsSpawn = false;
    }

    void Update()
    {
        if(!GameManager.Instance.GameOver)
        {
            normal_timer += Time.deltaTime;
            wave_timer += Time.deltaTime;
            circle_timer += Time.deltaTime;
            range_timer += Time.deltaTime;

            if (normal_timer >= normal_spawn_time)
                SpawnNormalMonster();

            if (range_timer >= range_spawn_time)
                SpawnRangeMonster();

            if (circle_timer >= circle_spawn_time && GameManager.Instance.GameTime >= 60)
                SpawnCircleMonster();

            if (wave_timer >= wave_spawn_time)
                SpawnWaveMonster();

            SpawnBoss();
        }
    }

    private void FixedUpdate()
    {
        normal_spawn_time = base_spawn_time - Mathf.Floor(GameManager.Instance.GameTime) / time_per_spwan * 0.01f;
        normal_spawn_time = Mathf.Max(normal_spawn_time, 0.05f);
        range_spawn_time = normal_spawn_time * 2.5f;
    }

    void SpawnRangeMonster()
    {
        if (GameManager.Instance.GameTime < 120f)
            return;
        GameManager.Instance.OffCleaner();
        Monster monster = null;
        monster = GameManager.Instance.Pool.GetMonster((int)MonsterData.MonsterID.Mushroom);
        Vector3 random_pos = GetRandomPos();
        monster.transform.position = GameManager.Instance.Player.transform.position + random_pos;
        monster.gameObject.SetActive(true);
        monster.Initialize();
        range_timer = 0f;
    }

    void SpawnNormalMonster()
    {
        GameManager.Instance.OffCleaner();

        int count;
        if (GameManager.Instance.GameTime < 90)
            count = 1;
        else
            count = 2;

        for(int i=0;i<count;i++)
        {
            Monster monster = null;
            monster = GameManager.Instance.Pool.GetMonster((int)MonsterData.MonsterID.Goblin + i);
            Vector3 random_pos = GetRandomPos();
            monster.transform.position = GameManager.Instance.Player.transform.position + random_pos;
            monster.gameObject.SetActive(true);
            monster.Initialize();
        }
        normal_timer = 0f;
    }

    void SpawnCircleMonster()
    {
        if (GameManager.Instance.GameTime < 60f)
            return;
        GameManager.Instance.OffCleaner();
        for (int i = 0; i < circle_monster_count; i++)
        {
            Vector3 random_dir = GetRandomPos().normalized;
            Monster monster = GameManager.Instance.Pool.GetMonster((int)MonsterData.MonsterID.Skeleton);
            monster.transform.position = GameManager.Instance.Player.transform.position + random_dir * 40f;
            monster.gameObject.SetActive(true);
            monster.Initialize();
        }
        circle_monster_count += plus_circle_count;
        circle_timer = 0f;
    }

    void SpawnWaveMonster()
    {
        if (GameManager.Instance.GameTime < 30f)
            return;
        GameManager.Instance.OffCleaner();
        Vector3 player_pos = GameManager.Instance.Player.transform.position;
        Vector3 random_pos = GetRandomPos();
        for (int i = 0; i < wave_monster_count; i++)
        {
            Monster monster = GameManager.Instance.Pool.GetMonster((int)MonsterData.MonsterID.Flying_Eye);
            monster.transform.position = new Vector3(random_pos.x + player_pos.x + Random.Range(-0.5f, 0.5f), random_pos.y + player_pos.y + Random.Range(-0.5f, 0.5f), 0);
            monster.gameObject.SetActive(true);
            monster.Initialize();
        }
        wave_monster_count += plus_wave_count;
        wave_timer = 0f;
    }

    void SpawnBoss()
    {
        for (int i = 0; i < bosses.Length; i++)
        {
            if (bosses[i].CheckSpawn())
            {
                bosses[i].IsSpawn = true;
                Boss boss = Instantiate(bosses[i]);
                boss.Initialize();
                GameManager.Instance.IsSpawnBoss = true;
                GameManager.Instance.OffCleaner();
            }
        }
    }

    Vector3 GetRandomPos()
    {
        Vector3 pos = Vector3.zero;
        int sign = Random.Range(0, 2) == 0 ? -1 : 1;

        switch (Random.Range(0, 2))
        {
            case 0: // ÁÂ¿ì
                pos = new Vector3(sign * Random.Range(22f, 25f), Random.Range(-15f, 15f), 0);
                break;
            case 1: // À§¾Æ·¡
                pos = new Vector3(Random.Range(-22f, 22f), sign * Random.Range(15f, 18f), 0);
                break;
        }
        return pos;
    }
}
