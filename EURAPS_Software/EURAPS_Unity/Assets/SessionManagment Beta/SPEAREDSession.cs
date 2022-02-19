using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SPEAREDSession : MonoBehaviour
{
    private static SPEAREDSession Instance;
    public string activeScenario { get; set; }

    public static SPEAREDSession GetInstance()
    {
        return Instance;
    }
    void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
