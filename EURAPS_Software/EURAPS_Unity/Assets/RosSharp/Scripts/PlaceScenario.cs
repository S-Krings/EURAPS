using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class PlaceScenario : MonoBehaviour
{
    public string targetObject = "ScenarioPlacer";
    public GameObject scenario;
    public bool vuforiaTracker = false;
    private bool placed = false;
    void Update()
    {
        if (!placed && GameObject.Find(targetObject) != null )
        {
            //Debug.Log("Vuforia: " + vuforiaTracker + " positions equal: " + (GameObject.Find(targetObject).transform.position.x != 0f));
            if (!vuforiaTracker || (vuforiaTracker && GameObject.Find(targetObject).transform.position.x != 0f))
            {
                GameObject placer = GameObject.Find(targetObject);
                Debug.Log("Position is " + scenario.transform.position+" goal is: "+ placer.transform.position);
#if ENABLE_WINMD_SUPPORT
                scenario.transform.position = placer.transform.position + new Vector3(0,0.5f,0);
#else
                scenario.transform.position = placer.transform.position;
#endif
                scenario.transform.rotation = placer.transform.rotation;
                Debug.Log("Position is " + scenario.transform.position);


                scenario.SetActive(true);

                Transform plane = scenario.transform.Find("Plane_");
                if (plane == null)
                {
                    Debug.Log("Plane_ is null");
                    GameObject planeObject = GameObject.Find("Plane");
                    Debug.Log("Plane is null: " + (plane == null));
                    plane = planeObject.transform;
                }
                Debug.Log("Plane:" + plane);
                plane.eulerAngles = new Vector3(0, plane.eulerAngles.y, 0);
                //scenario.SetActive(true);
                placed = true;
            }
        }
    }
}
