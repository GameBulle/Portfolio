using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WaveClearUI : MonoBehaviour
{
    System.Text.StringBuilder strBuilder = new System.Text.StringBuilder();

    [Header("UI")]
    [SerializeField] WaveClearResultUI resultUI;
    [SerializeField] GameObject selectUI;

    [Header("MakeUI")]
    [SerializeField] GameObject selectMakeUI;
    [SerializeField] MakeArrowUI makeArrowUI;
    [SerializeField] MakeBowUI makeBowUI;

    [Header("SettingUI")]
    [SerializeField] GameObject SettingUI;
    [SerializeField] SettingPlayerUI settingPlayerUI;
    [SerializeField] SettingNPCUI settingNPCUI;

    [Header("DefenceLineUI")]
    [SerializeField] DefenceLineUI defenceLineUI;

    [Header("FindSurvivorUI")]
    [SerializeField] FindSurvivorUI findSurvivorUI;

    [Header("GameClearUI")]
    [SerializeField] GameObject gameClearUI;

    [Header("GameOverUI")]
    [SerializeField] GameObject gameOverUI;

    [Header("Text")]
    [SerializeField] TextMeshProUGUI stuff;

    

    public void Initialize()
    {
        resultUI.gameObject.SetActive(false); ;
        selectUI.SetActive(false);

        selectMakeUI.SetActive(false);
        makeArrowUI.gameObject.SetActive(false);
        makeBowUI.gameObject.SetActive(false);

        SettingUI.SetActive(false);
        settingPlayerUI.gameObject.SetActive(false);

        defenceLineUI.gameObject.SetActive(false);

        gameClearUI.gameObject.SetActive(false);
        gameOverUI.gameObject.SetActive(false);

        stuff.gameObject.SetActive(false);
    }
    
    public void WaveClear()
    {
        resultUI.gameObject.SetActive(true);
        resultUI.WaveClear();
        selectUI.SetActive(false);

        selectMakeUI.SetActive(false);
        makeArrowUI.gameObject.SetActive(false);
        makeBowUI.gameObject.SetActive(false);

        SettingUI.SetActive(false);
        settingPlayerUI.gameObject.SetActive(false);
        settingNPCUI.gameObject.SetActive(false);

        defenceLineUI.gameObject.SetActive(false);

        findSurvivorUI.gameObject.SetActive(false);

        stuff.gameObject.SetActive(false);
    }

    public void ResultToSelect()
    {
        resultUI.gameObject.SetActive(false);

        selectUI.SetActive(true);
        stuff.gameObject.SetActive(true);
        DisplayStuff();
    }

    public void DisplayStuff()
    {
        strBuilder.Clear();
        strBuilder.AppendFormat("»À Á¶°¢ : {0}", ItemMgr.Instance.Bone);
        strBuilder.AppendFormat("   ");
        strBuilder.AppendFormat("¼è Á¶°¢ : {0}", ItemMgr.Instance.Iron);
        strBuilder.AppendFormat("   ");
        strBuilder.AppendFormat("¾ÏÈæ¹°Áú : {0}", ItemMgr.Instance.DarkMaterial);
        stuff.text = strBuilder.ToString();
    }

    public void SelectToMake()
    {
        selectUI.SetActive(false);

        selectMakeUI.SetActive(true);
    }

    public void MakeToSelect()
    {
        selectUI.SetActive(true);

        selectMakeUI.SetActive(false);
    }

    public void SelectToDefenceLine()
    {
        selectUI.SetActive(false);

        defenceLineUI.gameObject.SetActive(true);
        defenceLineUI.Initialize();
    }

    public void DefenceLineToSelect()
    {
        selectUI.SetActive(true);

        defenceLineUI.gameObject.SetActive(false);
    }

    // Make
    public void MakeToArrow()
    {
        selectMakeUI.SetActive(false);

        makeArrowUI.gameObject.SetActive(true);
        makeArrowUI.Initialize();
    }

    public void ArrowToMake()
    {
        selectMakeUI.SetActive(true);

        makeArrowUI.gameObject.SetActive(false);
    }

    public void MakeToBow()
    {
        selectMakeUI.SetActive(false);

        makeBowUI.gameObject.SetActive(true);
        makeBowUI.Initialize();
    }

    public void BowToMake()
    {
        selectMakeUI.SetActive(true);

        makeBowUI.gameObject.SetActive(false);
    }

    // Setting
    public void SelectToSetting()
    {
        selectUI.SetActive(false);

        SettingUI.SetActive(true);

        stuff.gameObject.SetActive(false);
    }

    public void SettingToSelect()
    {
        selectUI.SetActive(true);

        SettingUI.SetActive(false);

        stuff.gameObject.SetActive(true);
    }

    public void SettingToPlayer()
    {
        SettingUI.SetActive(false);

        settingPlayerUI.gameObject.SetActive(true);
        settingPlayerUI.Initialize();
    }

    public void SettingToNPC()
    {
        SettingUI.SetActive(false);

        settingNPCUI.gameObject.SetActive(true);
        settingNPCUI.Initialize();
    }

    public void PlayerToSetting()
    {
        SettingUI.SetActive(true);

        settingPlayerUI.gameObject.SetActive(false);
    }

    public void NPCToSetting()
    {
        SettingUI.SetActive(true);

        settingNPCUI.gameObject.SetActive(false);
    }

    // FindSurvivor
    public void SelectToFind()
    {
        selectUI.SetActive(false);

        findSurvivorUI.gameObject.SetActive(true);
        findSurvivorUI.Initialize();
    }

    public void FindToSelect()
    {
        selectUI.SetActive(true);

        findSurvivorUI.gameObject.SetActive(false);
    }

    // GameClear
    public void GameClear()
    {
        gameClearUI.SetActive(true);
    }

    //GameOver
    public void GameOver()
    {
        gameOverUI.SetActive(true);
    }
}
