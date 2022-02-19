using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class NXTBluetoothSocket
{
    protected UnityAction loadedAction;
    public abstract List<string> getDeviceNames();
    public abstract void openConnectionTo(string deviceName);
    public void openConnectionToDefault()
    {
        this.openConnectionTo("NXT");
    }
    public abstract void write(byte[] bytes);
    public abstract byte[] read();
    public abstract void close();
}
