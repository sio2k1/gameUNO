using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public delegate void level_finished(scene_state st);
public interface Igame_level
{
    void run_game_level(scene_state st, level_finished callback);
    void level_input_handler(string input_text);
    scene_state return_level_results();

}
public class scene_state
{
    public enum states
    {
        intro, wait_for_dest_cmd, level_progress, wait_for_input_answer, level_intro, wait_for_input_player_name
    }
   // public string player_name = "test";
    public int total_score = 0;
    public level current_level;
    public states scene_stt;
    public List<level> levels = new List<level>();

    private states Scene_stt { get => scene_stt; set => scene_stt = value; }


}

public class level
{
    public string name="";
    public int score=0;
    public float time_level_finished;
    public float time_level_started;
    public bool passed = false;
    public question current_question;
    public List<question> questions = new List<question>();


    public level(string level_name)
    {
        name = level_name;
    }
}


public static class levelnames
{
    public static string West = "west";
    public static string East = "east";
    public static string North = "north";
}
