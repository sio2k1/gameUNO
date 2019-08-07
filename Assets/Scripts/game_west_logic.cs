using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Data;
using cmn_infrastructure;

public class game_west_logic : MonoBehaviour, Igame_level
{
    public visual_effects vief;
    public Canvas player_bubble;

    public GameObject enemy_prefab;
    private GameObject enemy;
    private Canvas enemy_box_canvas;
    DataTable storyline;

    int debug = 1;

    private scene_state current_game_state;

    IEnumerator level_start()
    {
        
        storyline = sqlite_db_helper.GetTable("SELECT * from storyline where scene='"+current_game_state.current_level.name+"'");
        //vief.type_text_for_canvas_bubble(player_bubble, "Going west...");
        StartCoroutine(vief.scene_fade_to_0());

        yield return new WaitForSeconds(3);
        enemy = Instantiate(enemy_prefab, new Vector3(-200, -70, 0), Quaternion.identity, GameObject.Find("bigForest").transform); // player spawn so camera can catch hero nicely and smoothly
        enemy_box_canvas = enemy.GetComponentInChildren<Canvas>();
        vief.sprite_renderer_set_alpha(enemy.GetComponent<SpriteRenderer>(), 0); //init enemy sprite with 0 alpha
        
        enemy_box_canvas.enabled = false;
        player_bubble.enabled = false;

        yield return new WaitForSeconds(1);
        StartCoroutine(vief.scene_fade_to_100());
        yield return new WaitForSeconds(1);
        vief.images_and_text_at_canvas_fade(enemy_box_canvas, 0f, 0f);
        vief.images_and_text_at_canvas_fade(player_bubble, 0f, 0f);
        StartCoroutine(enemy_intro());
        //StartCoroutine(player_talk_dest_choise());
    }


    IEnumerator enemy_intro()
    {
        yield return new WaitForSeconds(0.1f);
        foreach (DataRow r in storyline.Select("", "order ASC")) //we starting from story text in narrator box, foreach row in table we display text with delay
        {
            string next_text = r["ctext"].ToString(); //getting a text record from table (field = ctext)
            string char_ = r["char_talks"].ToString();
            if (char_=="player")
            {
                if (!player_bubble.enabled)
                {
                    player_bubble.enabled = true;
                    vief.images_and_text_at_canvas_fade(player_bubble, 1f, 2f);
                }
                float te_delay = vief.type_text_for_canvas_bubble(player_bubble, next_text);
                int delay_len = Mathf.RoundToInt(te_delay * next_text.Length + 2); // depending on type_text delay and lettercount we delay output so text coud be read
                yield return new WaitForSeconds(delay_len);
            }
            if (char_ == "enemy")
            {
                if (!enemy_box_canvas.enabled)
                {
                    enemy_box_canvas.enabled = true;
                    vief.images_and_text_at_canvas_fade(enemy_box_canvas, 1f, 2f);
                }
                float te_delay = vief.type_text_for_canvas_bubble(enemy_box_canvas, next_text);
                int delay_len = Mathf.RoundToInt(te_delay * next_text.Length + 2); // depending on type_text delay and lettercount we delay output so text coud be read
                yield return new WaitForSeconds(delay_len);
            }
        }
        yield return new WaitForSeconds(2);
        current_game_state.scene_stt = scene_state.states.level_progress;
    }

    void questions_init()
    {
        for(int i=0; i<=3;i++)
        {
            question q = math_question_builder.create_math_question(10 + i);
            if (debug == 1)
            {
                q.question_text += ":" + q.answers[0].txt; //we will show right answer in question for debug reasons
            }
            current_game_state.current_level.questions.Add(q);
        }
        current_game_state.current_level.questions.Reverse();
    }

    public void run_game_level(scene_state st)
    {
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
        float te_delay = vief.type_text_for_canvas_bubble(player_bubble, "I think it is " + input_text);
        int delay_len = Mathf.RoundToInt(te_delay * input_text.Length + 2); // depending on type_text delay and lettercount we delay output so text coud be read
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
        te_delay = vief.type_text_for_canvas_bubble(enemy_box_canvas, text);
        delay_len = Mathf.RoundToInt(te_delay * text.Length + 2); // depending on type_text delay and lettercount we delay output so text coud be read
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
        
    }

    IEnumerator show_question(question q)
    {
        yield return null;
        current_game_state.current_level.current_question = q;
        float te_delay = vief.type_text_for_canvas_bubble(enemy_box_canvas, q.question_text);
        //int delay_len = Mathf.RoundToInt(te_delay * q.question_text.Length + 2); // depending on type_text delay and lettercount we delay output so text coud be read
        //yield return new WaitForSeconds(delay_len);

    }

    float timer;
    void Update()
    {
        timer += Time.deltaTime;
        if (timer>0.300f)
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
                        //level ends here
                    }
                }
            }
        }

    }
}
