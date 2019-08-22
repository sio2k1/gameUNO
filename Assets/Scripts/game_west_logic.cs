using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Data;
using cmn_infrastructure;

public class game_west_logic : MonoBehaviour, Igame_level
{
    /*public visual_effects vief;
    public Canvas player_bubble;
    public GameObject enemy_prefab;
    private GameObject enemy;
    private Canvas enemy_box_canvas;*/

    public scene_objects scene;
    //DataTable storyline;

    string debug = "LIMIT 1";

    private scene_state current_game_state;
    private level_finished lvl_finish_callback;

    IEnumerator level_start()
    {

        //storyline = sqlite_db_helper.GetTable("SELECT * from storyline where scene='"+current_game_state.current_level.name+"'");

        scene.scene_fade_to(0f);
        yield return new WaitForSeconds(3);
        scene.level_west_enemy_instatinate();
        scene.player_bubble_enabled(false);
        scene.level_west_enemy_enabled(false);

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

    void questions_init()
    {
        for(int i=0; i<5;i++)
        {
            question q = math_question_builder.create_math_question(10 + i);
            if (debug != "")
            {
                q.question_text += ":" + q.answers[0].txt; //we will show right answer in question for debug reasons
            }
            current_game_state.current_level.questions.Add(q);
        }
        current_game_state.current_level.questions.Reverse();
    }

    public void run_game_level(scene_state st, level_finished callback)
    {
        lvl_finish_callback = callback;
        current_game_state = st;
        questions_init();
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
            if (q.answers.Count==1)
            {
                
                if (q.answers[0].txt.ToLower()==input_text.ToLower())
                {
                    //ok
                    current_game_state.current_level.score += 2;
                    //score_calculator.add_right_answer(this);
                    StartCoroutine(next_question(true,input_text));
                } else
                {
                    StartCoroutine(next_question(false, input_text));
                    //next question
                }
            }
        }
    }

    void Start()
    {
        //score_start_time = Time.time;
    }

    IEnumerator show_question(question q)
    {
        yield return null;
        current_game_state.current_level.current_question = q;
        scene.level_west_enemy_bubble_type_text(q.question_text);
        //float te_delay = vief.type_text_for_canvas_bubble(enemy_box_canvas, q.question_text);
        //int delay_len = Mathf.RoundToInt(te_delay * q.question_text.Length + 2); // depending on type_text delay and lettercount we delay output so text coud be read
        //yield return new WaitForSeconds(delay_len);

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
