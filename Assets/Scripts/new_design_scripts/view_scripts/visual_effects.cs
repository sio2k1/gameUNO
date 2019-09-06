using System.Collections;
using System.Collections.Generic;
using UB.Simple2dWeatherEffects.Standard;
using UnityEngine;
using UnityEngine.UI;

// we use this class to create visual effects, like fog, fading etc..

public class visual_effects : MonoBehaviour
{
    public bool show_fog_; //show or hige fog (smooth fog dissapearing involved)
    private float fog_stop_ctr = 0; //counter for fog dissapear
    private float fog_stop_in_sec = 7; // duration of fog fading out
    private float fog_start_density = 0; 
    public void ShowFog(bool show_fog) //show\hide fog
    {
        show_fog_ = show_fog;
    }
    public void fog_handler() // we put this to update() to handle fog dissapearing incase show_fog_==false
    {
        D2FogsNoiseTexPE fog = GameObject.FindObjectOfType<D2FogsNoiseTexPE>();
        if (fog != null) // no fog script involved, do nothing.
        {
            if (!show_fog_) // if var changed to false, we decrease fog density to 0 in time
            {
                if (fog_stop_ctr < fog_stop_in_sec) //counter reach
                {
                    fog_stop_ctr += Time.deltaTime;
                    fog.Density = Mathf.Lerp(fog_start_density, 0, fog_stop_ctr / fog_stop_in_sec); //lefping the density of fog
                }
            }
        }
    }
    private void Start() //init
    {
        D2FogsNoiseTexPE fog = GameObject.FindObjectOfType<D2FogsNoiseTexPE>();
        if (fog != null)
        {
            fog_start_density = fog.Density;// init acutal fog dens at start so we can .lerp it later
        }
    }
    public IEnumerator FadeTo(float aValue, float aTime, SpriteRenderer sr) //using this to set alpha of SpriteRenderer
    {
        float alpha = sr.color.a;
        for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / aTime) // we use formula, as lerp assepts 0 to 1 float values, so we can convert time
        {
            Color newColor = new Color(1, 1, 1, Mathf.Lerp(alpha, aValue, t));
            sr.color = newColor;
            yield return null;
        }
    }

    public IEnumerator FadeTo(float aValue, float aTime, Image sr) //using this to set alpha of Image
    {
        //yield return null;
        float alpha = sr.color.a;

        if (aTime != 0f)
        {
            for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / aTime) // time to 0 to 1 float convert
            {
                Color newColor = new Color(1, 1, 1, Mathf.Lerp(alpha, aValue, t));
                sr.color = newColor;
                yield return null;
            }
        } else // if time is 0, we not lerping 
        {
            Color newColor = new Color(1, 1, 1, aValue);
            sr.color = newColor;
            yield return null;
        }
    }



    public IEnumerator scene_fade_to(float alpha) // using this to fade all SpriteRenderer, Canvas and Text within the scene
    {
        yield return new WaitForSeconds(0.1f);
        foreach (SpriteRenderer s in GameObject.FindObjectsOfType<SpriteRenderer>())
        {

            StartCoroutine(FadeTo(alpha, 3f, s)); // we use StartCoroutine to make it start in different thread, so it will fade smoothly
            //FadeTo(0f, 3f, s);
        }

        foreach (Canvas c in GameObject.FindObjectsOfType<Canvas>())
        {

            images_and_text_at_canvas_fade(c, alpha, 2f); // we use StartCoroutine to make it start in different thread, so it will fade smoothly
            //FadeTo(0f, 3f, s);
        }

        //images_and_text_at_canvas_fade(player_bubble, 0f, 2f);



    }



    public void sprite_renderer_set_alpha(SpriteRenderer sr, float alpha) // setting alpha of color of SpriteRenderer
    {

        Color newColor = new Color(1, 1, 1, alpha);
        sr.color = newColor;

    }

    /*
    public float type_text_for_canvas_bubble(Canvas cvn, string text) // type text effect for canvas, containing Text component
    {
        float res = 0;

        type_text_effect te = cvn.GetComponentInChildren<type_text_effect>();
        if (te != null) // in case we have type_effect attached to text
        {
            te.type_text(text);
            res = te.delay; //delay is calculated, based on ammount of letters in text
        }
        return res;
    }*/

    public float type_text_for_canvas_bubble_v2(Canvas cvn, string text) // type text effect for canvas, containing Text component
    {
        float res = 0;

        type_text_effect te = cvn.GetComponentInChildren<type_text_effect>();
        if (te != null) // in case we dont have type_effect attached to text
        {
            te.type_text(text);
            res = te.delay;
        }
        int delay_len = Mathf.RoundToInt(res * text.Length + 2); //calculate show of line duration bases on number of letters in text.
        return delay_len;
    }



    public void images_and_text_at_canvas_fade(Canvas cvn, float alpha, float time) //fade Image and Texc components at canvas
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

    void Update()
    {
        fog_handler(); //this is a different way, compared to "camera movement stop", fog dissapearing we r handling in this script.
    }
}
