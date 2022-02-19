using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PluginTest : MonoBehaviour
{
    const string PluginName = "de.upb.sicp.bluetoothbridge.BluetoothBridge";
    static AndroidJavaObject pluginInstance;
    // Start is called before the first frame update
    void Start()
    {

        using (AndroidJavaClass pluginClass = new AndroidJavaClass(PluginName))
        {
            if (pluginClass != null)
            {
                using (pluginInstance = pluginClass.CallStatic<AndroidJavaObject>("getInstance"))
                {
                    Debug.Log("Plugin:" + pluginInstance.ToString());
                    pluginInstance.SetStatic("deviceName", "NXT");
                    Debug.Log(pluginInstance.Call<string>("connect"));
                    byte[] bytes= new byte[3 * 4];
                    Debug.Log("Call Write");
                    pluginInstance.SetStatic("bytes", bytes);
                    pluginInstance.Call("write");
                    Debug.Log("Called Write");
                }
            }
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
