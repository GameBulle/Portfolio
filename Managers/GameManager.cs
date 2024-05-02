using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    //  ΩÃ±€≈Ê ∆–≈œ  //
    static GameManager instance;
    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<GameManager>();
                if (!instance)
                    instance = new GameObject("GameManager").AddComponent<GameManager>();
            }
            return instance;
        }
    }
    /////////////////////

    [SerializeField] Player player;
    [SerializeField] PoolManager pool;
    [SerializeField] WeaponManager weapon;
    [SerializeField] Bullet cleaner;
    [SerializeField] CharacterManager characterMgr;
    [SerializeField] AchievementManager achieve;
    [SerializeField] DataManager data;
    [SerializeField] LoadingUI loading;

    CharacterData.CharacterType player_id;
    Skill player_skill;

    int level = 1;
    int FPS;

    float max_mp;
    float cur_mp;
    float mp_plus;
    float mp_minus;
    bool use_skill;

    int gold = 0;
    int gold_in_game = 0;
    int gold_plus;

    float exp_next = 4f;
    float exp_sum = 0f;
    float exp_plus = 1f;

    float game_time;

    bool game_over;
    bool isSpawnBoss;

    public Player Player => player;
    public PoolManager Pool => pool;
    public WeaponManager Weapon => weapon;
    public CharacterData.CharacterType Character => player_id;
    public bool IsMaxMp => cur_mp >= max_mp;
    public int Gold { get { return gold; } set { gold = value; } }
    public int GoldUp { set { gold_plus += value; } }
    public int GoldInGame => gold_in_game;
    public float EXPUp { set { exp_plus = exp_plus + exp_plus * value; } }
    public bool GameOver { get { return game_over; } set { game_over = value; } }
    public bool IsSpawnBoss { get { return isSpawnBoss; } set { isSpawnBoss = value; } }
    public float CurrMP
    {
        get { return cur_mp; }
        set
        {
            cur_mp += value;
            cur_mp = Mathf.Min(cur_mp, max_mp);
        }
    }

    public float MP_Plus { set { mp_plus = mp_plus + mp_plus * value; } }
    public float GameTime => game_time;

    private void Awake()
    {
        if (this != Instance)
            Destroy(gameObject);
        cleaner.Initialize(100000, -10, 0, 0, Vector3.zero);
        OffCleaner();
        Application.targetFrameRate = 60;
        game_over = true;
        game_time = 0;
        loading.gameObject.SetActive(false);
        Load();
    }

    private void Update()
    {
        if (!game_over)
        {
            if(!isSpawnBoss)
                game_time += Time.deltaTime;
            if (!use_skill)
            {
                cur_mp += mp_plus * Time.deltaTime;
                cur_mp = Mathf.Min(cur_mp, max_mp);
            }
            else
            {
                cur_mp -= mp_minus * Time.deltaTime;
                if (cur_mp <= 0)
                {
                    player_skill.StopSkill();
                    use_skill = false;
                }  
            }
        }
    }

    void Initialize()
    {
        achieve.Initialize();
        characterMgr.Initialize();
        gold = 1000;
    }

    public void SetGame(CharacterData playerData)
    {
        game_time = 0f;
        use_skill = false;
        game_over = false;
        isSpawnBoss = false;
        player_id = playerData.ID;
        max_mp = 20f;
        cur_mp = max_mp;
        mp_plus = playerData.MPPlus;
        mp_minus = playerData.SkillUseMp;
        gold_plus = 1;
        player.Initialize(playerData);
        InterfaceManager.Instance.InitializeHUD(exp_next, playerData.HP, max_mp);
        SetPlayerStartWeapon(playerData.StartWeapon);
        SetPlayerSkill(playerData);
        AudioManager.Instance.PlayBackground(playerData.Clip);
    }

    void SetPlayerSkill(CharacterData data)
    {
        GameObject skill = new GameObject();
        switch (data.ID)
        {
            case CharacterData.CharacterType.Warrior:
                skill.transform.parent = player.transform;
                player_skill = skill.AddComponent<Warrior_Skill>();
                player_skill.Initialize(data.SkillDamage);
                break;
            case CharacterData.CharacterType.Wizard:
                skill.transform.parent = player.transform;
                player_skill = skill.AddComponent<Wizard_Skill>();
                player_skill.Initialize(data.SkillDamage);
                break;
            case CharacterData.CharacterType.Samurai:
                skill.transform.parent = player.transform;
                player_skill = skill.AddComponent<Samurai_Skill>();
                player_skill.Initialize(data.SkillDamage);
                break;
            case CharacterData.CharacterType.Shaman:
                skill.transform.parent = player.transform;
                player_skill = skill.AddComponent<Shaman_Skill>();
                player_skill.Initialize(data.SkillDamage);
                break;
        }
    }

    public void GameDefeat()
    {
        gold += gold_in_game;
        AudioManager.Instance.StopBackground();
        StartCoroutine(GameDefeatRoutine());
    }

    IEnumerator GameDefeatRoutine()
    {
        game_over = true;
        yield return new WaitForSeconds(1.0f);
        GameStop();
        SaveGameData();
        InterfaceManager.Instance.GameDefeat();
        AudioManager.Instance.PlaySFX("Defeat");
    }

    public void GameVictory()
    {
        gold += gold_in_game;
        StartCoroutine(GameVictoryRoutine());
    }

    IEnumerator GameVictoryRoutine()
    {
        game_over = true;
        OnCleaner();
        yield return new WaitForSeconds(1.0f);
        GameStop();
        SaveGameData();
        InterfaceManager.Instance.GameVictory();
        AudioManager.Instance.PlaySFX("Victory");
    }

    public void UseSkill()
    {
        if (cur_mp < max_mp && !game_over)
            return;
        use_skill = true;
        player_skill.UseSkill();
    }

    public void GetExp()
    {
        exp_sum += exp_plus;
        if (exp_sum >= exp_next)
            LevelUp();
        InterfaceManager.Instance.UpdateEXP(exp_sum, exp_next);
    }

    public void GetGold()
    {
        gold_in_game += gold_plus;
        InterfaceManager.Instance.UpdateGold(gold_in_game);
    }

    void LevelUp()
    {
        exp_sum -= exp_next;
        exp_next = exp_next * 1.3f;
        level++;
        InterfaceManager.Instance.UpdateLevel(level);
        weapon.GetRandomSkill();
        AudioManager.Instance.PlaySFX("LevelUp");
        AudioManager.Instance.BGMEffect(true);
        GameStop();
    }

    public void SelectSkill(int id, bool isNew = false)
    {
        weapon.SelectSkill(id, isNew);
        GameContinue();
    }

    public void SaveGameData()
    {
        data.SaveGameData(characterMgr.GetAllData(), achieve.GetCurrAchieve(), gold);
    }

    public void SaveOptionData()
    {
        data.SaveOptionData(AudioManager.Instance.MasterVolume, AudioManager.Instance.BackgroundVolume, AudioManager.Instance.SFXVolume, FPS);
    }

    public void Load()
    {
        GameData game_data = data.LoadGameData();
        if (game_data == null)
            Initialize();
        else
        {
            gold = game_data.gold;
            characterMgr.LoadData(game_data.character_name, game_data.character_level);
            achieve.LoadData(game_data.achieve_name, game_data.achieve_value);
        }

        OptionData option_data = data.LoadOptionData();
        if(option_data == null)
        {
            option_data = new();
            option_data.masterVolume = 0.8f;
            option_data.backgroundVolume = 0.8f;
            option_data.SFXVolume = 0.8f;
            option_data.FPS = 60;
        }
        InterfaceManager.Instance.InitializeOption(option_data);
        AudioManager.Instance.Initialize(option_data.masterVolume, option_data.backgroundVolume, option_data.SFXVolume);
    }

    public void GameExit()
    {
        SaveGameData();
        Application.Quit();
    }

    public void GameReStart() 
    {
        GameContinue();
        ShowLoadingUI(); 
    }

    public void SetFPS(int FPS) 
    { 
        this.FPS = FPS;
        Application.targetFrameRate = this.FPS; 
    }

    public void GameStop() { Time.timeScale = 0; }
    public void GameContinue() { Time.timeScale = 1; }
    void SetPlayerStartWeapon(SkillData.SkillID id) { weapon.SetPlayerStartWeapon(id); }
    public void HPRecovery(float value) { player.HPRecovery(value); }
    public void HPUp(float value) { player.HPUp(value); }
    public void OnCleaner() { cleaner.transform.localScale = Vector3.one; }
    public void OffCleaner() { cleaner.transform.localScale = Vector3.zero; }
    public void ReDraw() { weapon.GetRandomSkill(); }
    public string GetWeaponName(SkillData.SkillID id) { return weapon.GetWeaponName(id); }
    public int GetCharacterSize() { return characterMgr.Size; }
    public CharacterData GetCharacterData(int index) { return characterMgr.GetCharacterData(index); }
    public void AddAchieveObserver(IAchieveObserver o) { achieve.AddObserver(o); }
    public void ShowLoadingUI(CharacterData data = null) { loading.ShowLoadingUI(data); }
}
