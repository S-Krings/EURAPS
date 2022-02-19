using System;
using UnityEngine.Events;
using System.Linq;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NXTBluetoothSocktAndroid : NXTBluetoothSocket
{
    private List<string> names;
    const string PluginName = "de.upb.sicp.bluetoothbridge.BluetoothBridge";
    private AndroidJavaObject pluginInstance;
    public async void findDevices(Action a, TMP_Text debug)
    {
#if UNITY_ANDROID || UNITY_EDITOR
        /*
         * Search is not implemented by now
         */
        names = new List<string>();
        names.Add("NXT");
        a.Invoke();
#endif
    }
    public override byte[] read()
    {
#if UNITY_ANDROID

#endif
        return new byte[0];
    }

    public override void write(byte[] bytes)
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        using (AndroidJavaClass pluginClass = new AndroidJavaClass(PluginName))
        {
            if (pluginClass != null)
            {
                using (pluginInstance = pluginClass.CallStatic<AndroidJavaObject>("getInstance"))
                {
                    Debug.Log("Plugin:" + pluginInstance.ToString());
                    pluginInstance.SetStatic("bytes", bytes);
                    pluginInstance.Call("write");
                }
            }
        }    
#endif
    }
    public override List<string> getDeviceNames()
    {
        return this.names.Select(x => x).ToList();
    }

    public override void openConnectionTo(string deviceName)
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        using (AndroidJavaClass pluginClass = new AndroidJavaClass(PluginName))
        {
            if (pluginClass != null)
            {
                using (pluginInstance = pluginClass.CallStatic<AndroidJavaObject>("getInstance"))
                {
                    Debug.Log("Plugin:" + pluginInstance.ToString());
                    pluginInstance.SetStatic("deviceName", deviceName);
                    Debug.Log(pluginInstance.Call<string>("connect"));
                }
            }
        }
#endif
    }

    public override void close()
    {
#if UNITY_ANDROID
        using (AndroidJavaClass pluginClass = new AndroidJavaClass(PluginName))
        {
            if (pluginClass != null)
            {
                using (pluginInstance = pluginClass.CallStatic<AndroidJavaObject>("getInstance"))
                {
                    Debug.Log("Plugin:" + pluginInstance.ToString());
                    Debug.Log(pluginInstance.Call<string>("close"));
                }
            }
        }
#endif
    }
}
