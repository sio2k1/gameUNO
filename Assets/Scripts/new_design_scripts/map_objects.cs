using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class map_objects : MonoBehaviour
{

    public GameObject map;
    public visual_effects vief;
    public GameObject player;
    private float map_appear_in = 2f;
    public void map_visibility(bool show)
    {
        if (show) //show
        {
            map.SetActive(show);
        } else
        {
            secactive_after(map_appear_in, show);
        }
        foreach (Image sr in map.GetComponentsInChildren<Image>(true))
        {
            //map.SetActive(show);
            if (show) //show
            {
               StartCoroutine(vief.FadeTo(1f, map_appear_in, sr));
                //Debug.Log(sr.name);
            } else //hide
            {
                
               StartCoroutine(vief.FadeTo(0f, map_appear_in, sr));
                //secactive_after(3f, show);
            }
        }
    }

    private void Start()
    {
        map.SetActive(false);


        foreach (Image sr in map.GetComponentsInChildren<Image>(true))
        {
            
                
            StartCoroutine(vief.FadeTo(0f, 0f, sr));


        }
        

    }

    IEnumerator move_hero(level lvl)
    {
        yield return new WaitForSeconds(0.1f);
        if (lvl.name.ToLower()=="west")
        {
            player_setposition(95, 461, 1087, 79);
            player_set_rotation_y(180);
        }
        else if (lvl.name.ToLower() == "east")
        {
            player_setposition(95, 461, 1087, 79);
            player_set_rotation_y(0);
        }
        else if (lvl.name.ToLower() == "north")
        {
            player_setposition(467, 34, 748, 527);
            player_set_rotation_y(180);
        } 


    }


    public void player_reset_pos()
    {
        player_setposition(597, 388, 585, 152);
        player_set_rotation_y(180);
    }

    public void player_set_rotation_y(int y)
    {
        RectTransform rt = player.GetComponent<RectTransform>();
        rt.rotation = new Quaternion(0, y, 0, 0);
    }
    public void player_setposition(int l, int t, int r, int b)
    {
        RectTransform rt = player.GetComponent<RectTransform>();
        rect_transform_ext.SetTop(rt,t);
        rect_transform_ext.SetLeft(rt,l);
        rect_transform_ext.SetRight(rt, r);
        rect_transform_ext.SetBottom(rt, b);

    }

    IEnumerator secactive_after(float time, bool show)
    {
        yield return new WaitForSeconds(time+0.1f);
        map.SetActive(show);

    }

}
