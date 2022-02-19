using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuntimeCommand : ScriptableObject
{
    public static int globalMaxId = 0;
    // debug variable
    public int runtimeID;
    public SPEAREDCommand command;
    public List<object> arguments;
    public List<object> HelperElements;
}
