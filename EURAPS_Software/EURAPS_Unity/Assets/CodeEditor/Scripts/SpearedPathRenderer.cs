using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpearedPathRenderer : MonoBehaviour
{
    [SerializeField]
    private List<SpearedPosition> positions = new List<SpearedPosition>();
    private LineRenderer renderer;
    private Transform absoluteZero;

    public void Start()
    {
        renderer = GetComponent<LineRenderer>();
        renderer.positionCount = 100;
        renderer.startWidth = 0.01f;
        renderer.endWidth = 0.01f;
    }
    public void AddPosition(SpearedPosition pos)
    {
        positions.Add(pos);
        pos.AddListener(this.UpdatePath);
    }
    public void UpdatePath(SpearedPosition pos)
    {
        //positions.Sort();
        renderer.positionCount = 0;
        List<Vector3> poss = new List<Vector3>();
        CoordinateTransformationUtil util = CoordinateTransformationUtil.GetInstance();
        var first = true;
        List<RuntimeCommand> commands = ProgramStore.GetInstance().getCommands();
        foreach (RuntimeCommand command in commands)
        {
            if (command.command.isLegoCommand && command.command.Lcommand == LegoCommand.MOVE)
            {
                if (first)
                {
                    first = false;
                    if (this.absoluteZero == null)
                    {
                        this.absoluteZero = GameObject.Find("AbsoluteZero")?.transform;
                    }
                    if (this.absoluteZero != null)
                    {
                        Vector3 commandpos = util.ScenarioToUnityWorld(new Vector3((float)command.arguments[1], 0,(float)command.arguments[0]));
                        Debug.Log("Command: " + commandpos + " AbsZero: " + util.ScenarioToUnityWorld(new Vector3(this.absoluteZero.position.x, 0, this.absoluteZero.position.z)));
                        if (commandpos != util.ScenarioToUnityWorld(new Vector3(this.absoluteZero.position.x, 0, this.absoluteZero.position.z)))
                        {
                            poss.Add(this.absoluteZero.position);
                        }
                    }

                }
                //if (thispos.InUse)
                //{
                    poss.Add(util.ScenarioToUnityWorld(new Vector3((float)command.arguments[1], 0, (float)command.arguments[0])));
                //}
            }
        }
        renderer.positionCount = poss.Count;
        renderer.SetPositions(poss.ToArray());
        /*positions.Sort();
        renderer.positionCount = 0;
        List<Vector3> poss = new List<Vector3>();
        var first = true;
        foreach(var thispos in positions)
        {
            if (first)
            {
                first = false;
                if (this.absoluteZero == null)
                {
                    this.absoluteZero = GameObject.Find("AbsoluteZero")?.transform;
                }
                if (this.absoluteZero != null)
                {
                    if (thispos.GetPosition() != this.absoluteZero.position)
                    {
                        poss.Add(this.absoluteZero.position);
                    }
                }

            }
            if (thispos.InUse)
            {
                poss.Add(thispos.GetPosition());
            }
        }
        renderer.positionCount = poss.Count;
        renderer.SetPositions(poss.ToArray());
        */
    }
}
