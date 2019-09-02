using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//this script provide methods to operate objects, which displayed on map.

public class map_objects : MonoBehaviour
{
    public GameObject map; //link to map
    public visual_effects vief; // link to visual effect
    public GameObject player; // player on map
    private float map_appear_in = 2f; // duration of fading
    public void map_visibility(bool show) // change map visibility state
    {
        if (show) //show
        {
            map.SetActive(show);
        } else
        {
            StartCoroutine(setactive_after(map_appear_in, show)); // hide map in map_appear_in seconds
        }
        foreach (Image sr in map.GetComponentsInChildren<Image>(true)) //fade_in ot fade_out all images at map
        {
            if (show) //show
            {
               StartCoroutine(vief.FadeTo(1f, map_appear_in, sr));
            } else //hide
            {
               StartCoroutine(vief.FadeTo(0f, map_appear_in, sr));
            }
        }
    }

    private void Start()
    {
        map.SetActive(false); //hide evrything related to map on start
        foreach (Image sr in map.GetComponentsInChildren<Image>(true))
        {  
            StartCoroutine(vief.FadeTo(0f, 0f, sr));
        }
    }

    public IEnumerator move_hero(level lvl) // move a hero at map, based on selected level
    {
        yield return new WaitForSeconds(0.1f);
        if (lvl.name.ToLower()== levelnames.West)
        {
            player_setposition(95, 461, 1087, 79);
            player_set_rotation_y(180);
        }
        else if (lvl.name.ToLower() == levelnames.East)
        {
            player_setposition(1056, 369, 126, 171);
            player_set_rotation_y(0);
        }
        else if (lvl.name.ToLower() == levelnames.North)
        {
            player_setposition(467, 34, 748, 527);
            player_set_rotation_y(180);
        } 
    }

    public void player_reset_pos() //set player pos to crossroads at game start
    {
        player_setposition(597, 388, 585, 152);
        player_set_rotation_y(180);
    }

    public void player_set_rotation_y(int y) // rotate player (y)
    {
        RectTransform rt = player.GetComponent<RectTransform>();
        rt.rotation = new Quaternion(0, y, 0, 0);
    }
    public void player_setposition(int l, int t, int r, int b) // set position of player based on top\left\bottom\right
    {
        RectTransform rt = player.GetComponent<RectTransform>();
        rect_transform_ext.SetTop(rt, t);
        rect_transform_ext.SetLeft(rt, l);
        rect_transform_ext.SetRight(rt, r);
        rect_transform_ext.SetBottom(rt, b);
    }

    IEnumerator setactive_after(float time, bool show) // change actyvity of map after defined time (to let it fade)
    {
        //yield return new WaitForSeconds(0.1f);
        yield return new WaitForSeconds(time + 0.5f);
        map.SetActive(show);
    }

}
