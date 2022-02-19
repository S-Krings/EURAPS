using Microsoft.MixedReality.Toolkit.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "Command", menuName = "SPEARED/ArgumentModifier/MoveArgumentModifier", order = 1)]
public class MoveArgModifier : ArgModifier
{
    public GameObject targetSelectorPrefab;
    public GameObject targetSelectorPrefabHL;
    public Material waypointMaterial;

    public List<object> inputParams;
    public List<object> ouputParams;
    public RuntimeCommand outCmd;
    public bool drawMode = false;

    public float height;
    public GameObject selector;
    public override void ModArgs(RuntimeCommand inCmd, List<object> inargs, List<object> outargs, RuntimeCommand outCmd)
    {
        if (outCmd.HelperElements != null && outCmd.HelperElements.Count >= 2)
        {
            if (outCmd.HelperElements[0] is GameObject waypoint)
            {
#if UNITY_ANDROID
                this.selector.transform.position = waypoint.transform.position;
#elif UNITY_WSA
                this.selector.transform.GetChild(0).transform.position = waypoint.transform.position + new Vector3(0,0.3f,0);
#endif
            }
        }
        this.drawMode = false;
        inputParams = inargs;
        ouputParams = outargs;
        this.outCmd = outCmd;
        GameObject groundPlane = null;
        if (GameObject.Find("Plane") != null)
        {
            height = GameObject.Find("Plane").transform.position.y;
            groundPlane = GameObject.Find("Plane");
        }
        else
        {
            height = 0;
        }

        if (selector == null)
        {
#if UNITY_ANDROID
            selector = Instantiate(targetSelectorPrefab, Camera.main.transform.position + Camera.main.transform.forward * 0.5f, Quaternion.identity);
            selector.GetComponent<DragOnFloor>().floorHeight = height;
            selector.GetComponent<DragOnFloor>().inputParams = inputParams;
            selector.GetComponent<DragOnFloor>().outputParams = ouputParams;
            selector.GetComponent<DragOnFloor>().outCmd = this.outCmd;
            selector.GetComponent<DragOnFloor>().waypointMaterial = waypointMaterial;
            selector.GetComponent<DragOnFloor>().DrawLineMode = false;
#elif UNITY_WSA
        selector = Instantiate(targetSelectorPrefabHL, Camera.main.transform.position + Camera.main.transform.forward * 0.5f, Quaternion.identity);
        var snaper = selector.GetComponentInChildren<TargetSpherePlaneSnap>();
        if(groundPlane != null)
        {
                snaper.planePoint = groundPlane.transform.position;
                snaper.normalVec = groundPlane.transform.up;
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
        // TODO: same for android
        var manHandle = selector.GetComponentInChildren<ObjectManipulator>();
        if (manHandle != null)
        {
            manHandle.OnManipulationEnded.RemoveAllListeners();
            manHandle.OnManipulationStarted.RemoveAllListeners();
            manHandle.OnManipulationEnded.AddListener((ManipulationEventData data) => OnRelease());
            var drawLinel = selector.GetComponent<DrawLine>();
            if (drawLinel != null)
            {
                    drawLinel.drawMode = false;
                    manHandle.OnManipulationStarted.AddListener((ManipulationEventData data) => drawLinel.OnManipulationStarted());
                    manHandle.OnManipulationEnded.AddListener((ManipulationEventData data) => drawLinel.OnManipulationEnded());
            }

        }
        
#endif
        }
        else
        {
#if UNITY_ANDROID
            selector.GetComponent<DragOnFloor>().selecting = true;
            selector.GetComponent<DragOnFloor>().floorHeight = height;
            selector.GetComponent<DragOnFloor>().inputParams = inputParams;
            selector.GetComponent<DragOnFloor>().outputParams = ouputParams;
            selector.GetComponent<DragOnFloor>().outCmd = this.outCmd;
            selector.GetComponent<DragOnFloor>().waypointMaterial = waypointMaterial;
            selector.GetComponent<DragOnFloor>().DrawLineMode = false;
#endif
            //selector.SetActive(true);
            ///Drag on floor is not im prefab anymore
            /*selector.GetComponentInChildren<DragOnFloor>().SetMaterialPurple();
            selector.GetComponentInChildren<DragOnFloor>().selecting = true;*/
        }
        var drawLine = selector.GetComponent<DrawLine>();
        drawLine.drawMode = false;
    }

    public void OnRelease()
    {
        if (!drawMode)
        {
            Vector3 officialPosition = new Vector3(0, 0, 0);
            if (this.selector != null)
            {
                officialPosition = selector.GetComponentInChildren<GlobalCoordinateSetter>().getOfficialPosition();


                //Old approach using AbsoluteZero to get relative coordinates
                /*Vector3 unityPosition = this.selector.transform.position;
                GameObject zero = GameObject.Find("AbsoluteZero");
                if (zero != null)
                {
                    GameObject go = new GameObject("GO DragOnFloorReciever");
                    go.transform.position = unityPosition;
                    go.transform.parent = zero.transform;
                    Vector3 tmpPos = go.transform.localPosition;
                    officialPosition = new Vector3(tmpPos.z, tmpPos.y, tmpPos.x);
                    Destroy(go);
                }
                else
                {
                    Debug.Log("AbsoluteZero not found");
                }*/
            }

            if (this.ouputParams != null)
            {
                if (this.selector != null)// && selector.GetComponentInChildren<DragOnFloor>().selecting)
                {
                    ouputParams.Clear();
                    ouputParams.Add((float)(Mathf.Round(officialPosition.x * 100) / 100));
                    ouputParams.Add((float)(Mathf.Round(officialPosition.z * 100) / 100));

                    GameObject waypoint;
                    SpearedPosition Sposition;
                    if (outCmd.HelperElements == null)
                    {
                        waypoint = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                        waypoint.GetComponent<Collider>().enabled = false;
                        waypoint.GetComponent<MeshRenderer>().material = this.waypointMaterial;
                        waypoint.transform.localScale = new Vector3(0.09f, 0.09f, 0.09f);

                        Sposition = ScriptableObject.CreateInstance<SpearedPosition>();
                        GameObject.Find("PathManager").GetComponent<SpearedPathRenderer>().AddPosition(Sposition);
                        outCmd.HelperElements = new List<object>();
                        outCmd.HelperElements.Add(waypoint);
                        outCmd.HelperElements.Add(Sposition);
                    }
                    else
                    {
                        waypoint = outCmd.HelperElements[0] as GameObject;
                        Sposition = outCmd.HelperElements[1] as SpearedPosition;
                    }
                    var pointOnPlane = this.selector.transform.GetChild(1).position;
                    waypoint.transform.position = pointOnPlane;
                    if (ProgramStore.GetInstance().hasCommand(outCmd))
                    {
                        Sposition.InUse = true;
                    }
                    Sposition.SetPosition(pointOnPlane);
                }
            }
            //selector.GetComponentInChildren<DragOnFloor>().selecting = false;
        }
    }

    private void OnDestroy()
    {
        Destroy(selector);
    }

    public static Vector3 StringToVector3(string sVector)
    {
        // Remove the parentheses
        if (sVector.StartsWith("(") && sVector.EndsWith(")"))
        {
            sVector = sVector.Substring(1, sVector.Length - 2);
        }

        // split the items
        string[] sArray = sVector.Split(',');

        // store as a Vector3
        Vector3 result = new Vector3(
            float.Parse(sArray[0]),
            float.Parse(sArray[1]),
            float.Parse(sArray[2]));

        return result;
    }
}
