using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using System;

// script to operate with scoreboard

public class ladder_cl : MonoBehaviour
{
    public GameObject table_line; // table line prefab
    public GameObject ladder_panel; //panel at ladderboard
    public GameObject loading_screen;
    public void clear_scores() // clear scores, remove all existing table lines at start
    {
        var tagged = GameObject.FindGameObjectsWithTag("ladder_to_del");
        foreach (GameObject go in tagged) // delete everything with tag ladder_to_del
        {
            Destroy(go);
        }
    }

    public void display_search(string input_text) //display player by name in search field (top 10 from search list)
    {
        //List<db_helper_menu.table_line> lines = db_helper_menu.search_in_ladder(input_text); // get from DB using search conditions
        //display_scores(lines);
    }


    public void display_scores(List<mgame_scoreboard_game_entry> games) // display lines as prefabed table lines
    {
        clear_scores(); // clear old first

        games.ForEach(g => {
            var tl = Instantiate(table_line, new Vector3(0, 0, 0), Quaternion.identity, ladder_panel.transform); // place on panel to autodistribution with "vertical layout group"
            tl.tag = "ladder_to_del";

            tl.GetComponent<vis_scoreboarg_game_entry>().game_id.text = "GameID:" + g.gamekey;

            g.lines = g.lines.OrderByDescending(x => x.score).ToList();
            g.lines.ForEach(l=> {
                tl.GetComponent<vis_scoreboarg_game_entry>().add_player("Player:" + l.player_display_name + "\r\n" + "Score:" + l.score);
            });
        });

    }
    /*public void display_scores(List<db_helper_menu.table_line> lines) // display lines as prefabed table lines
    {
        clear_scores(); // clear old first
        foreach (var l in lines) // for each line create table_line
        {
            var tl = Instantiate(table_line, new Vector3(0, 0, 0), Quaternion.identity, ladder_panel.transform); // place on panel to autodistribution with "vertical layout group"
            tl.tag = "ladder_to_del";
            foreach (var t in tl.GetComponentsInChildren<Text>()) // asignin values to table fields
            {
                if (t.name.ToLower() == "position")
                {
                    t.text = l.pos;
                }
                if (t.name.ToLower() == "player")
                {
                    t.text = l.player;
                }
                if (t.name.ToLower() == "score")
                {
                    t.text = l.score;
                }
            }
        }
    }*/

    public async void load_scores() // load scores from DB
    {
        //List<db_helper_menu.table_line> lines = db_query_invoker.invoke(db_helper_menu.load_ladder); // get info from DB, we using delegate to cover method in try/catch
        //List<db_helper_menu.table_line> lines = db_helper_menu.load_ladder(); // get info from DB
        loading_screen.SetActive(true);
        try
        {
            List<mgame_scoreboard_game_entry> sbe = await mgame_manager.scores_get_score_from_fb(app_globals.loggined_user_fb);
            display_scores(sbe);
        }
        catch (Exception e)
        {
            Debug.Log("Can't get scores:"+e.Message);
        }
        loading_screen.SetActive(false);


    }
}
