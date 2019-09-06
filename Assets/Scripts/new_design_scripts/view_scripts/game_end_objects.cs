using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//this script provides acces to input player name interface and total scores gained in the end of the game

public class game_end_objects : MonoBehaviour
{
    public GameObject game_end_obj;

    public void game_end_screen_visibility(bool show) // show or hide input player name and total scores
    {
        game_end_obj.SetActive(show);
    }

    public void game_end_screen_set_scores(int scores) // set scores value to Text component "txt_score"
    {

        List<Text> l = new List<Text>();
        game_end_obj.GetComponentsInChildren<Text>(l);
        l.Find(a => a.name == "txt_score").text=scores.ToString();

    }

}
