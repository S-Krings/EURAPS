using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuntimeCommandFactory : MonoBehaviour
{
    public SPEAREDCommand moveSPEAREDCommand;
    public SPEAREDCommand clawUpSPEAREDCommand;
    public SPEAREDCommand clawDownSPEAREDCommand;

    public RuntimeCommandFactory()
    {
        
    }

    public RuntimeCommand createMovementCommand(Vector2 positions)
    {
        RuntimeCommand runtimeCommand = ScriptableObject.CreateInstance<RuntimeCommand>();
        runtimeCommand.command = moveSPEAREDCommand;
        runtimeCommand.arguments = new List<object>{positions};
        return runtimeCommand;
    }

    public RuntimeCommand createClawUpCommand()
    {
        RuntimeCommand runtimeCommand = ScriptableObject.CreateInstance<RuntimeCommand>();
        runtimeCommand.command = clawUpSPEAREDCommand;
        return runtimeCommand;
    }

    public RuntimeCommand createClawDownCommand()
    {
        RuntimeCommand runtimeCommand = ScriptableObject.CreateInstance<RuntimeCommand>();
        runtimeCommand.command = clawDownSPEAREDCommand;
        return runtimeCommand;
    }

    public List<RuntimeCommand> createSampleProgram()
    {
        List<RuntimeCommand> commands = new List<RuntimeCommand>();
        commands.Add(this.createClawUpCommand());
        commands.Add(this.createMovementCommand(new Vector2(0.6f, 0)));
        commands.Add(this.createClawDownCommand());
        commands.Add(this.createMovementCommand(new Vector2(1.1f, 0)));
        commands.Add(this.createClawUpCommand());
        commands.Add(this.createMovementCommand(new Vector2(0.9f, 0.4f)));
        return commands;
    }
}
