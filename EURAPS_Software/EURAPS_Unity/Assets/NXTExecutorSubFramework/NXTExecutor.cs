#if ENABLE_WINMD_SUPPORT
using Microsoft.MixedReality.Toolkit.UI;
#endif
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NXTExecutor : MonoBehaviour
{
    private static NXTExecutor Instance;
    private NXTBluetoothSocket socket;
    public GameObject BtnPrefab;
    public GameObject content;
    public TMP_Text debugtext;
    private float conversionMultiplier = 100;
    private float robotResetX = 0;
    private float robotResetY = 0;
    // Start is called before the first frame update
    void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
#if ENABLE_WINMD_SUPPORT
            socket = new NXTBluetoothSocktUWP();
            (socket as NXTBluetoothSocktUWP).debugText = debugtext;
            (socket as NXTBluetoothSocktUWP).findDevices(() => { }, debugtext);
#elif UNITY_ANDROID || UNITY_EDITOR
            socket = new NXTBluetoothSocktAndroid();
            (socket as NXTBluetoothSocktAndroid).findDevices(() => { }, debugtext);
#endif
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private void drawDeviceSelection()
    {
        Debug.Log("devices Loaded");
#if UNITY_ANDROID || UNITY_EDITOR
        var yCord = 200;
#elif ENABLE_WINMD_SUPPORT
        var yCord = 0.4f;
#endif
        List<string> dnames = new List<string>();
        dnames = socket.getDeviceNames();
        foreach (string name in dnames)
        {
#if ENABLE_WINMD_SUPPORT
            var element = Instantiate(BtnPrefab, content.transform);
            var interactable = element.GetComponent<Interactable>();

            interactable.OnClick.AddListener(() => {
                socket.openConnectionTo(name);
                var scenario = SPEAREDSession.GetInstance().activeScenario;
                SPEAREDSession.GetInstance().activeScenario = "";
                SceneManager.LoadScene(scenario);
            }
            );
            element.transform.localPosition = new Vector3(0, yCord, -0.5f);
            element.transform.localScale = new Vector3(3, 4, 3);
            foreach (var tmp in element.GetComponentsInChildren<TextMeshPro>())
            {
                tmp.SetText(name);
            }
            yCord = yCord - 0.15f;
            element.transform.localScale = new Vector3(1, 1, 1);
#elif UNITY_ANDROID || UNITY_EDITOR
            var element = Instantiate(BtnPrefab, content.transform);
            var interactable = element.GetComponent<Button>();
            var text = interactable?.gameObject?.transform?.GetChild(0)?.GetComponent<Text>();
            if(text != null)
            {
                text.text = name;
            }

            interactable.onClick.AddListener(() => {
#if !UNITY_EDITOR
                socket.openConnectionTo(name);
#endif
                var scenario = SPEAREDSession.GetInstance().activeScenario;
                SPEAREDSession.GetInstance().activeScenario = "";
                SceneManager.LoadScene(scenario);
            }
            );
            element.transform.localPosition = new Vector3(0, yCord, -0.5f);
            foreach (var tmp in element.GetComponentsInChildren<TextMeshPro>())
            {
                tmp.SetText(name);
            }
            yCord = yCord - 40;
#endif
            }
        
    }
    public void skipConnectToRobot()
    {
        var scenario = "";
        if (SPEAREDSession.GetInstance() != null)
        {
            scenario = SPEAREDSession.GetInstance().activeScenario;
            SPEAREDSession.GetInstance().activeScenario = "";
        }
        else
        {
            // Just the default :)
            scenario = "NXTSimulationTemplate";
        }
        
        SceneManager.LoadScene(scenario);
    }

    public void send()
    {
        if (ProgramStore.GetInstance() != null)
        {
            var commands = ProgramStore.GetInstance().getCommands();
            List<byte> bytes = new List<byte>();
            foreach(var command in commands)
            {
                if (command.command.isLegoCommand)
                {
                    float commandID = (float)command.command.Lcommand;
                    bytes = bytes.Concat(BitConverter.GetBytes((commandID)).Reverse()).ToList();
                    if (command.command.Signature != null)
                    {
                        var singleSignature = command.command.Signature as SPEAREDCommandSignature1;
                        if(singleSignature != null)
                        {
                            switch (singleSignature.argumentType)
                            {
                                case SPEAREDCommandSignature1.ArgumentType.Vector2d:
                                    // TODO remove quickfix maybe (*conversionMultiplier)
                                    bytes = bytes.Concat(BitConverter.GetBytes(((float)command.arguments[0]) * conversionMultiplier).Reverse()).ToList();
                                    bytes = bytes.Concat(BitConverter.GetBytes(((float)command.arguments[1]) * conversionMultiplier).Reverse()).ToList();
                                    break;
                                default:
                                    break;
                            }
                        }
                    }
                }
            }
            this.socket.write(bytes.ToArray());
        }
    }
    public void sendReset()
    {
        List<byte> bytes = new List<byte>();
        bytes = bytes.Concat(BitConverter.GetBytes(((float)LegoCommand.RESET)).Reverse()).ToList();
        this.socket.write(bytes.ToArray());
    }
    public void ConnectToDefault()
    {
        socket.openConnectionToDefault();
    }
    public static NXTExecutor GetInstance()
    {
        return Instance;
    }
    public void OnDestroy()
    {
        socket.close();
    }
    void OnApplicationQuit()
    {
        
    }
}
