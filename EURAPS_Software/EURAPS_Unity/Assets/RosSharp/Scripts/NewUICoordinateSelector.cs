using UnityEngine;

public abstract  class NewUICoordinateSelector : MonoBehaviour
{

    public abstract GameObject createCoordinatesSelector(RuntimeCommand moveStatement);
    public abstract Vector4 getCoordinateValues(GameObject coordinateSelector);
    public abstract string nameIdentifier();
}