using System.Collections;
using System.Collections.Generic;
using TMPro;
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


    public GameObject multiplayer_table;
    public GameObject mplayer_panel;
    public GameObject mtable_line_1;
    public GameObject mtable_line_2;
    public GameObject mtable_line_3;

    public void multiplayer_table_enabled(bool enabled) // set narrator activity
    {
        multiplayer_table.SetActive(enabled);
    }

    public void multiplayer_table_fade(float alpha, float time) // fade in\out input
    {
        vief.images_and_text_at_canvas_fade(multiplayer_table.GetComponentInChildren<Canvas>(), alpha, time);//hide image and text on start
    }

    public void mplayer_table_redraw(List<mgame_player> players) // redraw table with players from multiplayer
    {
        const string tag = "mplayer_list_to_del"; // tag for clearing table
        void clear_tagged(string tag_) // clear scores, remove all existing table lines at start
        {
            var tagged = GameObject.FindGameObjectsWithTag(tag_);
            foreach (GameObject go in tagged) // delete everything with tag ladder_to_del
            {
                Destroy(go);
            }
        }

        void display_scores(List<mgame_player> players_, string tag_, List<GameObject> table_entrys, GameObject panel) // display lines as prefabed table lines
        {
            clear_tagged(tag_); // clear old first

            players_.ForEach(p=> {

                if (p.playerpic>table_entrys.Count-1) // if pic out of bounds assign 0 pic
                { 
                    p.playerpic = 0; 
                } 
                
                var player_line = Instantiate(table_entrys[p.playerpic], new Vector3(0, 0, 0), Quaternion.identity, panel.transform); // place on panel to autodistribution with "vertical layout group"
                player_line.tag = tag_;

                player_line.GetComponentInChildren<TextMeshProUGUI>().text = p.player_name;
            });

        }

        /*
        players.Add(new mgame_player { player_name = "test" });
        players.Add(new mgame_player { player_name = "test" });
        players.Add(new mgame_player { player_name = "test" });
        players.Add(new mgame_player { player_name = "test" });
        players.Add(new mgame_player { player_name = "test" });
        players.Add(new mgame_player { player_name = "test" });
        */

        List<GameObject> knightpics = new List<GameObject>(); // list for different player pictures
        knightpics.Add(mtable_line_1);
        knightpics.Add(mtable_line_2);
        knightpics.Add(mtable_line_3);

        display_scores(players, tag, knightpics, mplayer_panel); // show list of players at level


    }

    public void level_duration_set(float num) // set level duration text
    {
        level_duration.text = Mathf.RoundToInt(num).ToString();
        if (level_duration.text=="0") // dont display anything if 0 (hide)
        {
            level_duration.text = "";
        } else
        {
            level_duration.text = "Time:"+ Mathf.RoundToInt(num).ToString();
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
    public void narrator_fade(float alpha, float time) // fade in and out for narration
    {
        vief.images_and_text_at_canvas_fade(narrator_bubble, alpha, time); //hide image and text on start
    }

    public void player_instatinate() // load player from prefab
    {
        if (player == null) // if there is no player obj, create new
        {
            player = Instantiate(knight_to_spawn, new Vector3(455, -120, 0), Quaternion.identity, GameObject.Find("bigForest").transform); // player spawn so camera can catch hero nicely and smoothly
        }
    }

    public float player_bubble_type_text(string text) // type text above the player
    {
        return vief.type_text_for_canvas_bubble_v2(player_bubble, text);
    }

    /*public void player_bubble_fade_to_0_in_0()
    {
        vief.images_and_text_at_canvas_fade(player_bubble, 0f, 0f);//hide image and text on start
    }*/

    public void player_bubble_enabled(bool enabled) //set activity to player bubble
    {
        player_bubble.enabled = enabled;
    }

    public bool player_bubble_is_enabled() //get activity of player bubble
    {
        return player_bubble.enabled; 
    }


    public void player_bubble_fade(float alpha, float time) // fade of player bubble
    {
        vief.images_and_text_at_canvas_fade(player_bubble, alpha, time);//hide image and text on start
    }


    public void input_enabled(bool enabled) // show\hide input
    {
        input_cvns.enabled = enabled;
    }


    public void input_fade(float alpha, float time) // fade in\out input
    {
        vief.images_and_text_at_canvas_fade(input_cvns, alpha, time);//hide image and text on start
    }

    public void input_activate() // set cursor to input field
    {
        input_cvns.GetComponentInChildren<InputField>().ActivateInputField();
    }



    public void fog_start_stop(bool show_fog) // start or stop fog
    {
        vief.show_fog_ = show_fog;
    }


    public void input_hide_disable() // hide input and set alpha to 0 in 0 sec
    {
        vief.images_and_text_at_canvas_fade(input_cvns, 0f, 0f);
        input_cvns.enabled = false;
    }

    public void scroll_bg(bool scroll) //scroll or not background
    {
        forest.scrollbg = scroll;
    }

    public void scene_fade_to(float alpha) // scene fade to 1(fadein) or 0 (fadeout)
    {
        StartCoroutine(vief.scene_fade_to(alpha));
    }



    public void level_enemy_instatinate() // create enemy from prefab
    {
        if (enemy == null) // if there is no enemy defined yes
        {
            enemy = Instantiate(enemy_prefab, new Vector3(-200, -70, 0), Quaternion.identity, GameObject.Find("bigForest").transform); // player spawn so camera can catch hero nicely and smoothly
            enemy_box_canvas = enemy.GetComponentInChildren<Canvas>();
        }
        vief.sprite_renderer_set_alpha(enemy.GetComponent<SpriteRenderer>(), 0); //init enemy sprite with 0 alpha
    }

    public void level_west_enemy_enabled(bool enabled) // setactive to enemy canvas
    {
        enemy_box_canvas.enabled = enabled;
    }

    public void level_west_enemy_bubble_fade(float alpha, float time) // fade in and out for enemy bubble
    {
        vief.images_and_text_at_canvas_fade(enemy_box_canvas, alpha, time);
    }

    public bool level_west_enemy_bubble_is_enabled() // get activity of enemy canvas
    {
        return enemy_box_canvas.enabled;
    }

    public float level_west_enemy_bubble_type_text(string text) // type text in enemy bubble
    {
        return vief.type_text_for_canvas_bubble_v2(enemy_box_canvas, text);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    private Transform lastpos;
    void Update()
    {
        try // if bupple or player not exist... dont show an error
        {
                player_bubble.transform.position = new Vector3(player.transform.position.x - 210, player.transform.position.y + 150); // stick player bubble to player 
        }
        catch { }
    }
}
