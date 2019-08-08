using System.Collections;
using System.Collections.Generic;
using UB.Simple2dWeatherEffects.Standard;
using UnityEngine;
using UnityEngine.UI;

public class visual_effects : MonoBehaviour
{
    public bool show_fog_;
    private float fog_stop_ctr = 0;
    private float fog_stop_in_sec = 7;
    private float fog_start_density = 0;
    public void ShowFog(bool show_fog)
    {
        show_fog_ = show_fog;
    }
    public void fog_handler()
    {
        D2FogsNoiseTexPE fog = GameObject.FindObjectOfType<D2FogsNoiseTexPE>();
        if (fog != null) // no fog script involved, do nothing.
        {
            if (!show_fog_) // if var changed to false, we decrease fog density to 0 in time
            {
                if (fog_stop_ctr < fog_stop_in_sec)
                {
                    fog_stop_ctr += Time.deltaTime;
                    fog.Density = Mathf.Lerp(fog_start_density, 0, fog_stop_ctr / fog_stop_in_sec);
                }
            }
        }
    }
    private void Start()
    {
        D2FogsNoiseTexPE fog = GameObject.FindObjectOfType<D2FogsNoiseTexPE>();
        if (fog != null)
        {
            fog_start_density = fog.Density;// init acutal fog dens at start so we can .lerp it later
        }
    }
    public IEnumerator FadeTo(float aValue, float aTime, SpriteRenderer sr)
    {
        float alpha = sr.color.a;
        for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / aTime)
        {
            Color newColor = new Color(1, 1, 1, Mathf.Lerp(alpha, aValue, t));
            sr.color = newColor;
            yield return null;
        }
    }



    public IEnumerator scene_fade_to_0()
    {
        yield return new WaitForSeconds(0.1f);
        foreach (SpriteRenderer s in GameObject.FindObjectsOfType<SpriteRenderer>())
        {

            StartCoroutine(FadeTo(0f, 3f, s));
            //FadeTo(0f, 3f, s);
        }

        foreach (Canvas c in GameObject.FindObjectsOfType<Canvas>())
        {

            images_and_text_at_canvas_fade(c, 0f, 2f);
            //FadeTo(0f, 3f, s);
        }

        //images_and_text_at_canvas_fade(player_bubble, 0f, 2f);



    }


    public IEnumerator scene_fade_to_100()
    {
        yield return new WaitForSeconds(0.1f);
        foreach (SpriteRenderer s in GameObject.FindObjectsOfType<SpriteRenderer>())
        {

            StartCoroutine(FadeTo(1f, 3f, s));
            //FadeTo(0f, 3f, s);
        }

        foreach (Canvas c in GameObject.FindObjectsOfType<Canvas>())
        {

            images_and_text_at_canvas_fade(c, 1f, 2f);
            //FadeTo(0f, 3f, s);
        }

        //yield return new WaitForSeconds(last);
        //images_and_text_at_canvas_fade(player_bubble, 1f, 2f);

    }

    public void sprite_renderer_set_alpha(SpriteRenderer sr, float alpha)
    {

        Color newColor = new Color(1, 1, 1, alpha);
        sr.color = newColor;

    }

    public float type_text_for_canvas_bubble(Canvas cvn, string text)
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

    public float type_text_for_canvas_bubble_v2(Canvas cvn, string text)
    {
        float res = 0;

        type_text_effect te = cvn.GetComponentInChildren<type_text_effect>();
        if (te != null) // in case we dont have type_effect attached to text
        {
            te.type_text(text);
            res = te.delay;
        }
        int delay_len = Mathf.RoundToInt(res * text.Length + 2);
        return delay_len;
    }



    public void images_and_text_at_canvas_fade(Canvas cvn, float alpha, float time)
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
        fog_handler(); //this is a different way, compared to camera movement stop, fog dissapearing we r handling in this script.
    }
}
