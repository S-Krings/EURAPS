using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SpearedPosition : ScriptableObject, IComparable<SpearedPosition> 
{
    private Vector3 position;
    private int positionInEditor;
    public bool InUse = false;
    public List<UnityAction<SpearedPosition>> callbacks = new List<UnityAction<SpearedPosition>>();

    public Vector3 GetPosition()
    {
        return position;
    }
    public void SetPosition(Vector3 position)
    {
        this.position = position;
        this.notify();
    }
    public int GetPositionInEditor()
    {
        return positionInEditor;
    }
    public void SetPositionInEditor(int positionInEditor)
    {
        this.positionInEditor = positionInEditor;
        this.notify();
    }
    public void AddListener(UnityAction<SpearedPosition> action)
    {
        this.callbacks.Add(action);
    }
    private void notify()
    {
        foreach(var ele in callbacks)
        {
            if(ele != null)
            {
                ele.Invoke(this);
            }
        }
    }
    public int CompareTo(SpearedPosition obj)
    {
        if(obj.GetPositionInEditor() > this.GetPositionInEditor())
        {
            return -1;
        }
        else if(obj.GetPositionInEditor() < this.GetPositionInEditor())
        {
            return 1;
        }
        else
        {
            return 0;
        }
    }
}
