using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GlobalCoordinateSetter : MonoBehaviour
{ 
    public TextMesh textField;
    public Vector3 officialPosition;
    private int counter = 0;

    /* On each update write the coordinates of the object this is attached to into the given text field. Coordinates are written as robot coordinates, not Unity coordinates. */
    void Update()
    {
        if(counter > 30)
        {
            officialPosition = this.transform.position;
            Vector3 unityPosition = this.transform.position;
            CoordinateTransformationUtil util = CoordinateTransformationUtil.GetInstance();
            officialPosition = util.GetZ0X(util.UnityWorldToScenario(this.transform.position));
            //Debug.Log("Position: " + this.transform.position + " to scenario: " + util.UnityWorldToScenario(this.transform.position) + " switch: " + util.GetZ0X(util.UnityWorldToScenario(this.transform.position)));
            textField.text = String.Format("({0}, {1}, {2})", Math.Round(officialPosition.x, 2), Math.Round(officialPosition.y, 2), Math.Round(officialPosition.z, 2));
            counter = 0;
        }
        counter++;
    }

    public Vector3 getOfficialPosition()
    {
        return officialPosition;
    }
}
