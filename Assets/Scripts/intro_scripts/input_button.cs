using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class input_button : MonoBehaviour
{
    public InputField inp;
    public intro_scene_manager ism;
    // Start is called before the first frame update
    public void on_button_input_click()
    {
        
            ism.button_handler(inp.text);
        
    }
}
