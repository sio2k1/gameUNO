using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//this script we use to control of object behavior in scene, to move objects, display or hide them etc...

public class scene_objects : MonoBehaviour
{
    public GameObject knight_to_spawn; //knight prefab
    private GameObject player; // player
    public Canvas player_bubble; //represent bubble with text for player

    public Canvas narrator_bubble; //represent narrator bubble
    public Canvas input_cvns; //represent canvas with input box

    public GameObject enemy_prefab; //prefab for spawning enemy
    private GameObject enemy; //enemy
    private Canvas enemy_box_canvas; //canvas for enemy text box

    public visual_effects vief; // link to videoeffects class

    public background_movement forest; //link to bg movement script

    public Text level_duration; // link to Text with level time duration


    public void level_duration_set(float num) // set level duration text
    {
        level_duration.text = Mathf.RoundToInt(num).ToString();
        if (level_duration.text=="0") // dont display anything if 0 (hide)
        {
            level_duration.text = "";
        }
    }
    
    public void background_change(level lvl) // setup different backgrounds for levels
    {


        foreach (SpriteRenderer sr in GameObject.Find("bigForest").GetComponentsInChildren<SpriteRenderer>())
        {
            Sprite s = Resources.Load<Sprite>("BG1");

            if (lvl.name.ToLower() == levelnames.West)
            {
                s = Resources.Load<Sprite>("BG1");
            }
            if (lvl.name.ToLower() == levelnames.East)
            {
                s = Resources.Load<Sprite>("BG2");
            }
            if (lvl.name.ToLower() == levelnames.North)
            {
                s = Resources.Load<Sprite>("BG3");
            }

            sr.sprite = s;
        }

    }

    public void change_enemy_model() // setup different enemy model (by switchng animation) for level randomly
    {
        Animator anim = enemy.GetComponent<Animator>();
        anim.speed = 0.2f;
        int rnd = Random.Range(1, 4);
        anim.SetInteger("anim_switcher", rnd);
    }
    
    public void narrator_hide() // set narrator alpha to 0 in 0 sec
    {
        vief.images_and_text_at_canvas_fade(narrator_bubble, 0f, 0f);//hide image and text on start
    }

    public void narrator_enabled(bool enabled) // set narrator activity
    {
        narrator_bubble.enabled = enabled;
    }

    public float narrator_type_text(string text) // display text in narration box
    {
        return vief.type_text_for_canvas_bubble_v2(narrator_bubble,text);
    }
    public void narrator_fade(float alpha, float time) //
    {
        vief.images_and_text_at_canvas_fade(narrator_bubble, alpha, time); //hide image and text on start
    }







    public void player_instatinate()
    {
        if (player == null)
        {
            player = Instantiate(knight_to_spawn, new Vector3(455, -120, 0), Quaternion.identity, GameObject.Find("bigForest").transform); // player spawn so camera can catch hero nicely and smoothly
        }
    }

    public float player_bubble_type_text(string text)
    {
        return vief.type_text_for_canvas_bubble_v2(player_bubble, text);
    }

    /*public void player_bubble_fade_to_0_in_0()
    {
        vief.images_and_text_at_canvas_fade(player_bubble, 0f, 0f);//hide image and text on start
    }*/

    public void player_bubble_enabled(bool enabled)
    {
        player_bubble.enabled = enabled;
    }

    public bool player_bubble_is_enabled()
    {
        return player_bubble.enabled; 
    }


    public void player_bubble_fade(float alpha, float time)
    {
        vief.images_and_text_at_canvas_fade(player_bubble, alpha, time);//hide image and text on start
    }




    public void input_enabled(bool enabled)
    {
        input_cvns.enabled = enabled;
    }


    public void input_fade(float alpha, float time)
    {
        vief.images_and_text_at_canvas_fade(input_cvns, alpha, time);//hide image and text on start
    }

    public void input_activate()
    {
        input_cvns.GetComponentInChildren<InputField>().ActivateInputField();
    }



    public void fog_start_stop(bool show_fog)
    {
        vief.show_fog_ = show_fog;
    }


    public void input_hide_disable()
    {
        vief.images_and_text_at_canvas_fade(input_cvns, 0f, 0f);
        input_cvns.enabled = false;
    }

    public void scroll_bg(bool scroll)
    {
        forest.scrollbg = scroll;
    }

    public void scene_fade_to(float alpha)
    {
        StartCoroutine(vief.scene_fade_to(alpha));
    }



    public void level_enemy_instatinate()
    {
        if (enemy == null)
        {
            enemy = Instantiate(enemy_prefab, new Vector3(-200, -70, 0), Quaternion.identity, GameObject.Find("bigForest").transform); // player spawn so camera can catch hero nicely and smoothly
            enemy_box_canvas = enemy.GetComponentInChildren<Canvas>();
        }
        vief.sprite_renderer_set_alpha(enemy.GetComponent<SpriteRenderer>(), 0); //init enemy sprite with 0 alpha
    }

    public void level_west_enemy_enabled(bool enabled)
    {
        enemy_box_canvas.enabled = enabled;
    }

    public void level_west_enemy_bubble_fade(float alpha, float time)
    {
        vief.images_and_text_at_canvas_fade(enemy_box_canvas, alpha, time);
    }

    public bool level_west_enemy_bubble_is_enabled()
    {
        return enemy_box_canvas.enabled;
    }

    public float level_west_enemy_bubble_type_text(string text)
    {
        return vief.type_text_for_canvas_bubble_v2(enemy_box_canvas, text);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        try
        {
            //sticking text bubble to player
            player_bubble.transform.position = new Vector3(player.transform.position.x - 210, player.transform.position.y + 150);
        }
        catch { }
    }
}
