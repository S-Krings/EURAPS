using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ResetScenario : MonoBehaviour
{
    public GameObject scenarioPrefab;
    public GameObject currentScenario;
    public CodetoCanvasGenNew code2canvas;
    // Start is called before the first frame update
    void Start()
    {
        var btn = GetComponent<Button>();
        if(btn != null)
        {
            btn.onClick.AddListener(() => {
                onCLickEventListerner();
            });
        }
    }
    public void onCLickEventListerner()
    {
        var scenario = Instantiate(scenarioPrefab);
        GameObject placer = currentScenario;
        scenario.transform.position = placer.transform.position;
        scenario.transform.rotation = placer.transform.rotation;
        Transform plane = scenario.transform.Find("Plane_");
        if (plane == null)
        {
            plane = scenario.transform.Find("Plane");
        }
        plane.eulerAngles = new Vector3(0, plane.eulerAngles.y, 0);
        scenario.SetActive(true);
        Destroy(currentScenario);
        this.currentScenario = scenario;
        code2canvas.movementController = scenario.GetComponentInChildren<MovementController>();
        code2canvas.simulationCommunicator = scenario.GetComponentInChildren<SimulationCommunicator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
