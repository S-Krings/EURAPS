#if ENABLE_WINMD_SUPPORT
using System.IO;
using Windows.Devices.Enumeration;
using Windows.Devices.Bluetooth;
using Windows.Devices.Bluetooth.Rfcomm;
using Windows.Networking.Sockets;
using System.Collections.Generic;

using System.Threading.Tasks;
using Windows.Foundation;
#endif

using System;
using UnityEngine.Events;
using System.Linq;
using System.Collections.Generic;
using TMPro;

public class NXTBluetoothSocktUWP : NXTBluetoothSocket
{
    private List<string> names;
#if ENABLE_WINMD_SUPPORT

    private Dictionary<string, BluetoothDevice> name2bluetooth = new Dictionary<string, BluetoothDevice>();
    private StreamSocket socket;
    private Stream inputStream;
    private Stream outputStream;
    private bool activeSocket = false;
    public TMP_Text debugText = null;
#endif
    public async void findDevices(Action a, TMP_Text debug)
    {
        debug.text += "Start finddevice\n";
#if ENABLE_WINMD_SUPPORT
        debug.text += "windmd enabled\n";
        names = new List<string>();
        //debug.text = "Pre query";
        var informations = await DeviceInformation.FindAllAsync(BluetoothDevice.GetDeviceSelectorFromPairingState(true));
        //debug.text = "Post query";
        foreach (var info in informations)
        {
            names.Add(info.Name.ToLower());
            debug.text += info.Name+"\n";
            name2bluetooth.Add(info.Name.ToLower(), await BluetoothDevice.FromIdAsync(info.Id));
        }
        debug.text += "Pre invoke\n";
        a.Invoke();
        debug.text += "Post invoke\n";
#endif
    }
    public override byte[] read()
    {
#if ENABLE_WINMD_SUPPORT
        
#endif
        return new byte[0];
    }

    public override void write(byte[] bytes)
    {
#if ENABLE_WINMD_SUPPORT
        outputStream.Write(bytes, 0, bytes.Length);
        outputStream.Flush();
#endif
    }

    public override List<string> getDeviceNames()
    {
        return this.names.Select(x => x).ToList();
    }

    public async override void openConnectionTo(string deviceName)
    {
#if ENABLE_WINMD_SUPPORT
        deviceName = deviceName.ToLower();
        debugText.text += "open Connection\n";
        var bluetoothDevice = this.name2bluetooth[deviceName];
        var services = await bluetoothDevice.GetRfcommServicesForIdAsync(RfcommServiceId.FromUuid(Guid.Parse("00001101-0000-1000-8000-00805F9B34FB")), BluetoothCacheMode.Uncached);
        debugText.text += services.Services.Count+"\n";
        if (services.Services.Count > 0)
        {
            var service = services.Services[0];
            socket = new StreamSocket();
            await socket.ConnectAsync(service.ConnectionHostName, service.ConnectionServiceName, SocketProtectionLevel.BluetoothEncryptionAllowNullAuthentication);
            inputStream = socket.InputStream.AsStreamForRead();
            outputStream = socket.OutputStream.AsStreamForWrite();
            activeSocket = true;
        }
        else
        {
        }
#endif
    }

    public override void close()
    {
#if ENABLE_WINMD_SUPPORT
        socket.Dispose();
#endif
    }
}
