using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class background_movement : MonoBehaviour
{
    public float speed; //bg transition speed
    private Vector3 start_pos; //we keep starting pos here
    scene_objects ism;
    private float time_counter_for_stop=0;
    public float stop_in_sec = 3;

    public bool scrollbg;

    void Start()
    {
        ism = GameObject.FindObjectOfType<scene_objects>();
        start_pos = transform.position;
    }


    void Update()
    {
        if (scrollbg)
        {
            transform.Translate((new Vector3(-1, 0, 0)) * speed * Time.deltaTime);
        } else
        {
            if (time_counter_for_stop < stop_in_sec)
            {
                time_counter_for_stop += Time.deltaTime;
                transform.Translate((new Vector3(-1, 0, 0)) * Mathf.Lerp(speed, 0, time_counter_for_stop/ stop_in_sec) * Time.deltaTime);
            }
        }
    }
}
