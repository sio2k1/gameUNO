using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Data;
using cmn_infrastructure;

public class game_level_logic : MonoBehaviour, Igame_level
{
    public scene_objects scene;
    public map_objects map_obj;
    //string debug = "LIMIT 1";
    private scene_state current_game_state;
    private level_finished lvl_finish_callback;

    IEnumerator level_start()
    {

        //storyline = sqlite_db_helper.GetTable("SELECT * from storyline where scene='"+current_game_state.current_level.name+"'");
        StartCoroutine(map_obj.move_hero(current_game_state.current_level));
        yield return new WaitForSeconds(1);
        map_show(false);
        scene.scene_fade_to(0f);
        yield return new WaitForSeconds(3);
        scene.level_enemy_instatinate();
        scene.player_bubble_enabled(false);
        scene.level_west_enemy_enabled(false);

        scene.background_change(current_game_state.current_level);
        scene.change_enemy_model();

        yield return new WaitForSeconds(1);
        scene.scene_fade_to(1f);
        scene.player_bubble_fade(0f, 0f);
        scene.level_west_enemy_bubble_fade(0f, 0f);
        
        yield return new WaitForSeconds(3);

        StartCoroutine(enemy_intro());
        //StartCoroutine(player_talk_dest_choise());
    }


    IEnumerator enemy_intro()
    {
        
        yield return new WaitForSeconds(0.1f);
        foreach (db_helper_west.dialog_entry de in db_helper_west.level_west_intro_talk())
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
        current_game_state.scene_stt = scene_state.states.level_progress;
        current_game_state.current_level.time_level_started = Time.time;
    }

    void questions_init(level lvl)
    {
        current_game_state.current_level.questions.Clear();

        if (lvl.name == levelnames.West)
        {
            current_game_state.current_level.questions = db_helper_questions.load_questions(levelnames.West, 5);
            //current_game_state.current_level.questions.Reverse();
        }

        if (lvl.name == levelnames.East)
        {
            current_game_state.current_level.questions = db_helper_questions.load_questions(levelnames.East, 4 );
        }
    }

    public void run_game_level(scene_state st, level_finished callback)
    {
        lvl_finish_callback = callback;
        current_game_state = st;
        questions_init(current_game_state.current_level);
        StartCoroutine(level_start());

    }

    public scene_state return_level_results()
    {
        return current_game_state;
    }

    IEnumerator next_question(bool correct, string input_text)
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
        current_game_state.scene_stt = scene_state.states.level_progress;
    }

    public void level_input_handler(string input_text)
    {
        if (current_game_state.scene_stt == scene_state.states.wait_for_input_answer)
        {
            question q = current_game_state.current_level.current_question;
            if (q.answers.Count == 1) // if question with one answer
            {

                if (q.answers[0].txt.ToLower() == input_text.ToLower())
                {
                    //ok
                    current_game_state.current_level.score += 1;
                    //score_calculator.add_right_answer(this);
                    StartCoroutine(next_question(true, input_text));
                } else
                {
                    StartCoroutine(next_question(false, input_text));
                    //next question
                }
            }
            else // if question with answer choice
            {
                if ((q.get_correct_answer().txt.ToLower() == input_text.ToLower())||(q.get_correct_answer_position()== input_text))
                {
                    //ok
                    current_game_state.current_level.score += 2;
                    //score_calculator.add_right_answer(this);
                    StartCoroutine(next_question(true, input_text));
                }
                else
                {
                    StartCoroutine(next_question(false, input_text));
                    //next question
                }
            }
        }
    }

    public void map_show(bool visibility)
    {
        map_obj.map_visibility(visibility);
    }

    void Start()
    {
        //score_start_time = Time.time;
    }

    IEnumerator show_question(question q)
    {
        yield return null;
        current_game_state.current_level.current_question = q;
        scene.level_west_enemy_bubble_type_text(q.generate_question_text());


    }

    float timer;
    void Update()
    {
        timer += Time.deltaTime;
        if (timer>0.300f) //evry 300ms
        {
            timer = 0;
            if (current_game_state != null)
            {
                if (current_game_state.scene_stt == scene_state.states.level_progress)
                {
                    current_game_state.scene_stt = scene_state.states.wait_for_input_answer;
                    int qcount = current_game_state.current_level.questions.Count;
                    if (qcount > 0)
                    {
                        question q = current_game_state.current_level.questions[qcount - 1];
                        current_game_state.current_level.questions.RemoveAt(qcount - 1);
                        StartCoroutine(show_question(q));
                    }
                    else
                    {
                        
                        lvl_finish_callback(current_game_state);
                        //level ends here
                    }
                }
            }
        }

    }
}
