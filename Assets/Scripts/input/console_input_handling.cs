using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;
using System.IO;
using cmn_infrastructure;

//this is main console input handler, there is a component attatched to input and "go" button in input_button.cs which passes all input here to input_on_new_text_handler method

public class console_input_handling : MonoBehaviour
{
    scene_state gamestate;
    public game_level_logic level_logic; //link to class that controlls behavior of level
    public game_end_objects ges; // link to class that handles player name input

    void init()
    {
        //setup env, clear gamestate and assing levels
        gamestate = new scene_state();
        gamestate.scene_stt = scene_state.states.wait_for_dest_cmd;
        level l1 = new level(levelnames.West);
        level l2 = new level(levelnames.East);
        level l3 = new level(levelnames.North);
        
        gamestate.levels.Add(l1);
        gamestate.levels.Add(l2);
        gamestate.levels.Add(l3);
    }
    void Start()
    {
        init(); // this is called on scene load, so every time we load scene we clear gamestate in init.
    }

    private async void game_end_handler(scene_state st) //game over - calc results, set state to waiting player name
    {
        int score = 0;
        foreach (level l in gamestate.levels) // calculating scores
        {
            score += Mathf.RoundToInt(l.score * 2000 / (l.time_level_finished - l.time_level_started));
        }
        ges.game_end_screen_visibility(true);
        level_logic.scene.input_fade(0.0f, 0.01f);
        ges.game_end_screen_set_scores(score);
        gamestate.total_score = score; //TODO: check if we need that, part of older functionality

        await mgame_manager.scores_write_score_to_fb(current_mgame.curr_mgame.key, app_globals.loggined_user_fb, score); // write scores in firebase
        //db_helper_menu.write_scores(app_globals.loggined_user_fb.login_display, score); // write scores in db
        gamestate.scene_stt = scene_state.states.wait_for_input_player_name; // set state where we wait for user to click ok at total score screen

        
    }

    public void level_finish_handler(scene_state st) //after level finish we come here from callback.
    {     
        gamestate = st;
        gamestate.current_level.time_level_finished = Time.time;
        gamestate.current_level.passed = true;
        foreach (level l in gamestate.levels)
        {
            if (l.name==gamestate.current_level.name) //replacing level with updated info and (passed state) of completed level
            {
                gamestate.levels.Remove(l);
                gamestate.levels.Add(gamestate.current_level);
                break;
            }
        }
        bool any_levels_left = false;
        foreach (level l in gamestate.levels) // checking if any levels left to pass
        {
            if (l.passed==false)
            {
                any_levels_left = true;
            }
        }
        if (any_levels_left) // if any levels left - display map
        {
            //map show if any levels left o pass
            gamestate.scene_stt = scene_state.states.wait_for_dest_cmd;
            level_logic.map_show(true);
        } else // if not do some gameover stuff
        {
            //game over - calc results, set state to waiting player name
            game_end_handler(st);
        }
    }


    bool game_save_handler(string input_text)
    {
        bool res = false;
        if (input_text=="save")
        {
            res = true;

            cmn_infrastructure.serializer_helper.json_serialize_object_to_string(gamestate, Path.Combine(Application.persistentDataPath, "game_save.json"));
            Debug.Log("Saved");
        }

        if (input_text == "load")
        {
            res = true;
            try
            {
                gamestate = cmn_infrastructure.serializer_helper.json_deserialize_object_from_file<scene_state>(Path.Combine(Application.persistentDataPath, "game_save.json"));
                level_logic.map_redreaw_according_to_scenetate(gamestate);
            } catch
            {
                Debug.Log("Error during loading");
            }
            Debug.Log("Loaded");
        }
        return res;
    }

    void destination_handler(string input_text) // handling destination to go
    {
        bool right_dest = false;
        gamestate.levels.Where(p => p.passed == false).ToList().ForEach(l=> { // we select levels we haven't visited yet, and apply foreach arrow func
            if (l.name.ToLower() == input_text) //if found level equal to input -> go to that level
            {
                right_dest = true;
                gamestate.current_level = l;
                gamestate.scene_stt = scene_state.states.level_intro;
                level_finished l_f_handler = level_finish_handler; // callback on level is finished
                level_logic.run_game_level(gamestate, l_f_handler);
                //change scene
            }
        });

        right_dest = game_save_handler(input_text); //mb we want to save with "save" cmd

        if (!right_dest)
        {
            Debug.Log("Wrong destination");
            //wrong dest/or level passed
        }
    }

    void level_progress_input_forwarder(string input_text) //forward input handling to level logic
    {
        level_logic.level_input_handler(input_text);
    }


    IEnumerator handling_thread(string input_text) // async handling of input, all input is comming here.
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
        if (gamestate.scene_stt == scene_state.states.wait_for_input_player_name) //if scenestate is wait_for_input_player_name -> we write scores to db and load menu.
        {
            //we handle score writing here before, now its moved to
            //db_helper_menu.write_scores(input_text, gamestate.total_score);
            //SceneManager.LoadScene(0);
        }

    }

    public void input_on_new_text_handler(string input_text) // general handling of input, all input is comming here
    {
        try
        {
            if (input_text != "") // if not empty
            {
                StartCoroutine(handling_thread(input_text.ToLower())); // passing input in lower case
            }
        } catch
        {
            Debug.Log("Input handling error");
        }
    }

}
