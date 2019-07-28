using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

//this script is an example to access game manager 
public class narrator_bubble_storyline : MonoBehaviour
{
    
    private intro_scene_manager ism;
    private void Awake()
    {
        ism = GameObject.FindObjectOfType<intro_scene_manager>(); 
    }

}
