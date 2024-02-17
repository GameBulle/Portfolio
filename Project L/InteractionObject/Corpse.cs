using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Corpse : ActionObject
{
    public override void Action()
    {
        SoundMgr.Instance.PlaySFXAudio("Action");
        QuestMgr.Instance.QuestComplete(questID);
    }
}
