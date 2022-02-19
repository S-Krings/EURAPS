using Microsoft.MixedReality.Toolkit.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CodeEditorBlock : MonoBehaviour
{
    [SerializeField]
    private Text args;
    [SerializeField]
    private Text comandName;
    public Transform touchingPoint;
    public RuntimeCommand cmd;
    private int counter = 0;

    [HideInInspector]
    public CodeEditor codeEditor;
    //[HideInInspector]
    public CodeEditorBlock topBlock = null;
    //[HideInInspector]
    public CodeEditorBlock bottomBlock = null;
    public int runtimeID = 0;
    public bool isLast = false; 

    public void lockBlock(ManipulationEventData data)
    {
        if(data.ManipulationSource.gameObject.Equals(gameObject))
        {
            codeEditor.SnapOrNot(this);
        }
    }
    public void SetCommand(RuntimeCommand cmd)
    {
        this.cmd = cmd;
        runtimeID = cmd.runtimeID;
        if (this.cmd.arguments != null && this.cmd.arguments.Count > 0)
        {
            args.text = "(";
            int i = 0;
            foreach (var arg in this.cmd.arguments)
            {
                args.text += arg;
                if (i < this.cmd.arguments.Count - 1)
                {
                    args.text += "/";
                }
                i++;
            }
            args.text += ")";
        }
        else
        {
            args.text = "";
        }
        comandName.text = cmd.command.name;
    }
    public void OnModifyButtonPress()
    {
        Debug.Log("Modify Block:" + this.cmd.runtimeID);
        if(this.cmd.command.ArgumentModifier != null)
        {
            foreach(var mod in this.cmd.command.ArgumentModifier)
            {
                if(mod is ArgModifier argMod){
                    List<object> cpList = null;
                    if (this.cmd.arguments != null)
                    {
                        cpList = new List<object>();
                        cpList.AddRange(this.cmd.arguments);
                    }
                    argMod.ModArgs(this.cmd, cpList, this.cmd.arguments, this.cmd);
                }
            }
        }
    }
    public void Update()
    {
        if(counter >= 150)
        {
            if(this.cmd.arguments != null && this.cmd.arguments.Count > 0)
            {
                args.text = "(";
                int i = 0;
                foreach (var arg in this.cmd.arguments)
                {
                    args.text += arg;
                    if (i < this.cmd.arguments.Count - 1)
                    {
                        args.text += "/";
                    }
                    i++;
                }
                args.text += ")";
                counter = 0;
            }   
            else
            {
                comandName.text = this.cmd?.command?.name;
                args.text = "";
            }
        }
        else
            counter++;      
    }
    public void SetBottomBlock(CodeEditorBlock block)
    {
        var currBlock = block;
        while(currBlock != null)
        {
            ProgramStore.GetInstance().AddCommand(currBlock.cmd);
            currBlock = currBlock.bottomBlock;
        }
        this.bottomBlock = block;
        block.transform.SetParent(this.transform);
    }
    public void AddCmdToStore()
    {
        ProgramStore.GetInstance().AddCommand(this.cmd);
    }
    public void RemoveBottomBlocks()
    {
        var bb = this.bottomBlock;
        while (bb != null)
        {
            ProgramStore.GetInstance().RemoveCommand(bb.cmd);
            bb = bb.bottomBlock;
        }
        this.bottomBlock = null;
    }
    public void RemoveThisCommand()
    {
        ProgramStore.GetInstance().RemoveCommand(cmd);
    }
    public void SetTopBlock(CodeEditorBlock block)
    {
        this.topBlock = block;
    }
    public void OnDestroy()
    {
        this.cmd.HelperElements.ForEach((element) =>
        {
            if (element is GameObject go)
            {
                DestroyImmediate(go);
            }
        });
    }
}
