using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Data;
using UB.Simple2dWeatherEffects.Standard;

// This is the introscene manager, handles a scene logic



public class intro_scene_manager : MonoBehaviour
{
    //intro scene manager is used to control scene in time
    //public Image story_img;
    //public Text story_text;
    public GameObject knight_to_spawn;

    public Canvas player_bubble;
    public Canvas narrator_bubble;

    private GameObject player;
    private DataTable storyline;
    private bool scroll_bg_ = false;
    private bool show_fog_ = false;


    private float fog_stop_ctr=0;
    private float fog_stop_in_sec = 7;
    private float fog_start_density=0;
    public bool ism_scroll_bg()
    {
        return scroll_bg_;
    }

    public void fog_handler()
    {
        D2FogsNoiseTexPE fog = GameObject.FindObjectOfType<D2FogsNoiseTexPE>();
        if (fog != null) // no fog script involved, do nothing.
        {
            if (!show_fog_) // if var changed to false, we decrease fog dencity to 0 in time
            {
                if (fog_stop_ctr < fog_stop_in_sec)
                {
                    fog_stop_ctr += Time.deltaTime;
                    fog.Density = Mathf.Lerp(fog_start_density, 0, fog_stop_ctr / fog_stop_in_sec);
                }
            }
        }
    }

    private void Awake()
    {
        storyline = MyDataBase.GetTable("SELECT * from storyline where scene='intro' order by [order] LIMIT 1"); //load storyline from db on load;
    }

    /*private void narrowbox_n_text_fade(float alpha, float time)
    {
        story_img.CrossFadeAlpha(alpha, time, false); //hide image and text on start
        story_text.CrossFadeAlpha(alpha, time, false);
    }*/


    private float type_text_for_canvas_bubble(Canvas cvn, string text)
    {
        float res = 0;
       
        type_text_effect te = cvn.GetComponentInChildren<type_text_effect>();
        if (te != null) // in case we dont have type_effect attached to text
        {
            te.type_text(text);
            res = te.delay;
        } 
        return res;
    }
    private void images_and_text_at_canvas_fade(Canvas cvn, float alpha, float time)
    {
        //new function to fade all images and texts in canvas
        foreach (var img in cvn.GetComponentsInChildren<Image>())
        {
            img.CrossFadeAlpha(alpha, time, false);
        }
        foreach (var img in cvn.GetComponentsInChildren<Text>())
        {
            img.CrossFadeAlpha(alpha, time, false);
        }
    }

    private void Update()
    {
        fog_handler(); //this is a different way, compared to camera movement stop, fog dissapearing we r handling in this script.
        try
        {
            player_bubble.transform.position = new Vector3(player.transform.position.x - 210, player.transform.position.y + 150);
        }
        catch { }
    }

    private void Start()
    {
        images_and_text_at_canvas_fade(narrator_bubble, 0f, 0f);//hide image and text on start
        //narrowbox_n_text_fade(0f, 0f); //hide image and text on start
        D2FogsNoiseTexPE fog = GameObject.FindObjectOfType<D2FogsNoiseTexPE>();
        if (fog != null)
        {
            fog_start_density = fog.Density;// init acutal fog dens at start so we can .lerp it later
        }

        

        //story_text.CrossFadeAlpha(0f, 0f, false);
        StartCoroutine(introstory()); // starting a storyline routine so we can use delay in scripts

        

    }

    IEnumerator player_intro_talk()
    {
        yield return new WaitForSeconds(1);
        //player.GetComponent<Animator>().SetBool("walk", true);
        images_and_text_at_canvas_fade(player_bubble, 0f, 0f);
        player_bubble.GetComponent<Canvas>().enabled = true;
        images_and_text_at_canvas_fade(player_bubble, 1f, 2f);
        type_text_for_canvas_bubble(player_bubble, "sdsadsafsfdsfdfsdf");
    }

    IEnumerator introstory()
    {

        scroll_bg_ = true; //setting up the background scrolling enabled on start
        show_fog_ = true;
        images_and_text_at_canvas_fade(narrator_bubble, 1f, 2f);// fadein story box and text
        //narrowbox_n_text_fade(1f, 2f);// fadein story box and text

        yield return new WaitForSeconds(1);
        
        foreach (DataRow r in storyline.Rows) //we starting from story text in narrator box, foreach row in table we display text with delay
        {
            string next_text = r["ctext"].ToString(); //getting a text record from table (field = ctext)
            float te_delay = type_text_for_canvas_bubble(narrator_bubble, next_text);
            int delay_len = Mathf.RoundToInt(te_delay * next_text.Length + 2); // depending on type_text delay and lettercount we delay output so text coud be read
            yield return new WaitForSeconds(delay_len);
            /*
            type_text_effect te = story_text.GetComponentInChildren<type_text_effect>();
            if (te!=null) // in case we dont have type_effect attached to text, we will use a basic text property
            {
                te.type_text(next_text);
                int delay_len = Mathf.RoundToInt(te.delay * next_text.Length + 2); // depending on type_text delay and lettercount we delay output so text coud be read
                yield return new WaitForSeconds(delay_len);
            } else
            {
                story_text.text = next_text;
                yield return new WaitForSeconds(8);
            }*/
        }
        images_and_text_at_canvas_fade(narrator_bubble, 0f, 3f); // after no story records in sql table left, we fadeout narrator box and text
        //narrowbox_n_text_fade(0f, 3f); // after no story records in sql table left, we fadeout narrator box and text

        show_fog_ = false; //stop fog
        player=Instantiate(knight_to_spawn, new Vector3(455, -120, 0), Quaternion.identity, GameObject.Find("bigForest").transform); // player spawn so camera can catch hero nicely and smoothly


        yield return new WaitForSeconds(7); //weiting a bit before stop the camera. Smoth camera stop is seted in background script


        scroll_bg_ = false; //stop bg scrolling
        StartCoroutine(player_intro_talk());
    }
    
    

}
