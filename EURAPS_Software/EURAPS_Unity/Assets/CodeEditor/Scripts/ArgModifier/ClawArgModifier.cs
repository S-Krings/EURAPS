using Microsoft.MixedReality.Toolkit.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Command", menuName = "SPEARED/ArgumentModifier/ClawArgumentModifier", order = 1)]
public class ClawArgModifier : ArgModifier
{
    public SPEAREDCommand ClawDown;
    public SPEAREDCommand ClawUp;
    private List<object> inputParams;
    private List<object> ouputParams;

    float height;
    private GameObject selector;
    public override void ModArgs(RuntimeCommand cmdIn, List<object> inargs, List<object> outargs, RuntimeCommand cmdOut)
    {
        if (cmdIn.command.Equals(ClawDown))
        {
            cmdOut.command = ClawUp;                                    
        }
        else
        {
            cmdOut.command = ClawDown;
        }

    }
}
