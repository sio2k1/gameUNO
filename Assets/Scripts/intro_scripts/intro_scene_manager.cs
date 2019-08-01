using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Data;
using UB.Simple2dWeatherEffects.Standard;
using System.Linq;

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

    private GameObject player;
    private DataTable storyline;
    private bool scroll_bg_ = false;
    private bool show_fog_ = false;


    private float fog_stop_ctr=0;
    private float fog_stop_in_sec = 7;
    private float fog_start_density=0;


    private scene_state stt = new scene_state();

    //private string debug_short_story = "";
    private string debug_short_story = " and order=1.0"; //""; //leave empty for release, using for debug purposes to make storylines shorter
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
        storyline = MyDataBase.GetTable("SELECT * from storyline"); //load storyline from db on load;
        //where scene='intro' order by [order] LIMIT 1
    }

    IEnumerator player_talk_wrong_dest()
    {
        yield return new WaitForSeconds(1);
        type_text_for_canvas_bubble(player_bubble, "Wrong destination.");
        yield return new WaitForSeconds(2);
        StartCoroutine(player_talk_dest_choise());

    }

    IEnumerator player_talk_dest_choise()
    {
        yield return new WaitForSeconds(0.1f);
        foreach (DataRow r in storyline.Select("id=5")) //we starting from story text in narrator box, foreach row in table we display text with delay
        {
            string next_text = r["ctext"].ToString(); //getting a text record from table (field = ctext)
            float te_delay = type_text_for_canvas_bubble(player_bubble, next_text);
            int delay_len = Mathf.RoundToInt(te_delay * next_text.Length + 2); // depending on type_text delay and lettercount we delay output so text coud be read
            yield return new WaitForSeconds(delay_len);
        }
    }

    IEnumerator scene_fading()
    {
        yield return new WaitForSeconds(2f);
        foreach (SpriteRenderer s in GameObject.FindObjectsOfType<SpriteRenderer>())
        {

            StartCoroutine(FadeTo(0f, 3f, s));
            //FadeTo(0f, 3f, s);
        }

        images_and_text_at_canvas_fade(player_bubble, 0f, 2f);

        yield return new WaitForSeconds(4);

        foreach (SpriteRenderer s in GameObject.FindObjectsOfType<SpriteRenderer>())
        {

            StartCoroutine(FadeTo(1f, 3f, s));
            //FadeTo(0f, 3f, s);
        }
        yield return new WaitForSeconds(2);
        images_and_text_at_canvas_fade(player_bubble, 1f, 2f);
        StartCoroutine(player_talk_dest_choise());
    }

    IEnumerator FadeTo(float aValue, float aTime, SpriteRenderer sr)
    {
        float alpha = sr.color.a;
        for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / aTime)
        {
            Color newColor = new Color(1, 1, 1, Mathf.Lerp(alpha, aValue, t));
            sr.color = newColor;
            yield return null;
        }
    }

    public void button_handler(string input_text)
    {
        if (stt.scene_stt==scene_state.states.wait_for_dest_cmd) //if scenestate 
        {
            if (input_text.ToLower()=="north")
            {
                type_text_for_canvas_bubble(player_bubble, "Going "+ input_text.ToLower() + "...");
                StartCoroutine(scene_fading());
            } else
            if (input_text.ToLower() == "west")
            {
                type_text_for_canvas_bubble(player_bubble, "Going " + input_text.ToLower() + "...");
                StartCoroutine(scene_fading());
            } else
            if (input_text.ToLower() == "east")
            {
                type_text_for_canvas_bubble(player_bubble, "Going " + input_text.ToLower() + "...");
                StartCoroutine(scene_fading());
            } else
            {
                StartCoroutine(player_talk_wrong_dest());
            }

        }
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
        if (alpha>0)
        {
            cvn.enabled = true;
        }

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
        images_and_text_at_canvas_fade(input_cvns, 0f, 0f);
        input_cvns.enabled = false;

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
        foreach (DataRow r in storyline.Select("scene='intro_hero'"+debug_short_story, "order ASC")) //we starting from story text in narrator box, foreach row in table we display text with delay
        {
            string next_text = r["ctext"].ToString(); //getting a text record from table (field = ctext)
            float te_delay = type_text_for_canvas_bubble(player_bubble, next_text);
            int delay_len = Mathf.RoundToInt(te_delay * next_text.Length + 2); // depending on type_text delay and lettercount we delay output so text coud be read
            yield return new WaitForSeconds(delay_len);
        }
        stt.scene_stt = scene_state.states.wait_for_dest_cmd; //change scene state so we can assept destination commnds;
        images_and_text_at_canvas_fade(input_cvns, 1f, 2f);
        //type_text_for_canvas_bubble(player_bubble, "sdsadsafsfdsfdfsdf");
    }

    IEnumerator introstory()
    {

        scroll_bg_ = true; //setting up the background scrolling enabled on start
        show_fog_ = true;
        images_and_text_at_canvas_fade(narrator_bubble, 1f, 2f);// fadein story box and text
        //narrowbox_n_text_fade(1f, 2f);// fadein story box and text

        yield return new WaitForSeconds(1);

        //DataRow[] results = ;
        foreach (DataRow r in storyline.Select("scene='intro'"+debug_short_story, "order ASC")) //we starting from story text in narrator box, foreach row in table we display text with delay
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
    
    public class scene_state
    {
        public enum states
        {
            intro, wait_for_dest_cmd
        }

        public states scene_stt;
        public List<level> levels = new List<level>();

        private states Scene_stt { get => scene_stt; set => scene_stt = value; }
    }

    public class level
    {
        public string name;
        public int score;
        public int time_in_sec;
    }

}


