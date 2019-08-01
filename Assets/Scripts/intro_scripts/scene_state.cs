using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class scene_state
{
    public enum states
    {
        intro, wait_for_dest_cmd
    }

    public states scene_stt;
    public List<level> levels = new List<level>();

    private states Scene_stt { get => scene_stt; set => scene_stt = value; }


}

public class level
{
    public string name;
    public int score;
    public int time_in_sec;
}
