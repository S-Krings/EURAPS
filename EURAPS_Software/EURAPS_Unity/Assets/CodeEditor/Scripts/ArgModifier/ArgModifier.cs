using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ArgModifier: ScriptableObject
{
    public abstract void ModArgs(RuntimeCommand inCmd, List<object> inargs, List<object> outargs, RuntimeCommand outCmd);
}
