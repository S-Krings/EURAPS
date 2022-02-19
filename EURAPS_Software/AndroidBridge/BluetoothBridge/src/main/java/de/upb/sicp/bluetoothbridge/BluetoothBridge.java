package de.upb.sicp.bluetoothbridge;

import android.bluetooth.BluetoothAdapter;
import android.bluetooth.BluetoothDevice;
import android.bluetooth.BluetoothSocket;
import android.content.Intent;
import android.util.Log;

import java.io.IOException;
import java.io.InputStream;
import java.io.OutputStream;
import java.util.Set;
import java.util.UUID;

import static android.app.PendingIntent.getActivity;

public class BluetoothBridge {
    private static BluetoothBridge Instance;
    private static ConnectedThread thread;
    public static String deviceName = "None";
    public static byte[] bytes = new byte[0];
    private final static int REQUEST_ENABLE_BT = 1;
    public static final String TAG = "SPEARED-BT";
    public static final String ServiceUUID = "00001101-0000-1000-8000-00805F9B34FB";
    public static final BluetoothBridge getInstance() {
        if(Instance == null){
            Instance = new BluetoothBridge();
        }
        return Instance;
    }
    public void connect(String deviceName){
        Log.d("SPEARED-BT","connect to device");
    }
    public String connect(){
        Log.d("SPEARED-BT","connect to device");
        ConnectThread connectThread = new ConnectThread();
        connectThread.run();
        Log.d("SPEARED-BT","succeed connect");
        return deviceName;
    }
    public void write(){
        Log.d("SPEARED-BT","start write");
        thread.write(bytes);
        Log.d("SPEARED-BT","end write");
    }
    public void close(){
        Log.d("SPEARED-BT","stop socket");
        thread.cancel();
        Log.d("SPEARED-BT","stop socket");
    }
    private class ConnectThread extends Thread {
        private BluetoothSocket mmSocket;
        //private final BluetoothDevice mmDevice;

        public void run() {
            // Use a temporary object that is later assigned to mmSocket
            // because mmSocket is final.
            BluetoothSocket tmp = null;
            //mmDevice = device;
            BluetoothAdapter bluetoothAdapter = BluetoothAdapter.getDefaultAdapter();
            if (bluetoothAdapter == null) {
                Log.d(TAG,"no bluetoothAdapter found");
                // Device doesn't support Bluetooth
            }
            if (!bluetoothAdapter.isEnabled()) {
                // This does not work for now
                // Intent enableBtIntent = new Intent(BluetoothAdapter.ACTION_REQUEST_ENABLE);
                //startActivityForResult(enableBtIntent, REQUEST_ENABLE_BT);
            }

            Set<BluetoothDevice> pairedDevices = bluetoothAdapter.getBondedDevices();
            BluetoothDevice NXTdevice = null;
            if (pairedDevices.size() > 0) {
                // There are paired devices. Get the name and address of each paired device.
                for (BluetoothDevice device : pairedDevices) {
                    String deviceName = device.getName();
                    String deviceHardwareAddress = device.getAddress(); // MAC address
                    Log.d(TAG,"device: "+deviceName);
                    if(deviceName.equals(BluetoothBridge.deviceName)){
                        NXTdevice = device;
                    }
                }
            }
            bluetoothAdapter.cancelDiscovery();


            try {
                // Get a BluetoothSocket to connect with the given BluetoothDevice.
                // MY_UUID is the app's UUID string, also used in the server code.
                tmp = NXTdevice.createRfcommSocketToServiceRecord(UUID.fromString(ServiceUUID));
            } catch (IOException e) {
                Log.e(TAG, "Socket's create() method failed", e);
            }
            mmSocket = tmp;
            // Cancel discovery because it otherwise slows down the connection.
            if (bluetoothAdapter == null) {
                Log.d(TAG,"no bluetoothAdapter found");
                // Device doesn't support Bluetooth
            }
            if (!bluetoothAdapter.isEnabled()) {
                // This does not work for now
                // Intent enableBtIntent = new Intent(BluetoothAdapter.ACTION_REQUEST_ENABLE);
                //startActivityForResult(enableBtIntent, REQUEST_ENABLE_BT);
            }

            bluetoothAdapter.cancelDiscovery();

            try {
                // Connect to the remote device through the socket. This call blocks
                // until it succeeds or throws an exception.
                mmSocket.connect();
            } catch (IOException connectException) {
                // Unable to connect; close the socket and return.
                try {
                    mmSocket.close();
                } catch (IOException closeException) {
                    Log.e(TAG, "Could not close the client socket", closeException);
                }
                return;
            }

            // The connection attempt succeeded. Perform work associated with
            // the connection in a separate thread.
            Log.d(TAG,"switch to connected");
            thread = new ConnectedThread(mmSocket);
            thread.start();
        }

        // Closes the client socket and causes the thread to finish.
        public void cancel() {
            try {
                mmSocket.close();
            } catch (IOException e) {
                Log.e(TAG, "Could not close the client socket", e);
            }
        }
    }
    private class ConnectedThread extends Thread {
        private final BluetoothSocket mmSocket;
        private final InputStream mmInStream;
        private final OutputStream mmOutStream;
        private byte[] mmBuffer; // mmBuffer store for the stream

        public ConnectedThread(BluetoothSocket socket) {
            mmSocket = socket;
            InputStream tmpIn = null;
            OutputStream tmpOut = null;

            // Get the input and output streams; using temp objects because
            // member streams are final.
            try {
                tmpIn = socket.getInputStream();
            } catch (IOException e) {
                Log.e(TAG, "Error occurred when creating input stream", e);
            }
            try {
                tmpOut = socket.getOutputStream();
            } catch (IOException e) {
                Log.e(TAG, "Error occurred when creating output stream", e);
            }

            mmInStream = tmpIn;
            mmOutStream = tmpOut;
        }

        public void run() {
            mmBuffer = new byte[1024];
            int numBytes; // bytes returned from read()

            // Keep listening to the InputStream until an exception occurs.
            Log.d(TAG,"run ConnectedThread");
            while (true) {
                try {
                    // Read from the InputStream.
                    numBytes = mmInStream.read(mmBuffer);
                    // Send the obtained bytes to the UI activity.
                    //Message readMsg = handler.obtainMessage(
                    //        MessageConstants.MESSAGE_READ, numBytes, -1,
                    //        mmBuffer);
                    //readMsg.sendToTarget();
                } catch (IOException e) {
                    Log.d(TAG, "Input stream was disconnected", e);
                    break;
                }
            }
        }

        // Call this from the main activity to send data to the remote device.
        public void write(byte[] bytes) {
            Log.e(TAG, "Write data");
            try {
                mmOutStream.write(bytes);
                mmOutStream.flush();
                Log.d(TAG,"wrote bytes");
            } catch (IOException e) {
                Log.e(TAG, e.toString());
            }
        }

        // Call this method from the main activity to shut down the connection.
        public void cancel() {
            try {
                mmSocket.close();
            } catch (IOException e) {
                Log.e(TAG, "Could not close the connect socket", e);
            }
        }
    }


}
