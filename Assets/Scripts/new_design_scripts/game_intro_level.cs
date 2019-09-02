using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//this script is made to display intro scene with narration, and pass to level selection at the end
public class game_intro_level : MonoBehaviour
{
    public scene_objects scene;
    public map_objects map_obj;
    void Start() //define some start settings, hide input box, hide narration box so we can smoothly fade them in later.
    {
        scene.narrator_enabled(false);
        scene.narrator_hide();
        scene.input_hide_disable();
        StartCoroutine(introstory()); // start to display story
    }

    IEnumerator introstory()
    {
        yield return null; //its a coroutine, so we need to return some delay or null to make it work.
        global_debug_state.use_debug(); //its almost gamestart, so we set debug state incase we neeed to our game became shorter.


        //db_helper_questions.add_some_questions_to_db(2,questions_provider.diff_level.medium);

        //yield return new WaitForSeconds(2);

        //level l = new level("north");
        //scene.background_change(l);
        //questions_provider qp = new questions_provider();


        //map_obj.map_visibility(true);
        //map_obj.player_reset_pos();


        //yield return new WaitForSeconds(4);
        //map_obj.map_visibility(false);


        scene.narrator_enabled(true); //show narrator box
        scene.scroll_bg(true); //start bg scrolling
        scene.fog_start_stop(true); // use some fog
        scene.narrator_fade(1f, 2f);// fade_in story box and text
        yield return new WaitForSeconds(1); // couroutine delay float seconds
        foreach (string next_text in db_helper.intro_story_line()) // display narration line by line
        {
            float delay_len = scene.narrator_type_text(next_text);
            yield return new WaitForSeconds(delay_len); //calculate show of line duration based on number of letters in text.
        }
        scene.narrator_fade(0f, 3f);// after no story records in sql table left, we fadeout narrator box and text
        scene.fog_start_stop(false); //stop fog
        scene.player_instatinate();
        yield return new WaitForSeconds(7); //waiting a bit before stop the camera. Smoth camera stop is seted in background script
        scene.narrator_enabled(false);
        scene.scroll_bg(false); //stop bg scrolling

        StartCoroutine(player_intro_talk()); //follow to player talking part of intro
    }

    IEnumerator player_intro_talk() // player char start talking 
    {
        yield return new WaitForSeconds(1);
        scene.player_bubble_fade(0f, 0f); //set alpha to 0 so we can fade it in later
        //scene.player_bubble_fade_to_0_in_0();
        scene.player_bubble_enabled(true);
        scene.player_bubble_fade(1f, 2f); // fading text bubble of our char
        foreach (string next_text in db_helper.intro_hero_text()) // display text line by line
        {
            float delay_len = scene.player_bubble_type_text(next_text);
            yield return new WaitForSeconds(delay_len); //calculate show of line duration based on number of letters in text.
        }
        scene.input_fade(1f, 2f); // show input box
        scene.input_enabled(true);
        scene.input_activate();
        yield return new WaitForSeconds(2);
        map_obj.player_reset_pos(); // set player to crosroads and show map
        map_obj.map_visibility(true);

    }
}
