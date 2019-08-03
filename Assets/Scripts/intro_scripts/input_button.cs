using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class input_button : MonoBehaviour
{
    public InputField inp;
    //public intro_scene_manager ism;
    public console_input_handling cih;
    // Start is called before the first frame update
    public void on_button_input_click()
    {
        cih.input_on_new_text_handler(inp.text); // pass input text to input handler
        //ism.button_handler();
        inp.text = "";
        inp.ActivateInputField();
    }
}
