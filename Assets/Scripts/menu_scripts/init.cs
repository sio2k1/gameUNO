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
    public GameObject loading;


    public void setusername(user_fb u) // set user after login
    {
        app_globals.userpic_for_mplay_taple_id = UnityEngine.Random.Range(0, 4); // this is for random picture in multiplayer list at level, should be user pic from camera though :))))
        username.text = u.login_display;

        Texture2D t = new Texture2D(200, 200);
        byte[] b2 = System.Convert.FromBase64String(u.userpic);

        t.LoadImage(b2);
        userpic.texture = t;
    }
    private async void perform_init_login() // load last saved user
    {
        try
        {
            loading.SetActive(true); // show fb query proceed screen
            string combinedhash = db_helper_common.get_setting("lastuserid"); // get last loggined user
            if (combinedhash != "") // if we have some session login saved in db
            {
                app_globals.loggined_user_fb = await db_helper_login_firebase.check_user_creds(combinedhash); //check credentials
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
        /*try
        {
            string json = db_helper_login.get_last_user();
            app_globals.loggined_user = serializer_helper.json_deserialize_object_from_string<user>(json); //deserialize to global user object
            Debug.Log(app_globals.loggined_user.id.ToString()); // dev purpose
        }
        catch
        {
            Debug.Log("Unable to get info about last user");
            app_globals.loggined_user = new user(); // clear current user
        }

        if (app_globals.loggined_user.id != -1)  // set name in menu if user found or shom login again if not
        {
            setusername(app_globals.loggined_user);
            //show menu and playername
        }
        else
        {
            
            //show login screen
        }*/

       
    }

    public void show_alert(string text)
    {
        alert.SetActive(true);
        alert_text.text = text;
    }

    void Start() // we
    {
        perform_init_login();
    }


}
