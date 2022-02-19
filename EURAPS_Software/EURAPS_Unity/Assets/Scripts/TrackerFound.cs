using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackerFound : MonoBehaviour
{
    public GameObject scenario;
    public CodeEditor codeEditor;
    public GameObject Tracker;
    private bool scenarioSet = false;

    public void TrackerFoundCallback()
    {
        if (!scenarioSet)
        {
            scenarioSet = true;
            StartCoroutine(spawnScenarioOnTracker());
        }
    }
    private IEnumerator spawnScenarioOnTracker()
    {
        yield return new WaitForSeconds(2);
        var scenarioInstance = Instantiate(scenario);
        scenarioInstance.transform.position = Tracker.transform.position;
        scenarioInstance.transform.eulerAngles = new Vector3(0, Tracker.transform.eulerAngles.y, 0);
        codeEditor.currentScenario = scenarioInstance;
        codeEditor.simcom = scenarioInstance.GetComponentInChildren<SimulationCommunicator>();
    }
#if UNITY_EDITOR
    public void Start()
    {
        var scenarioInstance = Instantiate(scenario);
        codeEditor.currentScenario = scenarioInstance;
        codeEditor.simcom = scenarioInstance.GetComponentInChildren<SimulationCommunicator>();
        scenarioSet = true;
    }
#endif
}
