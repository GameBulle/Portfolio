using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Changeable Portal Data", menuName = "ScriptableObject/Changeable Portal", order = 1)]
public class ChangeablePortalData : PortalData
{
    public void ChangeMapID(int mapID) { nextMapID = mapID; }
    public void ChangePlayerPos(Vector3 pos) { playerPos= pos; }
    public void ChangePlayerAngle(Vector3 angle) { playerAngle= angle; }
}
