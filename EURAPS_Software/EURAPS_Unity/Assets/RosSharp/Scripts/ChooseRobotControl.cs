using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ChooseRobotControl : MonoBehaviour
{
    public void UnitySimulationScene()
    {
        //SceneManager.LoadScene("UnitySimulationScene");
    }

    public void NewRobotUnitySimulationScene()
    {
        /*      old code (with robot selection)
         *      SPEAREDSession.GetInstance().activeScenario = "NewRobotUnitySimulationScene";
        #if UNITY_ANDROID
                SceneManager.LoadScene("NXTBluetoothConnection");
        #elif ENABLE_WINMD_SUPPORT
                SceneManager.LoadScene("NewRobotUnitySimulationScene");
                //SceneManager.LoadScene("NXTBluetoothConnectionUWP");
        #elif UNITY_EDITOR
                SceneManager.LoadScene("NewRobotUnitySimulationScene");
        #endif
                //SceneManager.LoadScene("NewRobotUnitySimulationScene");
        */
        //SceneManager.LoadScene("NewRobotUnitySimulationScene");
    }

    public void GrabAndDropUnitySimulationScene()
    {
        /*      old code (with robot selection)

        //SceneManager.LoadScene("GrabAndDropUnitySimulationScene");
        SPEAREDSession.GetInstance().activeScenario = "NXTSimulationTemplate";
        #if UNITY_ANDROID
                SceneManager.LoadScene("NXTBluetoothConnection");
        #elif ENABLE_WINMD_SUPPORT
                SceneManager.LoadScene("NXTBluetoothConnectionUWP");
        #elif UNITY_EDITOR
                SceneManager.LoadScene("NXTSimulationTemplate");
        #endif*/
        NXTExecutor.GetInstance().ConnectToDefault();
        SceneManager.LoadScene("NXTSimulationTemplate");
    }
}
