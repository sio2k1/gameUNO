using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;
using UnityEngine.UI;

//this script is defined a gamestate class, which stores our game state, IGame_level interface, which our levels inherit from, and level finish callback 

public delegate void level_finished(scene_state st); //delegate we use to determin if our level is finished
public interface IGame_level
{
    void run_game_level(scene_state st, level_finished callback); // level start ocuures here
    void level_input_handler(string input_text); // this method we will use to handle input inside of level
    scene_state return_level_results(); // this method return level results

}

[DataContract]
public class scene_state
{
    public enum states // defines gamestates
    {
        intro, wait_for_dest_cmd, level_progress, wait_for_input_answer, level_intro, wait_for_input_player_name, enemy_pronouncing_question
    }
    [DataMember]
    public int total_score = 0; // record scores
    [DataMember]
    public level current_level; // current level we r doing
    [DataMember]
    public states scene_stt; // current state
    [DataMember]
    public List<level> levels = new List<level>(); //all set of levels
    //private states Scene_stt { get => scene_stt; set => scene_stt = value; }


}
[DataContract]
public class level // store info about particular level
{
    [DataMember]
    public string name="";
    [DataMember]
    public int score=0;
    [DataMember]
    public float time_level_finished;
    [DataMember]
    public float time_level_started;
    [DataMember]
    public bool passed = false; // determine if we pass this level already
    [DataMember]
    public question current_question; // current question for level
    [DataMember]
    public List<question> questions = new List<question>(); // list of questions for level

    public level(string level_name)
    {
        name = level_name;
    }
}


public static class levelnames // define of levelnames
{
    public static string West = "west";
    public static string East = "east";
    public static string North = "north";
    public static List<string> names = new List<string>(){West, East, North};

}


