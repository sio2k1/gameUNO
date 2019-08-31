using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;

public class console_input_handling : MonoBehaviour
{
    // Start is called before the first frame update
    scene_state gamestate;
    public game_level_logic level_logic;

    void init()
    {
        gamestate = new scene_state();
        gamestate.scene_stt = scene_state.states.wait_for_dest_cmd;
        level l1 = new level(levelnames.West);
        level l2 = new level(levelnames.East);
        level l3 = new level(levelnames.North);
        gamestate.levels.Add(l1);
        //gamestate.levels.Add(l2);
        //gamestate.levels.Add(l3);
    }
    void Start()
    {
        //initialisation
        init();

    }

    public void level_finish_handler(scene_state st)
    {
        //after level finish we come here from callback.
        //Debug.Log("lvl fin");

        gamestate = st;
        gamestate.current_level.time_level_finished = Time.time;
        gamestate.current_level.passed = true;
        foreach (level l in gamestate.levels)
        {
            if (l.name==gamestate.current_level.name) //replacing level with updated info of completed level
            {
                gamestate.levels.Remove(l);
                gamestate.levels.Add(gamestate.current_level);
                break;
            }
        }
        bool any_levels_left = false;
        foreach (level l in gamestate.levels)
        {
            if (l.passed==false)
            {
                any_levels_left = true;
            }
        }
        if (any_levels_left)
        {
            //map
            gamestate.scene_stt = scene_state.states.wait_for_dest_cmd;
            level_logic.map_show(true);
        } else
        {
            int score = 0;
            foreach (level l in gamestate.levels)
            {
                score += Mathf.RoundToInt(l.score*1000/(l.time_level_finished- l.time_level_started));
            }
            db_helper_menu.write_scores(gamestate.player_name, score);
                //game over - calc results 


             SceneManager.LoadScene(0);
        }


    }

    void destination_handler(string input_text)
    {
        bool right_dest = false;
        var linq = gamestate.levels.Where(p => p.passed == false); // we select levels we haven't visited yet
        foreach (level l in linq)
        {
            if (l.name.ToLower() == input_text)
            {
                right_dest = true;
                gamestate.current_level = l;
                gamestate.scene_stt = scene_state.states.level_intro;
                level_finished l_f_handler = level_finish_handler;

                level_logic.run_game_level(gamestate, l_f_handler);
                //change scene
            }
            if (!right_dest)
            {
                //wrong dest/or level passed
            }
        }
    }

    void level_progress_input_forwarder(string input_text)
    {
        //if (gamestate.current_level.name.ToLower() == levelnames.West)
        //{
            //pass_input_to_west_mngr
            level_logic.level_input_handler(input_text);
        //}
    }


    IEnumerator handling_thread(string input_text)
    {
        yield return new WaitForSeconds(0.1f);
        if (gamestate.scene_stt == scene_state.states.wait_for_dest_cmd) //if scenestate is dest_wait -> we pass input to dest_handler
        {
            destination_handler(input_text);
        }
        if (gamestate.scene_stt == scene_state.states.wait_for_input_answer) //if scenestate is level in progress -> we pass input to that level control script
        {
            level_progress_input_forwarder(input_text);
        }
    }

    public void input_on_new_text_handler(string input_text)
    {
        StartCoroutine(handling_thread(input_text));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
