using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//this is input handler transfer script, all it does is send input to console_input_handling
public class input_button : MonoBehaviour
{
    public InputField inp;
    public console_input_handling cih;
    public void on_button_input_click()
    {
        cih.input_on_new_text_handler(inp.text); // pass input text to input handler
        inp.text = ""; // clear last command
        
        #if UNITY_EDITOR
        inp.ActivateInputField(); //return cursor to input field
        #endif
    }
}
