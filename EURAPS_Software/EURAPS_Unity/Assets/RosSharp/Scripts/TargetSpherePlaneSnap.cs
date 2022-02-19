using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetSpherePlaneSnap : MonoBehaviour
{
    public Vector3 normalVec;
    public Vector3 planePoint;
    public bool PlaneSet = false;
    private int layerMask;
    public Transform SphereTransform;
    public LineRenderer lineRenderer;
    // Start is called before the first frame update
    void Start()
    {
        layerMask = LayerMask.NameToLayer("Ground");
        lineRenderer = new GameObject("Line").AddComponent<LineRenderer>();
        lineRenderer.startColor = Color.black;
        lineRenderer.endColor = Color.black;
        lineRenderer.startWidth = 0.01f;
        lineRenderer.endWidth = 0.01f;
        lineRenderer.positionCount = 2;
        lineRenderer.useWorldSpace = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (PlaneSet)
        {
            var groundPlane = new Plane(normalVec, planePoint);
            this.transform.position = groundPlane.ClosestPointOnPlane(SphereTransform.position);

            //For drawing line in the world space, provide the x,y,z values
            lineRenderer.SetPosition(0, this.transform.position); //x,y and z position of the starting point of the line
            lineRenderer.SetPosition(1, SphereTransform.position); //x,y and z position of the end point of the line
        }     
    }
}