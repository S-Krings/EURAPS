using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Command", menuName = "SPEARED/CommandSignature", order = 1)]
public class SPEAREDCommandSignature1 : SPEAREDCommandSignature
{
    public ArgumentType argumentType;
    public string argumentName;
    public enum ArgumentType
    {
        Vector2d,
        Vector3d,
        boolean
    }
}
