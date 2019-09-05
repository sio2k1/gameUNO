using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//using this for handling search input in ladder
public class input_ladder : MonoBehaviour
{
    public InputField inp;
    public ladder_board ladder;
    public void inp_ladder_onEndEdit() // hide scores on back click
    {
        ladder.display_search(inp.text);
        


    }
}
