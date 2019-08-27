using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class scene_objects : MonoBehaviour
{
    public GameObject knight_to_spawn;
    private GameObject player;
    public Canvas player_bubble;

    public Canvas narrator_bubble;
    public Canvas input_cvns;

    public GameObject enemy_prefab;
    private GameObject enemy;
    private Canvas enemy_box_canvas;

    public visual_effects vief;

    public background_movement forest;

    
    public void background_change(level lvl)
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

    public void change_enemy_model()
    {
        
        //anim.SetInteger("anim_switcher", 2);
        enemy_res_anim era = GameObject.FindObjectOfType<enemy_res_anim>();
        Animator anim = enemy.GetComponent<Animator>();
        anim.speed = 0.2f;
        int rnd = Random.Range(1, 3);
        anim.SetInteger("anim_switcher", rnd);
        /*if (rnd==1)
        {
            enemy.GetComponentsInChildren<SpriteRenderer>()[0].sprite = era.spr_t1;   
        }*/
    }
    
    public void narrator_hide()
    {
        vief.images_and_text_at_canvas_fade(narrator_bubble, 0f, 0f);//hide image and text on start
    }

    public void narrator_enabled(bool enabled)
    {
        narrator_bubble.enabled = enabled;
        //level_west_enemy_instatinate();
        //level_west_enemy_enabled(true);
        

    }

    public float narrator_type_text(string text)
    {
        return vief.type_text_for_canvas_bubble_v2(narrator_bubble,text);
    }
    public void narrator_fade(float alpha, float time)
    {
        vief.images_and_text_at_canvas_fade(narrator_bubble, alpha, time);//hide image and text on start
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
