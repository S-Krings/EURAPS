using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoordinateTransformationUtil : MonoBehaviour
{
    private static CoordinateTransformationUtil instance = null;
    private GameObject zero;
    private int counter = 0;

    public static CoordinateTransformationUtil GetInstance()
    {
        if(instance != null)
        {
            return instance;
        }
        else
        {
            instance = new GameObject().AddComponent<CoordinateTransformationUtil>();

            return instance;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        zero = GameObject.Find("AbsoluteZero");
    }

    private void Update()
    {
        counter++;
        if(counter >= 60)
        {
            zero = GameObject.Find("AbsoluteZero");
            counter = 0;
        }
    }

    public Vector3 ScenarioToUnityWorld(Vector3 scenarioCoordinates)
    {
        if (zero == null)
        {
            zero = GameObject.Find("AbsoluteZero");
        }
        if (zero != null)
        {
            Vector3 result = zero.transform.TransformPoint(scenarioCoordinates);
            return result;
        }
        else
        {
            Debug.Log("No AbsoluteZero found by CoordinateTransformationUtil.");
        }
        return new Vector3(0, 0, 0); //Error state
    }
    public Vector3 UnityWorldToScenario(Vector3 unityCoordinates)
    {
        if (zero == null)
        {
            zero = GameObject.Find("AbsoluteZero");
        }
        if (zero != null)
        {
            Vector3 result = zero.transform.InverseTransformPoint(unityCoordinates);
            return result;
        }
        else
        {
            Debug.Log("No AbsoluteZero found by CoordinateTransformationUtil.");
        }
        return new Vector3(0,0,0); //Error state
    }

    public Vector3 GetX0Z(Vector3 vector)
    {
        return new Vector3(vector.x, 0, vector.z);
    }
    public Vector3 GetZ0X(Vector3 vector)
    {
        return new Vector3(vector.z, 0, vector.x);
    }
}

