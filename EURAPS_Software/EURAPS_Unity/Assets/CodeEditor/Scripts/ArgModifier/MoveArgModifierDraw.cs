using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
[CreateAssetMenu(fileName = "Command", menuName = "SPEARED/ArgumentModifier/MoveArgumentModifierDraw", order = 1)]
public class MoveArgModifierDraw : ArgModifier
{
    public GameObject targetSelectorPrefab;
    public GameObject targetSelectorPrefabHL;
    public Material waypointMaterial;

    private List<object> inputParams;
    private List<object> ouputParams;
    private RuntimeCommand outCmd;

    float height;
    private GameObject selector;
    public override void ModArgs(RuntimeCommand inCmd, List<object> inargs, List<object> outargs, RuntimeCommand outCmd)
    {
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
            selector.transform.position = groundPlane.transform.position;
            var dragOnFloor = selector.GetComponent<DragOnFloor>();
            dragOnFloor.floorHeight = height;
            dragOnFloor.inputParams = inputParams;
            dragOnFloor.outputParams = ouputParams;
            dragOnFloor.outCmd = this.outCmd;
            dragOnFloor.waypointMaterial = waypointMaterial;
            dragOnFloor.DrawLineMode = true;
            var drawLine = selector.GetComponent<DrawLine>();
            drawLine.inCmd = inCmd;
#elif UNITY_WSA
            selector = Instantiate(targetSelectorPrefabHL, Camera.main.transform.position + Camera.main.transform.forward * 0.5f, Quaternion.identity);
            selector.transform.position = groundPlane.transform.position + new Vector3(0, 0.5f, 0);
            var snaper = selector.GetComponentInChildren<TargetSpherePlaneSnap>();
            if (groundPlane != null)
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
            var drawLine = selector.GetComponent<DrawLine>();
            drawLine.inCmd = inCmd.command;
            drawLine.waypointMaterial2 = waypointMaterial;
            ///Drag on flor is not in prefab anymore
            /*selector.GetComponentInChildren<DragOnFloor>().selecting = true;
            Debug.Log(selector.name);*/
/*
            var manHandle = selector.GetComponentInChildren<ObjectManipulator>();
            if (manHandle != null)
            {
#if UNITY_WSA
                manHandle.OnManipulationStarted.RemoveAllListeners();
                manHandle.OnManipulationEnded.RemoveAllListeners();
                manHandle.OnManipulationStarted.AddListener((ManipulationEventData evt) => drawLine.OnManipulationStarted());
                manHandle.OnManipulationEnded.AddListener((ManipulationEventData evt) => {
                    this.ouputParams.Clear();
                    this.ouputParams.Add(0f);
                    this.ouputParams.Add(0f);
                    drawLine.OnManipulationEnded();
               });
#endif
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
#endif
            var drawLine = selector.GetComponent<DrawLine>();
            drawLine.inCmd = inCmd.command;

            //selector.SetActive(true);
            ///Drag on floor is not im prefab anymore
            /*selector.GetComponentInChildren<DragOnFloor>().SetMaterialPurple();
            selector.GetComponentInChildren<DragOnFloor>().selecting = true;*/ /*
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
*/
