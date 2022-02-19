using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class ProgramStore : MonoBehaviour
{
    private static ProgramStore Instance;
    //private List<SPEAREDCommand> commandTypes;
    private List<UnityAction> updateActions = new List<UnityAction>();
    // Start is called before the first frame update
    public List<RuntimeCommand> commands = new List<RuntimeCommand>();
    void Awake()
    {
        if(Instance == null)
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
        }
        else if(Instance != this)
        {
            //this.commands = Instance.commands;
            //this.commandTypes = Instance.commandTypes;
            Destroy(Instance.gameObject);
            Instance = this;
        }
    }

    public static ProgramStore GetInstance()
    {
        if(Instance == null)
        {
            Debug.LogError("No ProgrammStore initialized");
        }
        return Instance;
    }

    public void moveCommandFromPosToPos(int i, int j)
    {
        var command = this.commands[i];
        this.commands.RemoveAt(i);
        this.commands.Insert(j, command);
        this.notifyAll();
    }
    public void AddCommand(RuntimeCommand command)
    {
        this.commands.Add(command);
        if(command.HelperElements != null)
        {
            if(command.HelperElements.Count >= 2 && command.HelperElements[1] is SpearedPosition spos)
            {
                spos.InUse = true;
                spos.SetPositionInEditor(this.commands.Count-2);
            }
        }
        // this.DebugPathPoints();
        this.notifyAll();
    }
    private void DebugPathPoints()
    {
        Debug.Log("Debug points");
        foreach(var cmd in this.commands)
        {
            if(cmd.HelperElements != null)
            {
                Debug.Log((cmd.HelperElements[1] as SpearedPosition).GetPosition());
                Debug.Log((cmd.HelperElements[1] as SpearedPosition).GetPositionInEditor());
            }
        }
    }
    public void RemoveCommand(RuntimeCommand command)
    {
        this.commands.Remove(command);
        if (command.HelperElements != null)
        {
            if (command.HelperElements.Count >= 2 && command.HelperElements[1] is SpearedPosition spos)
            {
                spos.InUse = false;
                spos.SetPositionInEditor(-1);
            }
        }
        this.notifyAll();
    }
    public List<RuntimeCommand> getCommands()
    {
        return this.commands.Select(x => {
            var runtimeCMD = ScriptableObject.CreateInstance<RuntimeCommand>();
            runtimeCMD.command = x.command;
            runtimeCMD.arguments = x.arguments;
            return runtimeCMD;
        }).ToList(); ;
    }

    public List<RuntimeCommand> getRealCommands()
    {
        return this.commands;
    }

    private void notifyAll()
    {
        foreach(var updateAction in updateActions)
        {
            if(updateAction != null)
            {
                updateAction.Invoke();
            }
        }
    }

    public void registerForUpdates(UnityAction action)
    {
        updateActions.Add(action);
    }
    public bool hasCommand(RuntimeCommand cmd)
    {
        foreach(var currCmd in commands)
        {
            if(currCmd == cmd)
            {
                return true;
            }
        }
        return false;
    }
}
