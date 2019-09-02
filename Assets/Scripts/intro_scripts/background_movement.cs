using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//this script is for smootly moove background in intro,we apply it to our background object

public class background_movement : MonoBehaviour
{
    public float speed; //bg transition speed
    private Vector3 start_pos; //we keep starting pos here
    scene_objects scene_ob;
    private float time_counter_for_stop=0;
    public float stop_in_sec = 3; // how long it takes to stop scrolling in sec

    public bool scrollbg; // determine if we scroll bg or not

    void Start()
    {
        scene_ob = GameObject.FindObjectOfType<scene_objects>();
        start_pos = transform.position;
    }


    void Update()
    {
        if (scrollbg)
        {
            //scroll bg according to speed
            transform.Translate((new Vector3(-1, 0, 0)) * speed * Time.deltaTime);
        } else
        {
            //stop scrolling smoothly
            if (time_counter_for_stop < stop_in_sec)
            {
                time_counter_for_stop += Time.deltaTime;
                transform.Translate((new Vector3(-1, 0, 0)) * Mathf.Lerp(speed, 0, time_counter_for_stop/ stop_in_sec) * Time.deltaTime);
            }
        }
    }
}
