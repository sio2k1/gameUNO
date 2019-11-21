using cmn_infrastructure;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;
using System.IO;
using System;
using UnityEngine.Android;


//this script defines button event handlers at main menu
public class main_menu_script : MonoBehaviour
{
    public GameObject ladderboard;
    public init menu_init;

    
    public RawImage cam_out; // camera output (user pic)
    public async void btn_play_onClick() // start new game
    {

        menu_init.loading.SetActive(true); // show loading screen
        try
        {
            current_mgame.curr_mgame = await mgame_manager.get_or_start_new_multiplayer_game(app_globals.loggined_user_fb.key, current_mgame.curr_mgame.key); // get new or existing game from firegase
            if (current_mgame.curr_mgame.key != "") // if got a game
            {
                //start new game here
                SceneManager.LoadScene(1); //Basically loads a gamelevel scene                             
            }
        }
        catch (Exception e)
        {
            
            Debug.Log("Can't start new game:" + e.Message);
        }
        menu_init.loading.SetActive(false);
    }

    public void btn_ladder_onClick()
    {
        ladderboard.SetActive(true); //show up scores stats
        ladderboard.GetComponent<ladder_cl>().load_scores(); //load scores form db
    }



    private void Start()
    {
        //TODO: debug state in menu
        //global_debug_state.use_debug();

        #if UNITY_ANDROID
        if (!Permission.HasUserAuthorizedPermission(Permission.Camera)) //permission request for camera on start
        {
            Permission.RequestUserPermission(Permission.Camera);
        }
        #endif
    }


    public void btn_logoff_onClick() // logoff
    {
        try
        {
            menu_init.inp_login.text = ""; // clear fields
            menu_init.inp_pwd.text = "";
            app_globals.loggined_user_fb = new user_fb();
            db_helper_common.set_setting("lastuserid", "");
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
