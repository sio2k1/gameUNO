using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public interface Igame_level
{
    void run_game_level(scene_state st);
    void level_input_handler(string input_text);
    scene_state return_level_results();

}
public class scene_state
{
    public enum states
    {
        intro, wait_for_dest_cmd, level_progress
    }

    public level current_level;
    public states scene_stt;
    public List<level> levels = new List<level>();

    private states Scene_stt { get => scene_stt; set => scene_stt = value; }


}

public class level
{
    public string name="";
    public int score=0;
    public int time_in_sec=0;
    public bool passed = false;

    public level(string level_name)
    {
        name = level_name;
    }
}
