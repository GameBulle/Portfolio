using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RecallStone : ActionObject
{
    [SerializeField] PortalData recallData;
    [SerializeField] ChangeablePortalData data;
    bool isRecall = false;
    Vector3 pos;

    public override void Action()
    {
        SoundMgr.Instance.PlaySFXAudio("Action");
        DontDestroyOnLoad(this);
        if(!isRecall)
        {
            data.ChangeMapID(MapMgr.Instance.GetMapData(SceneManager.GetActiveScene().name).MapID);
            data.ChangePlayerPos(gameObject.transform.position);
            data.ChangePlayerAngle(Vector3.zero);

            isRecall = true;
            SceneLoader.Instance.LoadScene(recallData);
            gameObject.transform.position = recallData.PlayerPos;
        }
        else
        {
            SceneLoader.Instance.LoadScene(data);
            Destroy(gameObject);
            RemoveInteractionToList();
        }
    }

    public void SetPosition(Vector3 pos)
    {
        gameObject.transform.position = pos;
    }
}
