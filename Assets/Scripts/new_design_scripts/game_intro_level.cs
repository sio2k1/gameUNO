using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class game_intro_level : MonoBehaviour
{
    // Start is called before the first frame update
    public scene_objects scene;
    public map_objects map_obj;
    //private string debug = "LIMIT 1";
    void Start()
    {
        scene.narrator_enabled(false);
        scene.narrator_hide();
        scene.input_hide_disable();
        StartCoroutine(introstory());
    }

    IEnumerator introstory()
    {
        yield return null;
        global_debug_state.use_debug();


        //db_helper_questions.add_some_questions_to_db();

        //yield return new WaitForSeconds(2);

        //level l = new level("north");
        //scene.background_change(l);
        //questions_provider qp = new questions_provider();


        //map_obj.map_visibility(true);
        //map_obj.player_reset_pos();


        //yield return new WaitForSeconds(4);
        //map_obj.map_visibility(false);


        scene.narrator_enabled(true);
        scene.scroll_bg(true);
        scene.fog_start_stop(true);
        scene.narrator_fade(1f, 2f);// fade_in story box and text
        yield return new WaitForSeconds(1);
        foreach (string next_text in db_helper.intro_story_line())
        {
            float delay_len = scene.narrator_type_text(next_text);
            yield return new WaitForSeconds(delay_len);
        }
        scene.narrator_fade(0f, 3f);// after no story records in sql table left, we fadeout narrator box and text
        scene.fog_start_stop(false); //stop fog
        scene.player_instatinate();
        yield return new WaitForSeconds(7); //weiting a bit before stop the camera. Smoth camera stop is seted in background script
        scene.narrator_enabled(false);
        scene.scroll_bg(false); //stop bg scrolling

        StartCoroutine(player_intro_talk()); //follow to player talking part of intro
    }

    IEnumerator player_intro_talk()
    {
        yield return new WaitForSeconds(1);
        scene.player_bubble_fade(0f, 0f); //set alph to 0 so we can fade it in later
        //scene.player_bubble_fade_to_0_in_0();
        scene.player_bubble_enabled(true);
        scene.player_bubble_fade(1f, 2f);
        foreach (string next_text in db_helper.intro_hero_text())
        {
            float delay_len = scene.player_bubble_type_text(next_text);
            yield return new WaitForSeconds(delay_len);
        }
        scene.input_fade(1f, 2f);
        scene.input_enabled(true);
        scene.input_activate();
        yield return new WaitForSeconds(1);
        map_obj.player_reset_pos();
        map_obj.map_visibility(true);

    }
}
