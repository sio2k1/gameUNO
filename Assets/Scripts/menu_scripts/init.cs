using cmn_infrastructure;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

//init of game, restore last login state, holds some menu GameObjects
public class init : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject login;
    public GameObject register;
    public GameObject menu;
    public TextMeshProUGUI username;
    public InputField inp_login;
    public InputField inp_pwd;
   
    public TextMeshProUGUI alert_text;
    public GameObject alert;

    public RawImage userpic;
    public GameObject userpic_vis;
    public GameObject loading;


    public void setusername(user_fb u) // set user after login
    {

        app_globals.userpic_for_mplay_table_id = UnityEngine.Random.Range(0, 4); // this is for random picture in multiplayer list at level, should be user pic from camera though :))))
        username.text = u.login_display; // set username

        if (u.userpic != "") // if we have pic, display it
        {
            Texture2D t = new Texture2D(320, 240);
            byte[] picbytes = System.Convert.FromBase64String(u.userpic);
            t.LoadImage(picbytes);
            userpic.texture = t;
            userpic_vis.SetActive(true);
        } 
    }
    private async void perform_init_login() // load last saved user
    {
        try
        {
            loading.SetActive(true); // show fb query proceed screen
            string combinedhash = db_helper_common.get_setting("lastuserid"); // get last loggined user
            if (combinedhash != "") // if we have some session login saved in db
            {

                if (app_globals.loggined_user_fb.key == "") // if its first launch
                {
                    userpic_vis.SetActive(false);
                    username.text = "Loading...";
                    app_globals.loggined_user_fb = await db_helper_login_firebase.check_user_creds(combinedhash); //check credentials
                }
                setusername(app_globals.loggined_user_fb);
            }
            loading.SetActive(false); // hide fb query proceed screen
        }
        catch
        {
            Debug.Log("Unable to get info about last user");
            app_globals.loggined_user_fb = new user_fb(); // clear current user
        }

        if (app_globals.loggined_user_fb.key != "")  // set name in menu if user found or shom login again if not
        {
            setusername(app_globals.loggined_user_fb);
            //show menu and playername
        }
        else
        {
            loading.SetActive(false);
            login.SetActive(true);
            //show login screen
        }


       
    }

    public void show_alert(string text)
    {
        alert.SetActive(true);
        alert_text.text = text;
    }

    void Start() // autologin on start
    {
        perform_init_login();
    }


}
