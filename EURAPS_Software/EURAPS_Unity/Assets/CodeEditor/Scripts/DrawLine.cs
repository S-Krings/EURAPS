using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawLine : MonoBehaviour
{
    public Transform PointOnPlane;
    public SPEAREDCommand inCmd;
    public bool shouldRemoveLast = true;
    private LineRenderer LineObj;
    private bool manipulating;
    public List<Vector3> pointsOnLine = new List<Vector3>();
    private int counter = 0;
    public bool drawMode = false;

    private GameObject AbsoluteZero;
    public Material waypointMaterial2;

    private void Start()
    {
        AbsoluteZero = GameObject.Find("AbsoluteZero");
    }


    public void OnManipulationStarted()
     {
        if (drawMode)
        {
            counter = 0;
            if (LineObj == null)
            {
                var Line = new GameObject();
                LineObj = Line.AddComponent<LineRenderer>();
                LineObj.positionCount = 100;
                LineObj.startWidth = 0.01f;
                LineObj.endWidth = 0.01f;
            }
            pointsOnLine = new List<Vector3>();
            manipulating = true;
        }
    }
    public void OnManipulationEnded()
    {
        if (drawMode)
        {
            manipulating = false;
            LineObj.transform.position = this.pointsOnLine[this.pointsOnLine.Count - 1];
            LineObj.Simplify(0.1f);
            Vector3[] positions = new Vector3[LineObj.positionCount];
            LineObj.GetPositions(positions);
            if (AbsoluteZero == null)
            {
                AbsoluteZero = GameObject.Find("AbsoluteZero");
            }
            if (AbsoluteZero != null)
            {
                foreach (var point in positions)
                {
                    var officialPoint = AbsoluteZero.transform.InverseTransformPoint(point);
                    var cmd = ScriptableObject.CreateInstance<RuntimeCommand>();
                    cmd.command = inCmd;
                    cmd.arguments = new List<object>()
                {
                    Mathf.Round(officialPoint.z*100)/100,
                    Mathf.Round(officialPoint.x*100)/100
                };
                    ProgramStore.GetInstance().AddCommand(cmd);

                    GameObject waypoint;
                    SpearedPosition Sposition;
                    if (cmd.HelperElements == null)
                    {
                        waypoint = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                        waypoint.GetComponent<Collider>().enabled = false;
                        waypoint.GetComponent<MeshRenderer>().material = this.waypointMaterial2;
                        waypoint.transform.localScale = new Vector3(0.04f, 0.04f, 0.04f);

                        Sposition = ScriptableObject.CreateInstance<SpearedPosition>();
                        Sposition.InUse = true;
                        GameObject.Find("PathManager").GetComponent<SpearedPathRenderer>().AddPosition(Sposition);
                        cmd.HelperElements = new List<object>();
                        cmd.HelperElements.Add(waypoint);
                        cmd.HelperElements.Add(Sposition);
                    }
                    else
                    {
                        waypoint = cmd.HelperElements[0] as GameObject;
                        Sposition = cmd.HelperElements[1] as SpearedPosition;
                    }
                    Sposition.SetPosition(point);

                    waypoint.transform.position = point;
                    Vector3[] poss = new Vector3[LineObj.positionCount];
                    LineObj.GetPositions(poss);
                    pointsOnLine = new List<Vector3>(poss);
                }
            }
            CodeEditor.MainInstance.SyncFromProgramStore();
            Destroy(LineObj.gameObject);
        }       
    }

    // Update is called once per frame
    void Update()
    {
        if (manipulating)
        {
            if(counter == 0)
            {
                Vector3 officialPosition = new Vector3(0, 0, 0);
                if (this.PointOnPlane != null)
                {
                    officialPosition = PointOnPlane.GetComponentInChildren<GlobalCoordinateSetter>().getOfficialPosition();
                }
                pointsOnLine.Add(PointOnPlane.position);
                LineObj.positionCount = pointsOnLine.Count;
                LineObj.SetPositions(pointsOnLine.ToArray());
            }
            counter++;
            if(counter > 30)
            {
                counter = 0;
            }
        }
    }
}
