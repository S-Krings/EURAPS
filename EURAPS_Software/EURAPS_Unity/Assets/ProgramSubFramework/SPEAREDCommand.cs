using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Command", menuName = "SPEARED/Command", order = 1)]
public class SPEAREDCommand : ScriptableObject
{
    public List<ArgModifier> ArgumentModifier;
    public bool isLegoCommand;
    public LegoCommand Lcommand;
    public bool isDobotCommand;
    public LegoCommand Dcommand;
    public string name;
    public SPEAREDCommandSignature Signature;
}
