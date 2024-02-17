using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Event : MonoBehaviour
{
    [SerializeField] Vector3 pos;
    [SerializeField] Vector3 angle;
    [SerializeField] int questID;
    [SerializeField] MP4Loader.Video video;

    bool isEvent = false;
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag.Equals("Player"))
        {
            if (!isEvent)
            {
                if (questID == -1)
                    isEvent = true;
                else if (QuestMgr.Instance.GetQuestData(questID).State == QuestMgr.QuestState.Proceed)
                    isEvent = true;

                if (isEvent)
                {
                    MP4Loader.Instance.LoadVideo(video);
                    MonsterMgr.Instance.SetBoss(video, pos, angle);
                }
            }
        }
    }
}
