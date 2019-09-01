using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class game_end_objects : MonoBehaviour
{
    public GameObject game_end_obj;


    public void game_end_screen_visibility(bool show)
    {

        game_end_obj.SetActive(show);
    }

    public void game_end_screen_set_scores(int scores)
    {

        List<Text> l = new List<Text>();
        game_end_obj.GetComponentsInChildren<Text>(l);
        l.Find(a => a.name == "txt_score").text=scores.ToString();

    }

}
