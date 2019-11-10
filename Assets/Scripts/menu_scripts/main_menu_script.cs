﻿using cmn_infrastructure;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;


//this script defines button event handlers at main menu
public class main_menu_script : MonoBehaviour
{
    public GameObject ladderboard;
    public init menu_init;
    public void btn_play_onClick()
    {
        SceneManager.LoadScene(1); //Basically loads a gamelevel scene
    }

    public void btn_ladder_onClick()
    {
        ladderboard.SetActive(true); //show up scores stats
        ladderboard.GetComponent<ladder_cl>().load_scores(); //load scores form db
    }

    public class testrecord
    {
        [JsonProperty(PropertyName = "height")]
        public int h { get; set; }
    }
    public void fb_get_(List<testrecord> obj)
    {
        foreach (var o in obj)
        {
            Debug.Log(
            o.h.ToString());
        }
    }

    public void btn_logoff_onClick() // logoff
    {

        
        
        /*
        testrecord tr = new testrecord();
        tr.h = 444;

        firebase_comm.put_object_into_path(tr, "qqq");

        
        firebase_comm.get_objects_from_path<testrecord>(null, "qqq", fb_get_);
        */
        
        
        try
        {
            menu_init.inp_login.text = ""; // clear fields
            menu_init.inp_pwd.text = "";
            user u = new user();
            db_helper_login.set_last_user(u); // reset user
            menu_init.login.SetActive(true); // show login screen
        }
        catch
        {
            Debug.Log("Logoff error");
        }
    }



    public void btn_ladder_back_onClick() // hide scores on back click
    {
        ladderboard.SetActive(false); 
    }

    public void btn_ok_alert_click()
    {
        menu_init.alert.SetActive(false); // hide allert on "ok" click
    }


    public void btn_quit_onClick() //exit app
    {
        Application.Quit();
    }
}
