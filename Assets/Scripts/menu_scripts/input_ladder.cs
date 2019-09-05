using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//using this for handling search input in ladder
public class input_ladder : MonoBehaviour
{
    public InputField inp; // link to input field
    public ladder_cl ladder; // link to ladder gameobject
    public void inp_ladder_onEndEdit() // hide scores on back click
    {
        ladder.display_search(inp.text); // display search results
        inp.ActivateInputField(); // set cursor to inputfield after search
    }
    private void Start()
    {
        inp.ActivateInputField(); // set cursor to inputfield on start
    }
}
