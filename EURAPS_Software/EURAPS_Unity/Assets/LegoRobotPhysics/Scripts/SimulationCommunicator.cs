using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimulationCommunicator : MonoBehaviour
{
    public MovementController movementController;
    ProgramStore programStore;
    public bool debugRun = false;

    public void Start()
    {
        programStore = ProgramStore.GetInstance();
        if(programStore != null)
        {
            programStore.registerForUpdates(onProgramUpdate);
        }
        if (debugRun)
        {
            DebugRun();
        }
    }

    public void DebugRun()
    {
        List<MovementController.states> stateList = new List<MovementController.states> { MovementController.states.RELEASE, MovementController.states.BRUTEMOVE, MovementController.states.GRAB, MovementController.states.BRUTEMOVE, MovementController.states.GRAB, MovementController.states.BRUTEMOVE, MovementController.states.RELEASE};
        List<Vector3> values = new List<Vector3> { new Vector3(0, 0, 0), fixRotation(new Vector3(0f, 0, 0.6f)), new Vector3(0, 0, 0), fixRotation(new Vector3(0.4f, 0, 0.9f)), new Vector3(0, 0, 0), fixRotation(new Vector3(0, 0, 0.6f)), new Vector3(0, 0, 0) };
        this.movementController.simulateProgram(stateList, values);
    }

    private void onProgramUpdate()
    {

    }

    public void simulate()
    {
        Debug.Log("Simulation Communicator: simulate called");
        List<RuntimeCommand> commands = programStore.getCommands();
        //////FOR TESTING//////////////////////////////////////////////////////
        //List<RuntimeCommand> commands = FindObjectOfType<RuntimeCommandFactory>().createSampleProgram();
        ////////////////////////////////////////////////////////////////////////////////////

        List <MovementController.states> stateList = new List<MovementController.states>();
        List<Vector3> values = new List<Vector3>();

        foreach (RuntimeCommand command in commands)
        {
            if (command.command.isLegoCommand)
            {
                if (command.command.Lcommand == LegoCommand.MOVE)
                {
                    stateList.Add(MovementController.states.BRUTEMOVE); //because the legoCommand MOVE command is the simulation command BRUTEMOVE
                    Debug.Log(command.arguments[0] + " Type " + command.arguments[0].GetType());
                    Vector2 goal = new Vector2((float)command.arguments[0], (float)command.arguments[1]);
                    values.Add(fixRotation(new Vector3(goal.y, 0, goal.x)));
                }
                else if (command.command.Lcommand == LegoCommand.CLAW_UP)
                {
                    stateList.Add(MovementController.states.RELEASE);
                    values.Add(new Vector3(0, 0, 0));
                }
                else if (command.command.Lcommand == LegoCommand.CLAW_DOWN)
                {
                    stateList.Add(MovementController.states.GRAB);
                    values.Add(new Vector3(0, 0, 0));
                }
            }
        }
        this.movementController.simulateProgram(stateList, values);
    }

    /* Transforms rotation from robot coordinates back to unity coordinates (those are needed for the simulation)*/
    public Vector3 fixRotation(Vector3 vector)
    {
        CoordinateTransformationUtil util = CoordinateTransformationUtil.GetInstance();
        return util.GetX0Z(util.ScenarioToUnityWorld(vector));
    }
}