using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformSelektor : MonoBehaviour
{
    public GameObject HoloLensCanvas;
    public GameObject AndroidCanvas;
    // Start is called before the first frame update
    void Start()
    {
#if UNITY_WSA
        Destroy(AndroidCanvas);
        HoloLensCanvas.SetActive(true);
#elif UNITY_ANDROID
        Destroy(HoloLensCanvas);
        AndroidCanvas.SetActive(true);
#endif
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
