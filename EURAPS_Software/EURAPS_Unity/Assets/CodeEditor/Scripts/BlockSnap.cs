using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockSnap : MonoBehaviour
{
    public bool isStart = false;
    BlockSnap topBlock = null;
    BlockSnap bottomBlock = null;
    BlockSnap intermediateBlock = null;
    public RuntimeCommand Rcommand;
    public bool snaping = false;
    private void OnTriggerEnter(Collider other)
    {
        if (!isStart) {
            BlockSnap component;
            if((topBlock != null && (other.transform.position-transform.position).magnitude < (topBlock.transform.position-transform.position).magnitude) 
                || (topBlock == null)){ 
                if (other.TryGetComponent<BlockSnap>(out component))
                {
                    
                    Debug.Log("Enter zone");
                    // Calc docking point
                    if (component.bottomBlock == null)
                    {
                        var coordinateDiff = transform.position - component.transform.position;
                        if (coordinateDiff.y < 0)
                        {
                            topBlock = component;
                            component.setBottomBlock(this);
                        }
                    }
                }
            }
        }       
    }
    private void OnTriggerExit(Collider other)
    {
        BlockSnap component;
        if (other.TryGetComponent<BlockSnap>(out component))
        {
            Debug.Log("Exit zone");
            if(topBlock == component && !snaping)
            {
                topBlock.unsetBottomBlock();
                topBlock = null;
            }
        }
    }
    public BlockSnap getTopBlock()
    {
        return topBlock;
    }
    public void setBottomBlock(BlockSnap block)
    {
        ProgramStore.GetInstance().AddCommand(block.Rcommand);
        bottomBlock = block;
    }
    public void unsetBottomBlock()
    {
        ProgramStore.GetInstance().RemoveCommand(bottomBlock.Rcommand);
        bottomBlock = null;
    }
    public BlockSnap getBottomBlock()
    {
        return bottomBlock;
    }
    public bool getDirection()
    {
        return true;
    }
}
