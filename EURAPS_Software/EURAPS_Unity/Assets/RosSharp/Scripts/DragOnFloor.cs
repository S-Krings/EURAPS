using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragOnFloor : MonoBehaviour
{
    public float floorHeight = 0.05f;
    public Material purpleMat;
    public Material transparentMat;
    public bool selecting = true;

    public List<object> inputParams;
    public List<object> outputParams;
    public RuntimeCommand outCmd;
    public Material waypointMaterial;

    private DrawLine line;
    public bool DrawLineMode = false;

    private void Start()
    {
        line = GetComponent<DrawLine>();
    }
    private void OnMouseDown()
    {
        if (DrawLineMode)
        {
            this.line.OnManipulationStarted();
        }

    }
    private void OnMouseDrag()
    {
        //Debug.Log("Onmousedrag, selecting:"+selecting);
        if (selecting)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Plane plane = new Plane(Vector3.up, new Vector3(0, floorHeight+0.025f, 0));
            float distance;
            if (plane.Raycast(ray, out distance))
            {
                Vector3 v = ray.GetPoint(distance);
                //v.Set(round(v).x, round(v).y, round(v).z);
                //v.Set(Mathf.Round(v.x * 10f) / 10f, Mathf.Round(v.y * 1000f) / 1000f, Mathf.Round(v.z * 10f) / 10f);
                transform.position = v;
            }
        }
    }

    private void OnMouseUp()
    {
        if (selecting)
        {
            selecting = false;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Plane plane = new Plane(Vector3.up, new Vector3(0, floorHeight+0.025f, 0));
            float distance;
            if (plane.Raycast(ray, out distance))
            {
                Vector3 v = ray.GetPoint(distance);
                //v = v - zero.transform.position;
                //v.Set(round(v).x, round(v).y, round(v).z);
                //v.Set(Mathf.Round(v.x * 10f) / 10f, Mathf.Round(v.y * 1000f) / 1000f, Mathf.Round(v.z * 10f) / 10f);
                //transform.position = v;
                if(outputParams != null)
                {
                    outputParams.Clear();
                }


                //Matrix4x4 worldToZero = zero.transform.worldToLocalMatrix;
                //v = this.position
                //Vector3 result = worldToZero * v;
                Vector3 result = CoordinateTransformationUtil.GetInstance().UnityWorldToScenario(this.transform.position);
                //transform.position = result;
                if (DrawLineMode)
                {
                    if (outputParams != null)
                    {
                        this.outputParams.Add(0f);
                        this.outputParams.Add(0f);
                    }
                    line.OnManipulationEnded();
                }
                else
                {
                    if (outputParams != null)
                    {
                        outputParams.Add((float)Math.Round(result.z, 2));
                        outputParams.Add((float)Math.Round(result.x, 2));
                    }

                    GameObject waypoint;
                    SpearedPosition Sposition;
                    if (outCmd.HelperElements == null)
                    {
                        waypoint = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                        waypoint.GetComponent<Collider>().enabled = false;
                        waypoint.GetComponent<MeshRenderer>().material = this.waypointMaterial;
                        waypoint.transform.localScale = new Vector3(0.04f, 0.04f, 0.04f);

                        Sposition = ScriptableObject.CreateInstance<SpearedPosition>();
                        Sposition.InUse = true;

                        outCmd.HelperElements = new List<object>();
                        outCmd.HelperElements.Add(waypoint);
                    }
                    else
                    {
                        waypoint = outCmd.HelperElements[0] as GameObject;
                        Sposition = outCmd.HelperElements[1] as SpearedPosition;
                    }

                    Sposition.SetPosition(this.transform.position);

                    waypoint.transform.position = v;

                    Debug.Log(result.z + " " + result.x);
                }
                
            }
            this.GetComponent<MeshRenderer>().material = transparentMat;
            //this.gameObject.SetActive(false);
        }
    }

    protected Vector3 round(Vector3 v)
    {
        float x = Mathf.Round(v.x * 100f) / 100f;
        float y = Mathf.Round(v.y * 1000f) / 1000f;
        float z = Mathf.Round(v.z * 100f) / 100f;
        GameObject zero = GameObject.Find("AbsoluteZero");
        if(zero != null)
        {
            Debug.Log("AbsoluteZero at: " + zero.transform.position);
            x = x + (zero.transform.position.x - Mathf.Round(zero.transform.position.x * 100f) / 100f);
            y = y + (zero.transform.position.y - Mathf.Round(zero.transform.position.y * 1000f) / 1000f);
            z = z + (zero.transform.position.z - Mathf.Round(zero.transform.position.z * 100f) / 100f);
        }
        else
        {
            Debug.Log("AbsoluteZero not found");
        }
        Vector3 vector = new Vector3(x,y,z);
        return vector;
    }

    public void SetMaterialPurple()
    {
        this.GetComponent<MeshRenderer>().material = purpleMat;
    }
}
