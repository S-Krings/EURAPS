using Microsoft.MixedReality.Toolkit.UI;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

#if ENABLE_WINMD_SUPPORT
using System.IO;
using Windows.Devices.Enumeration;
using Windows.Devices.Bluetooth;
using Windows.Devices.Bluetooth.Rfcomm;
using Windows.Networking.Sockets;
using Windows.Storage.Streams;
#endif

public class UWP_BT_NXT : MonoBehaviour
{
    public List<string> names;
    public GameObject BtnPrefab;
    public GameObject content;
    public TMP_Text debugtext;


#if ENABLE_WINMD_SUPPORT
    public Dictionary<string, DeviceInformation> name2device = new Dictionary<string, DeviceInformation>();
    public Dictionary<string, BluetoothDevice> name2bluetooth = new Dictionary<string, BluetoothDevice>();
    public Stream inputStream;
    public Stream outputStream;
    private StreamSocket socket;
    private static bool stingBack = true;
    private int mPower = 80;

    private double amod;
    private double lmod;
    private double rmod;
    private bool mRegulateSpeed;
    private bool mSynchronizeMotors;
#endif

    private void Start()
    {
        findDevices();
        DontDestroyOnLoad(gameObject);
    }
    public async void findDevices()
    {
#if ENABLE_WINMD_SUPPORT
        names = new List<string>();
        var informations = await DeviceInformation.FindAllAsync(BluetoothDevice.GetDeviceSelectorFromPairingState(true));
        foreach (var info in informations)
        {
            names.Add(info.Name.ToLower());
            name2device.Add(info.Name.ToLower(),info);
            name2bluetooth.Add(info.Name.ToLower(), await BluetoothDevice.FromIdAsync(info.Id));
        }
#endif
        var yCord = 0.4f;
        foreach (string name in names)
        {
            var element = Instantiate(BtnPrefab, content.transform);
            var interactable = element.GetComponent<Interactable>();
#if ENABLE_WINMD_SUPPORT
            interactable.OnClick.AddListener(SelectBluetoothDevice(name));
#endif
            element.transform.localPosition = new Vector3(0, yCord, -0.5f);
            element.transform.localScale = new Vector3(3,4,3);
            foreach (var tmp in element.GetComponentsInChildren<TextMeshPro>())
            {
                tmp.SetText(name);
            }
            yCord = yCord - 0.15f;
        }
    }
#if ENABLE_WINMD_SUPPORT
    public UnityAction SelectBluetoothDevice(string name)
    {
        var bluetoothDevice = this.name2bluetooth[name];
        var device = this.name2bluetooth[name];
        debugtext.text += "\ncreate callback\n";
        return async () =>
        {
            var services = await bluetoothDevice.GetRfcommServicesForIdAsync(RfcommServiceId.FromUuid(Guid.Parse("00001101-0000-1000-8000-00805F9B34FB")), BluetoothCacheMode.Uncached);
            if (services.Services.Count > 0)
            {
                var service = services.Services[0];
                socket = new StreamSocket();
                await socket.ConnectAsync(service.ConnectionHostName, service.ConnectionServiceName, SocketProtectionLevel.BluetoothEncryptionAllowNullAuthentication);
                inputStream = socket.InputStream.AsStreamForRead();
                outputStream = socket.OutputStream.AsStreamForWrite();
                SendCommandsViaConnection();
            }
            else
            {
            }
        };
    }  
    public void SendCommandsViaConnection()
    {
        byte[] commandType = BitConverter.GetBytes(0.0f);
        outputStream.Write(commandType, 0, commandType.Length);
        outputStream.Flush();
    }
#endif
    public enum CommandType
    {
        MOVE,
        CLAW_UP,
        END,
        CLAW_DOWN,
        RESET,
        BACK,
        LOCATION
    }
}
