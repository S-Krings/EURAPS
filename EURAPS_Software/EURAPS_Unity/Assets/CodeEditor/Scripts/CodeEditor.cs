using Microsoft.MixedReality.Toolkit.UI;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class CodeEditor : MonoBehaviour
{
    [Header("Commands")]
    public SPEAREDCommand moveCommand;
    public SPEAREDCommand brutemoveCommand;
    public SPEAREDCommand clawCommand;
    [Header("Technical References")]
    public SimulationCommunicator simcom;
    public GameObject blockCommand;
    public Transform programRoot;
    public GameObject currentScenario;
    public GameObject scenarioPrefab;
    public GameObject programCodeUnformatted;
    public GameObject bin;
    [Header("Custom Parameters")]
    public float spawnDistance = 0.7f;
    public Transform TouchingPoint;
    public float distanceSnapThreshold = 0.6f;
    public float distanceSnapLock = 0.2f;
    public float distanceDeleteThreshold = 0.6f;
    public SyntaxColor syntaxColor;
    string colorMove;
    string colorBrute;
    string colorClaw;
    string colorBoolTrue;
    string colorBoolFalse;
    [Header("Debug Vars")]
    [SerializeField]
    private CodeEditorBlock lastBlock;
    private CodeEditorBlock firstBlock;
    public static CodeEditor MainInstance;
    public MoveArgModifier moveMod;
    public LineRenderer pathManager;
    private GameObject plane; 

    public void Awake()
    {
        if(CodeEditor.MainInstance == null)
        {
            CodeEditor.MainInstance = this;
        }
        else
        {
            Destroy(CodeEditor.MainInstance.gameObject);
        }

    }
    public void CreateMoveCommand()
    {
        var cmd = ScriptableObject.CreateInstance<RuntimeCommand>();
        cmd.command = moveCommand;
        cmd.arguments = new List<object>();
        cmd.arguments.Add(1.0f);
        cmd.arguments.Add(1.0f);
        cmd.runtimeID = RuntimeCommand.globalMaxId;
        RuntimeCommand.globalMaxId++;
        initCommandInScene(cmd);
    }
    public void CreateBruteMoveCommand()
    {
        var cmd = ScriptableObject.CreateInstance<RuntimeCommand>();
        cmd.command = brutemoveCommand;
        cmd.arguments = new List<object>();
        cmd.arguments.Add(1);
        cmd.arguments.Add(2); 
        cmd.runtimeID = RuntimeCommand.globalMaxId;
        RuntimeCommand.globalMaxId++;
        initCommandInScene(cmd);
    }
    public void CreateClawMoveCommand()
    {
        var cmd = ScriptableObject.CreateInstance<RuntimeCommand>();
        cmd.command = clawCommand;
        cmd.runtimeID = RuntimeCommand.globalMaxId;
        RuntimeCommand.globalMaxId++;
        initCommandInScene(cmd);
    }
    public void DrawCommand()
    {
        if(plane == null)
        {
            plane = GameObject.Find("Plane");
        }

        if (plane != null)
        {
            moveMod.height = GameObject.Find("Plane").transform.position.y;
        }
        else
        {
            moveMod.height = 0;
        }
        moveMod.drawMode = true;
        if (moveMod.selector == null)
        {
#if UNITY_ANDROID
            moveMod.selector = Instantiate(moveMod.targetSelectorPrefab, Camera.main.transform.position + Camera.main.transform.forward * 0.5f, Quaternion.identity);
            moveMod.selector.transform.position = plane.transform.position;
            var dragOnFloor = moveMod.selector.GetComponent<DragOnFloor>();
            dragOnFloor.floorHeight = moveMod.height;
            dragOnFloor.inputParams = moveMod.inputParams;
            dragOnFloor.outputParams = moveMod.ouputParams;
            dragOnFloor.outCmd = this.moveMod.outCmd;
            dragOnFloor.waypointMaterial = moveMod.waypointMaterial;
            dragOnFloor.DrawLineMode = true;
            var drawLine = moveMod.selector.GetComponent<DrawLine>();
            drawLine.drawMode = true;
#elif UNITY_WSA
            moveMod.selector = Instantiate(moveMod.targetSelectorPrefabHL, Camera.main.transform.position + Camera.main.transform.forward * 0.5f, Quaternion.identity);
            DrawLine drawLine = moveMod.selector.GetComponent<DrawLine>();
            drawLine.drawMode = true;
            if(drawLine != null)
            {
                drawLine.waypointMaterial2 = moveMod.waypointMaterial;
            }
            if(ProgramStore.GetInstance().getCommands().Count > 0)
            {
                var lastCmd = ProgramStore.GetInstance().getCommands()[ProgramStore.GetInstance().getCommands().Count - 1];
                float x = 0;
                float z = 0;
                if(lastCmd.arguments != null && lastCmd.arguments.Count == 2)
                {
                    x = (float)lastCmd.arguments[0];
                }
                if (lastCmd.arguments != null && lastCmd.arguments.Count == 2)
                {
                    z = (float)lastCmd.arguments[1];
                }


                moveMod.selector.transform.position = plane.transform.position + new Vector3(x, 0, z) + new Vector3(0, 0.5f, 0);
            }
            else
            {
                moveMod.selector.transform.position = plane.transform.position + new Vector3(0, 0.5f, 0);
            }     
            var snaper = moveMod.selector.GetComponentInChildren<TargetSpherePlaneSnap>();
            if (plane != null)
            {
                snaper.planePoint = plane.transform.position;
                snaper.normalVec = plane.transform.up;
            }
            else
            {

                snaper.planePoint = Vector3.zero;
                snaper.normalVec = new Vector3(0, 1, 0);
            }

            snaper.PlaneSet = true;
            ///Drag on flor is not in prefab anymore
            /*selector.GetComponentInChildren<DragOnFloor>().selecting = true;
            Debug.Log(selector.name);*/
            var manHandle = moveMod.selector.GetComponentInChildren<ObjectManipulator>();
            if (manHandle != null)
            {
#if UNITY_WSA
                manHandle.OnManipulationStarted.RemoveAllListeners();
                manHandle.OnManipulationEnded.RemoveAllListeners();
                manHandle.OnManipulationStarted.AddListener((ManipulationEventData evt) => drawLine.OnManipulationStarted());
                manHandle.OnManipulationEnded.AddListener((ManipulationEventData evt) => moveMod.OnRelease());
                manHandle.OnManipulationEnded.AddListener((ManipulationEventData evt) => {
                    drawLine.OnManipulationEnded();
                });
#endif
            }

#endif
        }
        else
        {
            if (ProgramStore.GetInstance().getCommands().Count > 0)
            {
                var lastCmd = ProgramStore.GetInstance().getCommands()[ProgramStore.GetInstance().getCommands().Count - 1];
                float x = 0;
                float z = 0;
                if (lastCmd.arguments != null && lastCmd.arguments.Count == 2)
                {
                    x = (float)lastCmd.arguments[1];
                }
                if (lastCmd.arguments != null && lastCmd.arguments.Count == 2)
                {
                    z = (float)lastCmd.arguments[0];
                }


                moveMod.selector.transform.position = plane.transform.position + new Vector3(x, 0, z) + new Vector3(0, 0.5f, 0);
            }
            else
            {
                moveMod.selector.transform.position = plane.transform.position + new Vector3(0, 0.5f, 0);
            }
#if UNITY_ANDROID
            moveMod.selector.GetComponent<DragOnFloor>().selecting = true;
            moveMod.selector.GetComponent<DragOnFloor>().floorHeight = moveMod.height;
            moveMod.selector.GetComponent<DragOnFloor>().inputParams = moveMod.inputParams;
            moveMod.selector.GetComponent<DragOnFloor>().outputParams = moveMod.ouputParams;
            moveMod.selector.GetComponent<DragOnFloor>().outCmd = moveMod.outCmd;
            moveMod.selector.GetComponent<DragOnFloor>().waypointMaterial = moveMod.waypointMaterial;
            moveMod.selector.GetComponent<DragOnFloor>().DrawLineMode = true;
#endif
            var drawLine = moveMod.selector.GetComponent<DrawLine>();
            drawLine.drawMode = true;
        }
    }
    private CodeEditorBlock initCommandInScene(RuntimeCommand cmd)
    {
        var blockCmd = Instantiate(blockCommand, new Vector3(transform.position.x + spawnDistance, transform.position.y, transform.position.z), Quaternion.identity).GetComponent<CodeEditorBlock>();
        blockCmd.transform.SetParent(programRoot);
        blockCmd.SetCommand(cmd);
        blockCmd.codeEditor = this;
        return blockCmd;
    }
    public void simulateProgramm()
    {
        DebugLogProgram();
        simcom.simulate();
        /*string output = this.highlightSyntax();

        this.programCodeUnformatted.GetComponent<Text>().text = output;

        GameObject onScreenCodePanel = GameObject.Find("Menu_NXTCode_Panel/Viewport/Content/AR_DoBotCode_Panel/Highlighted_text");
        try
        {
            onScreenCodePanel.GetComponent<Text>().text = output;
        }
        catch (System.NullReferenceException e)
        {
            Debug.Log("Code Panel not opened, NullReferenceException: " + e.ToString());
        }*/
    }
    public void makeAndRunProgramm()
    {
        var nxtexecuter = NXTExecutor.GetInstance();
        if (nxtexecuter != null)
        {
            nxtexecuter.send();
        }
        /*string output = this.highlightSyntax();

        this.programCodeUnformatted.GetComponent<Text>().text = output;

        GameObject onScreenCodePanel = GameObject.Find("Menu_NXTCode_Panel/Viewport/Content/AR_DoBotCode_Panel/Highlighted_text");
        try
        {
            onScreenCodePanel.GetComponent<Text>().text = output;
        }
        catch (System.NullReferenceException e)
        {
            Debug.Log("Code Panel not opened, NullReferenceException: " + e.ToString());
        }*/
    }
    public void resetScenario()
    {
        var pos = currentScenario.transform.position;
        var rotation = currentScenario.transform.rotation;
        DestroyImmediate(currentScenario);
        var scenario = Instantiate(scenarioPrefab);
        scenario.transform.position = pos;
        scenario.transform.rotation = rotation;
        Transform plane = scenario.transform.Find("NXTWithPlane_/Plane");
        plane.eulerAngles = new Vector3(0, plane.eulerAngles.y, 0);
        scenario.SetActive(true);    
        this.currentScenario = scenario;
        simcom = scenario.GetComponentInChildren<SimulationCommunicator>();
    }
    public void resetRobot()
    {
        NXTExecutor.GetInstance().sendReset();
    }
    public void DebugLogProgram()
    {
        string program = "";
        program+="-----------------------------------------------------------------------------------"+ "\n";
        foreach (var command in ProgramStore.GetInstance().commands)
        {
            if(command.arguments != null)
            {
                program += $"{command.command.name}( {string.Join(",", command.arguments)})" + "\n";
            }
            else
            {
                program += $"{command.command.name}()" +"\n";
            }        
        }
        program += "-----------------------------------------------------------------------------------" + "\n";
        Debug.Log(program);
    }
    public string highlightSyntax()
    {

        //colorToggleSuction = "#" + syntaxColor.toggleSuctionColor[0].ToString("X2") + syntaxColor.toggleSuctionColor[1].ToString("X2") + syntaxColor.toggleSuctionColor[2].ToString("X2");
        colorMove = "#" + syntaxColor.moveColor[0].ToString("X2") + syntaxColor.moveColor[1].ToString("X2") + syntaxColor.moveColor[2].ToString("X2");
        colorBoolTrue = "#" + syntaxColor.trueColor[0].ToString("X2") + syntaxColor.trueColor[1].ToString("X2") + syntaxColor.trueColor[2].ToString("X2");
        colorBoolFalse = "#" + syntaxColor.falseColor[0].ToString("X2") + syntaxColor.falseColor[1].ToString("X2") + syntaxColor.falseColor[2].ToString("X2");

        //new color for BruteMove
        colorBrute = "#" + syntaxColor.bruteColor[0].ToString("X2") + syntaxColor.bruteColor[1].ToString("X2") + syntaxColor.bruteColor[2].ToString("X2");

        //new color for Claw command
        colorClaw = "#" + syntaxColor.clawColor[0].ToString("X2") + syntaxColor.clawColor[1].ToString("X2") + syntaxColor.clawColor[2].ToString("X2");

        /* Debug.Log("From Syntax color box: ");
        Debug.Log("Move: " + colorMove);
        Debug.Log("Move: " + bruteMove);
        
        Debug.Log("Inside the Syntax Highlighter for Android Screen Panel"); */
        string intermediateOutput0 = "";
        string intermediateOutput1 = "";
        string intermediateOutput2 = "";
        string intermediateOutput3 = "";
        string intermediateOutput4 = "";
        string output = "";
        // TODO
        string program = string.Join("\n", ProgramStore.GetInstance().getCommands().Select(cmd => CultureInfo.CurrentCulture.TextInfo.ToTitleCase(cmd.command.name) + " " + string.Join(",",cmd.arguments)));
        //highlighting without any condition
        intermediateOutput0 = program.Replace("Claw", "<color=" + colorClaw + ">Claw</color>");
        intermediateOutput1 = intermediateOutput0.Replace("True", "<color=" + colorBoolTrue + ">UP</color>");
        intermediateOutput2 = intermediateOutput1.Replace("False", "<color=" + colorBoolFalse + ">DOWN</color>");
        intermediateOutput3 = intermediateOutput2.Replace("Brute", "<color=" + colorBrute + ">Brute</color>");
        intermediateOutput4 = intermediateOutput3.Replace("Move", "<color=" + colorMove + ">Move</color>");

        output = intermediateOutput4;
        Debug.Log("string for syntax highlighting :" + output);
        return output;
    }
    public void SnapOrNot(CodeEditorBlock block)
    {
        // Check whether should delete because is to close to bin
        // Be carefull this only works if the editor do not rotate
        var diff = (block.transform.position - bin.transform.position);
        var angle = Vector3.Angle(block.transform.position - bin.transform.position, bin.transform.right);
        if (angle < 10 && diff.magnitude < this.distanceDeleteThreshold)
        {
            if(block.topBlock != null)
            {
                block.topBlock.bottomBlock = null;
                lastBlock = block.topBlock;
                RemoveBottomBlockTopReferenceInCase(block);
                block.RemoveBottomBlocks();
                block.RemoveThisCommand();
            }
            else if (firstBlock !=null && firstBlock.Equals(block))
            {
                lastBlock = null;
                RemoveBottomBlockTopReferenceInCase(block);
                block.RemoveBottomBlocks();
                ProgramStore.GetInstance().commands.Clear();
            }
            Destroy(block.gameObject);

        }
        else if(block.topBlock == null)
        {
            // typically means this block is not liked to the editor by now
            
            if (lastBlock == null)
            {
                // typically means the editor has currently 0 code blocks
                // Add as first block if close enough
                if ((block.transform.position - this.TouchingPoint.transform.position).y < 0 
                    && (block.transform.position - this.TouchingPoint.transform.position).magnitude < distanceSnapThreshold && firstBlock == null)
                {
                    Snap(this.transform, block.transform);
                    block.AddCmdToStore();
                    this.firstBlock = block;
                    if(lastBlock != null)
                    {
                        this.lastBlock.isLast = false;
                    }
                    this.lastBlock = block;
                    block.isLast = true;
                    return;
                }
            }
            else if ((block.transform.position - lastBlock.touchingPoint.transform.position).y < 0
                // Add as next block if close enough
                && (block.transform.position - lastBlock.touchingPoint.transform.position).magnitude < distanceSnapThreshold)
            {
                block.SetTopBlock(this.lastBlock);
                this.lastBlock.SetBottomBlock(block);
                Snap(lastBlock.transform, block.transform);
                if (lastBlock != null)
                {
                    this.lastBlock.isLast = false;
                }
                this.lastBlock = block;
                block.isLast = true;
                RemoveBottomBlockTopReferenceInCase(block);
            }
            // Remove first block if too far away from editor (execption because first block also has no topblock)
            if (this.firstBlock != null && this.firstBlock.Equals(block) && (this.TouchingPoint.transform.position - block.transform.position).magnitude > distanceSnapThreshold)
            {
                this.lastBlock = null;
                this.firstBlock = null;
                ProgramStore.GetInstance().commands.Clear();
                block.topBlock = null;
                RemoveBottomBlockTopReferenceInCase(block);
            }
        }
        else
        {
            // if linked block is to far away from its topblock remove it from internal program (and remove liked structure to it)
            if ((block.topBlock.touchingPoint.transform.position - block.transform.position).magnitude > distanceSnapThreshold)
            {
                if (lastBlock != null && ProgramStore.GetInstance().hasCommand(this.lastBlock.cmd)) {
                    this.lastBlock.isLast = false;
                    this.lastBlock = block.topBlock;
                    lastBlock.isLast = true;
                }
                // RemoveBottomBlockTopReferenceInCase(block.topBlock);
                block.transform.SetParent(this.programRoot);
                if (block != firstBlock)
                {
                    block.topBlock.RemoveBottomBlocks();                 
                }
                else
                {
                    this.firstBlock = null;
                }
                block.topBlock = null;
            }
            else
            {
                // snap block again if it has not gone to far :)
                Snap(block.topBlock.transform, block.transform);
            }
        }
        
    }
    private void RemoveBottomBlockTopReferenceInCase(CodeEditorBlock block)
    {
        if(block.bottomBlock != null)
        {
            block.bottomBlock.topBlock = null;
            block.transform.SetParent(this.programRoot);
        }
    }
    private void Snap(Transform lastblock, Transform block)
    {
        block.position = lastblock.position - new Vector3(0, distanceSnapLock, 0);
        ApplyColorAsFeedback(block);
    }
    private void ApplyColorAsFeedback(Transform block)
    {
        var mesh = block.transform.GetChild(0).GetComponent<MeshRenderer>();
        mesh.material.color = new Color(255, 0, 0);
    }

    public void SyncFromProgramStore()
    {
        firstBlock = null;
        this.lastBlock = null;
        while(programRoot.transform.childCount > 0)
        {
            DestroyImmediate(programRoot.transform.GetChild(0).gameObject);
        }
        var last_block = this.transform;
        var list = ProgramStore.GetInstance().getRealCommands();
        foreach (var Currcmd in list)
        {
            var block = initCommandInScene(Currcmd);
            Snap(last_block, block.transform);
            if(firstBlock == null)
            {
                firstBlock = block;
                block.transform.SetParent(programRoot);
            }
            else
            {
                block.transform.SetParent(last_block);
                block.SetTopBlock(last_block.GetComponent<CodeEditorBlock>());
                last_block.GetComponent<CodeEditorBlock>().bottomBlock = block;
            }
            last_block = block.transform;
        }
        lastBlock = last_block.gameObject.GetComponent<CodeEditorBlock>() ;
    }
}
