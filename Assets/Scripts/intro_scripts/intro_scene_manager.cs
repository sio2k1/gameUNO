﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Data;
//using System.Linq;

// This is the introscene manager, handles a scene logic



public class intro_scene_manager : MonoBehaviour
{
    //intro scene manager is used to control scene in time
    //public Image story_img;
    //public Text story_text;
    public GameObject knight_to_spawn;

    public Canvas player_bubble;
    public Canvas narrator_bubble;
    public Canvas input_cvns;

    public visual_effects vief;

    private GameObject player;
    private DataTable storyline;
    private bool scroll_bg_ = false;
    //private bool show_fog_ = false;





    private scene_state stt = new scene_state();

    //private string debug_short_story = "";
    private string debug_short_story = " and order=1.0"; //""; //leave empty for release, using for debug purposes to make storylines shorter
    public bool ism_scroll_bg()
    {
        return scroll_bg_;
        
    }





    IEnumerator player_talk_wrong_dest()
    {
        yield return new WaitForSeconds(1);
        vief.type_text_for_canvas_bubble(player_bubble, "Wrong destination.");
        yield return new WaitForSeconds(2);
        StartCoroutine(player_talk_dest_choise());

    }

    IEnumerator player_talk_dest_choise()
    {
        yield return new WaitForSeconds(0.1f);
        foreach (DataRow r in storyline.Select("id=5")) //we starting from story text in narrator box, foreach row in table we display text with delay
        {
            string next_text = r["ctext"].ToString(); //getting a text record from table (field = ctext)
            float te_delay = vief.type_text_for_canvas_bubble(player_bubble, next_text);
            int delay_len = Mathf.RoundToInt(te_delay * next_text.Length + 2); // depending on type_text delay and lettercount we delay output so text coud be read
            yield return new WaitForSeconds(delay_len);
        }
    }


    IEnumerator inp_handler(string input_text)
    {
        yield return new WaitForSeconds(0.1f);
        if (stt.scene_stt == scene_state.states.wait_for_dest_cmd) //if scenestate 
        {
            if (input_text.ToLower() == "north")
            {
                vief.type_text_for_canvas_bubble(player_bubble, "Going " + input_text.ToLower() + "...");
                StartCoroutine(vief.scene_fading(2, 4, 2, player_bubble));
                yield return new WaitForSeconds(2 + 4 + 2);
                StartCoroutine(player_talk_dest_choise());
            }
            else
            if (input_text.ToLower() == "west")
            {
                vief.type_text_for_canvas_bubble(player_bubble, "Going " + input_text.ToLower() + "...");
                StartCoroutine(vief.scene_fading(2, 4, 2, player_bubble));
                yield return new WaitForSeconds(2 + 4 + 2);
                StartCoroutine(player_talk_dest_choise());
            }
            else
            if (input_text.ToLower() == "east")
            {
                vief.type_text_for_canvas_bubble(player_bubble, "Going " + input_text.ToLower() + "...");
                StartCoroutine(vief.scene_fading(2, 4, 2, player_bubble));
                yield return new WaitForSeconds(2 + 4 + 2);
                StartCoroutine(player_talk_dest_choise());
            }
            else
            {
                StartCoroutine(player_talk_wrong_dest());

            }

        }
    }


    public void button_handler(string input_text)
    {
        StartCoroutine(inp_handler(input_text));
    }
    



    private void Update()
    {
       
        try
        {
            //sticking text bubble to player
            player_bubble.transform.position = new Vector3(player.transform.position.x - 210, player.transform.position.y + 150);
        }
        catch { }
    }

    private void Start()
    {
        storyline = MyDataBase.GetTable("SELECT * from storyline"); //load storyline from db on load;
        vief.images_and_text_at_canvas_fade(narrator_bubble, 0f, 0f);//hide image and text on start
        vief.images_and_text_at_canvas_fade(input_cvns, 0f, 0f);
        input_cvns.enabled = false;

        StartCoroutine(introstory()); // starting a storyline routine so we can use delay in scripts
    }



    IEnumerator introstory()
    {

        scroll_bg_ = true; //setting up the background scrolling enabled on start
        vief.show_fog_ = true;
        vief.images_and_text_at_canvas_fade(narrator_bubble, 1f, 2f);// fadein story box and text
        yield return new WaitForSeconds(1);

        foreach (DataRow r in storyline.Select("scene='intro'"+debug_short_story, "order ASC")) //we starting from story text in narrator box, foreach row in table we display text with delay
        {
            string next_text = r["ctext"].ToString(); //getting a text record from table (field = ctext)
            float te_delay = vief.type_text_for_canvas_bubble(narrator_bubble, next_text);
            int delay_len = Mathf.RoundToInt(te_delay * next_text.Length + 2); // depending on type_text delay and lettercount we delay output so text coud be read
            yield return new WaitForSeconds(delay_len);
        }

        vief.images_and_text_at_canvas_fade(narrator_bubble, 0f, 3f); // after no story records in sql table left, we fadeout narrator box and text
        vief.show_fog_ = false; //stop fog
        player=Instantiate(knight_to_spawn, new Vector3(455, -120, 0), Quaternion.identity, GameObject.Find("bigForest").transform); // player spawn so camera can catch hero nicely and smoothly
        yield return new WaitForSeconds(7); //weiting a bit before stop the camera. Smoth camera stop is seted in background script

        scroll_bg_ = false; //stop bg scrolling
        StartCoroutine(player_intro_talk());
    }

    IEnumerator player_intro_talk()
    {
        yield return new WaitForSeconds(1);
        //player.GetComponent<Animator>().SetBool("walk", true);
        vief.images_and_text_at_canvas_fade(player_bubble, 0f, 0f);
        player_bubble.GetComponent<Canvas>().enabled = true;
        vief.images_and_text_at_canvas_fade(player_bubble, 1f, 2f);
        

        foreach (DataRow r in storyline.Select("scene='intro_hero'" + debug_short_story, "order ASC")) //we starting from story text in narrator box, foreach row in table we display text with delay
        {
            string next_text = r["ctext"].ToString(); //getting a text record from table (field = ctext)
            float te_delay = vief.type_text_for_canvas_bubble(player_bubble, next_text);
            int delay_len = Mathf.RoundToInt(te_delay * next_text.Length + 2); // depending on type_text delay and lettercount we delay output so text coud be read
            yield return new WaitForSeconds(delay_len);
        }
        stt.scene_stt = scene_state.states.wait_for_dest_cmd; //change scene state so we can assept destination commnds;
        vief.images_and_text_at_canvas_fade(input_cvns, 1f, 2f);
        input_cvns.GetComponentInChildren<InputField>().ActivateInputField();
        //type_text_for_canvas_bubble(player_bubble, "sdsadsafsfdsfdfsdf");
    }



}


