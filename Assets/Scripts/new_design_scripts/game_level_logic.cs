using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Data;
using cmn_infrastructure;

/*
 * this class is represent our 3 levels, its a controller class, so we dont operate gameobjects directry, we r using scene_objects for it
 * and we dont operate db directly, we r using db_helper_* for it
 * This class represent logic of level from intro(intro to level with an enemy dialog), during the level to the end of game.
 * This class cooperates with console_input_handler, to receive user input.
*/

public class game_level_logic : MonoBehaviour, IGame_level
{
    public scene_objects scene; // link to scene_objects
    public map_objects map_obj; // link to map
    private scene_state current_game_state; // current game state, defines if we r accepting input, how many questions answered, etc...
    private level_finished lvl_finish_callback; // we call it when there is no questions levt to ask, to determine if level is finished

    private bool level_timer = false; // we dont count time spend on intro, so this var is determine when we actualy start count user spent on questions

    IEnumerator level_start()
    {
        StartCoroutine(map_obj.move_hero(current_game_state.current_level)); //we move avatar of player at map to current level pos
        yield return new WaitForSeconds(1);
        map_show(false); // hide map
        scene.scene_fade_to(0f); //fade scene out to place aneby, change decorations
        yield return new WaitForSeconds(3);
        scene.level_enemy_instatinate(); // create enemy, if enemy is already exists we skip this step
        scene.player_bubble_enabled(false); //hide bubbles
        scene.level_west_enemy_enabled(false); // hide enemy

        scene.background_change(current_game_state.current_level); // set level background
        scene.change_enemy_model(); // set random enemy 

        yield return new WaitForSeconds(1);
        scene.scene_fade_to(1f); // show up the scene
        scene.player_bubble_fade(0f, 0f);
        scene.level_west_enemy_bubble_fade(0f, 0f);
        
        yield return new WaitForSeconds(3);

        StartCoroutine(enemy_intro()); // start level intro
    }


    IEnumerator enemy_intro()
    {
        
        yield return new WaitForSeconds(0.1f);
        foreach (db_helper_level_logic.dialog_entry de in db_helper_level_logic.level_west_intro_talk()) //showing dialog entries for player\enemi in appropriate bubbles
        {
            if (de.char_t == "player")
            {
                if (!scene.player_bubble_is_enabled())
                {
                    scene.player_bubble_enabled(true);
                    scene.player_bubble_fade(1f, 2f);                 
                }
                float delay_len = scene.player_bubble_type_text(de.txt);
                yield return new WaitForSeconds(delay_len);
            }
            if (de.char_t == "enemy")
            {
                if (!scene.level_west_enemy_bubble_is_enabled())
                {
                    scene.level_west_enemy_enabled(true);
                    scene.level_west_enemy_bubble_fade(1f, 2f);
                }
                float delay_len = scene.level_west_enemy_bubble_type_text(de.txt);
                yield return new WaitForSeconds(delay_len);
            }
        }
        yield return new WaitForSeconds(2);
        current_game_state.scene_stt = scene_state.states.level_progress; //change gamestate so we can show next question
        current_game_state.current_level.time_level_started = Time.time; // writing a time for timer
        level_timer = true; // starting level timer here

    }

    void questions_init(level lvl) // init questions for current level
    {
        current_game_state.current_level.questions.Clear();
        current_game_state.current_level.questions = db_helper_questions.load_questions(lvl.name, global_debug_state.questions_per_level); // get questions from db. for debug reasons we load questiond number from global_debug_state

    }

    public void run_game_level(scene_state st, level_finished callback) // this one is called on level start
    {
        lvl_finish_callback = callback; // setup callback for level end
        current_game_state = st; // set current gamestate;
        questions_init(current_game_state.current_level); // load questions from DB
        StartCoroutine(level_start()); // start intro of level

    }

    public scene_state return_level_results() // return gamestate, this is not using atm, as we pass gamestate in callback
    {
        return current_game_state;
    }

    IEnumerator next_question(bool correct, string input_text) // show up answer, display correct or not, change state to display different question
    {
        float delay_len = scene.player_bubble_type_text("I think it is " + input_text);
        yield return new WaitForSeconds(delay_len);

        yield return new WaitForSeconds(1);

        string text = "";

        if (correct)
        {
            text = "Correct!";

        } else
        {
            text = "U wrong!";
        }

        delay_len = scene.level_west_enemy_bubble_type_text(text);
        yield return new WaitForSeconds(delay_len);
        yield return new WaitForSeconds(2);
        current_game_state.scene_stt = scene_state.states.level_progress; // change state to get new question from list in Update(), better to refactor this with callbacks in future
    }

    public void level_input_handler(string input_text) // console_input_handler will pass all level-related input here
    {
 
        if (current_game_state.scene_stt == scene_state.states.wait_for_input_answer) // if we r waiting for answer
        {
            current_game_state.scene_stt = scene_state.states.enemy_pronouncing_question; // enemy speaking true or false, we dont assept input when its happening
            question q = current_game_state.current_level.current_question;
            if ((q.get_correct_answer().txt.ToLower() == input_text.ToLower()) || (q.get_correct_answer_position() == input_text))
            {
                //answer is right
                current_game_state.current_level.score += 1;
                StartCoroutine(next_question(true, input_text));
            }
            else
            {
                //answer is wrong
                StartCoroutine(next_question(false, input_text));
                //next question
            }

        }
    }

    public void map_show(bool visibility) //map visibility changer
    {
        map_obj.map_visibility(visibility);
    }

    public void map_redreaw_according_to_scenetate(scene_state st)
    {
        map_obj.map_redraw_according_scenestate(st);
    }


    IEnumerator show_question(question q) // show current question
    {
        yield return null;
        current_game_state.current_level.current_question = q;
        float delay_len = scene.level_west_enemy_bubble_type_text(q.generate_question_text());
        if (delay_len >= 1.2f) // fix to compensate input delay on question typing...
        {
            yield return new WaitForSeconds(delay_len - 1.2f);
        } else
        {
            yield return new WaitForSeconds(delay_len);
        }
        current_game_state.scene_stt = scene_state.states.wait_for_input_answer; //after anouncing question we can accept input
    }

    float timer; // we use this to perform code on update, as it can affect performance not in every single frame, but every 300 ms
    void Update()
    {
        if (level_timer)
        {
            scene.level_duration_set((Time.time - current_game_state.current_level.time_level_started)); // display seonds you spent at level
       
        }

        timer += Time.deltaTime;
        if (timer>0.300f) // we use this to perform code on update, as it can affect performance not in every single frame, but every 300 ms
        {
            timer = 0;
            if (current_game_state != null)
            {
                if (current_game_state.scene_stt == scene_state.states.level_progress) // if gamestate is  level_progress we show next question
                {
                    current_game_state.scene_stt = scene_state.states.enemy_pronouncing_question; // w8 for enemy to pronounce question fully
                    int qcount = current_game_state.current_level.questions.Count;
                    if (qcount > 0) // if questions left
                    {
                        question q = current_game_state.current_level.questions[qcount - 1]; 
                        current_game_state.current_level.questions.RemoveAt(qcount - 1); // delete question from list
                        StartCoroutine(show_question(q)); // display question text and set as current question
                    }
                    else
                    {
                        //no questions left, we finishing this level
                        level_timer = false; // stop timer
                        scene.level_duration_set(0); // hide digits, that display duration
                        map_obj.map_redraw_according_passed_level(current_game_state.current_level);
                        lvl_finish_callback(current_game_state); // call calback to provide info to conole_input_handler that level is completed
                        //level ends here
                    }
                }
            }
        }

    }
}
