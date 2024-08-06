using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Auth;
using Firebase;
using Firebase.Database;
using System;
using System.IO;
using Google.MiniJSON;
using UnityEngine.InputSystem;
using Unity.VisualScripting;

public class FirebaseManager : MonoBehaviour
{
    static FirebaseManager instance;
    public static FirebaseManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<FirebaseManager>();
                if (!instance)
                    instance = new GameObject("FirebaseManager").AddComponent<FirebaseManager>();
            }
            return instance;
        }
    }

    [SerializeField] GameObject loginFail;

    DatabaseReference reference;
    DataSnapshot snapshot;
    DependencyStatus dependencyStatus;
    FirebaseAuth auth;  // 로그인, 회원가입 등에 사용
    FirebaseUser user;  // 인증이 완료된 유어 정보

    public DataSnapshot SnapShot => snapshot;

    private void Awake()
    {
        if (Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
        InitializeFirebase();
    }

    public void InitializeFirebase()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            dependencyStatus = task.Result;
            if (dependencyStatus == DependencyStatus.Available)
            {
                auth = FirebaseAuth.DefaultInstance;
                reference = FirebaseDatabase.DefaultInstance.RootReference;
                Debug.Log("Firebase 초기화 완료");
            }
        });
    }


    void OnChangeState(object sender, EventArgs e)
    {
        if (auth.CurrentUser != user)
        {
            bool signed = (auth.CurrentUser != user && auth.CurrentUser != null);
            if (!signed && user != null)
            {
                Debug.Log("로그아웃");
            }
        }
    }

    public void Create(string email, string password, string nickname)
    {
        auth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWith(task =>
        {
            if (task.IsCanceled)
            {
                Debug.LogError("회원가입 취소");
                return;
            }

            if (task.IsFaulted)
            {
                Debug.LogError("회원가입 실패");
                return;
            }

            user = task.Result.User;
            SaveUserInfo(email, password, user.UserId, nickname);
            InitUser();
            Debug.Log("회원가입 완료");
        });
    }

    void InitUser()
    {
        SaveOptionData();
        SaveGameData();
        Debug.Log("유저 초기화 완료");
    }

    void SaveUserInfo(string email, string password, string user_id, string nickname)
    {
        UserInfoData user = new UserInfoData();
        user.email = email;
        user.password = password;
        user.nickname = nickname;

        string json = JsonUtility.ToJson(user);
        reference.Child("Users").Child(user_id).Child("Info").SetRawJsonValueAsync(json);
    }

    public void SaveOptionData(float masterVolume = 0.8f, float backgrouindVolume = 0.8f, float SFXVolume = 0.8f, int FPS = 60)
    {
        OptionData optionData = new();
        optionData.masterVolume = masterVolume;
        optionData.backgroundVolume = backgrouindVolume;
        optionData.SFXVolume = SFXVolume;
        optionData.FPS = FPS;

        string json = JsonUtility.ToJson(optionData);
        reference.Child("Users").Child(user.UserId).Child("Option").SetRawJsonValueAsync(json);
    }

    void SaveGameData()
    {
        GameData saveData = new();
        saveData.gold = 500;
        
        for(int i=0;i<=(int)CharacterData.CharacterType.Shaman;i++)
        {
            saveData.character_id.Add(i);
            saveData.character_level.Add(0);
            saveData.character_level.Add(0);
            saveData.character_level.Add(0);
            saveData.character_level.Add(0);
        }

        AchievementManager.Unlock[] unlocks = (AchievementManager.Unlock[])Enum.GetValues(typeof(AchievementManager.Unlock));
        saveData.achieve_name.Add("Init");
        saveData.achieve_value.Add(1);
        for (int i=0;i<unlocks.Length;i++)
        {
            saveData.achieve_name.Add(unlocks[i].ToString());
            saveData.achieve_value.Add(0);
        }

        string json = JsonUtility.ToJson(saveData, true);
        reference.Child("Users").Child(user.UserId).Child("Game").SetRawJsonValueAsync(json);
    }

    public void SaveGameData(CharacterData[] charcater_data, Dictionary<string, int> achieve_data, int gold)
    {
        GameData save_data = new();
        save_data.gold = gold;
        for (int i = 0; i < charcater_data.Length; i++)
        {
            save_data.character_id.Add(i);
            save_data.character_level.Add(charcater_data[i].Upgrade.GetMoveSpeedLevel());
            save_data.character_level.Add(charcater_data[i].Upgrade.GetHPLevel());
            save_data.character_level.Add(charcater_data[i].Upgrade.GetMPPlusLevel());
            save_data.character_level.Add(charcater_data[i].Upgrade.GetSkillDamageLevel());
        }

        foreach (KeyValuePair<string, int> achieve in achieve_data)
        {
            save_data.achieve_name.Add(achieve.Key);
            save_data.achieve_value.Add(achieve.Value);
        }

        string json = JsonUtility.ToJson(save_data, true);
        reference.Child("Users").Child(user.UserId).Child("Game").SetRawJsonValueAsync(json);
    }

    public async void LoadData()
    {
        await reference.Child("Users").Child(user.UserId).Child("Game").GetValueAsync().ContinueWith(task =>
        {
            if (task.IsFaulted)
                Debug.Log("데이터 베이스 읽기 실패");
            else if (task.IsCompleted)
            {
                snapshot = task.Result;
                Debug.Log( "Game 읽기 완료");

                var data = snapshot.Value as Dictionary<string, object>;
                var achieveName = (List<object>)data["achieve_name"];
                var achieveValue = (List<object>)data["achieve_value"];
                var characterID = (List<object>)data["character_id"];
                var characterValue = (List<object>)data["character_level"];
                var goldD = snapshot.Value as Dictionary<string, object>;

                GameManager.Instance.Gold = Convert.ToInt32(goldD["gold"]);
                GameManager.Instance.CharacterMgr.LoadData(characterID, characterValue);
                GameManager.Instance.Achieve.LoadData(achieveName, achieveValue);

                Debug.Log("Game 불러오기 성공");
            }
        });

        OptionData optionData = new();
        await reference.Child("Users").Child(user.UserId).Child("Option").GetValueAsync().ContinueWith(task =>
        {
            if (task.IsFaulted)
                Debug.Log("데이터 베이스 읽기 실패");
            else if (task.IsCompleted)
            {
                snapshot = task.Result;
                Debug.Log("Option 읽기 완료");

                var option = snapshot.Value as Dictionary<string, object>;
                optionData.FPS = Convert.ToInt32(option["FPS"]);
                optionData.masterVolume = Convert.ToSingle(option["masterVolume"]);
                optionData.backgroundVolume = Convert.ToSingle(option["backgroundVolume"]);
                optionData.SFXVolume = Convert.ToSingle(option["SFXVolume"]);

                Debug.Log("Option 불러오기 성공");
            }
        });

        string nickname = "";
        await reference.Child("Users").Child(user.UserId).Child("Info").GetValueAsync().ContinueWith(task =>
        {
            if (task.IsFaulted)
                Debug.Log("데이터 베이스 읽기 실패");
            else if (task.IsCompleted)
            {
                snapshot = task.Result;
                Debug.Log("Info 읽기 완료");

                var info = snapshot.Value as Dictionary<string, object>;
                nickname = info["nickname"].ToString();
                Debug.Log("Info 불러오기 성공");
            }
        });

        InterfaceManager.Instance.LoadData(optionData, nickname);
        AudioManager.Instance.Initialize(optionData.masterVolume, optionData.backgroundVolume, optionData.SFXVolume);
    }

    public async void Login(string email, string password)
    {
        bool login = false;
        await auth.SignInWithEmailAndPasswordAsync(email, password).ContinueWith(task =>
        {
            if (task.IsCanceled)
            {
                Debug.Log("로그인 취소");
                return;
            }

            if (task.IsFaulted)
            {
                Debug.Log("로그인 실패");
                return;
            }

            AuthResult result = task.Result;
            user = result.User;
            login = true;
            Debug.Log("로그인 완료");

        });

        if(!login)
            loginFail.SetActive(true);
        else if (auth.CurrentUser == user)
            LoadSceneManager.Instance.LoadScene("Main");
    }

    public void LogOut()
    {
        auth.SignOut();
        Debug.Log("로그아웃");
        LoadSceneManager.Instance.LoadScene("Login");
    }

    public void QuitApp() { Application.Quit(); }

    [System.Serializable]
    public class UserInfoData
    {
        public string email;
        public string password;
        public string nickname;
    }

    [System.Serializable]
    public class GameData
    {
        public List<int> character_id = new();
        public List<int> character_level = new();
        public List<string> achieve_name = new();
        public List<int> achieve_value = new();
        public int gold;
    }

    [System.Serializable]
    public class OptionData
    {
        public float masterVolume;
        public float backgroundVolume;
        public float SFXVolume;
        public int FPS;
    }
}
