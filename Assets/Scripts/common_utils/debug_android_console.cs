using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class debug_android_console : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (global_debug_state.is_debug)
        {
            GameObject.FindObjectOfType<Text>().text = debub_console_log.msg;
        }
    }
}

public static class debub_console_log
{
    public static string msg = "DEBUG";
}
