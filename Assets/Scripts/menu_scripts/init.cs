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


    public void setusername(user u) // set user after login
    {
        username.text = u.login;
    }
    private void perform_init_login() // load last saved user
    {
        try
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
            login.SetActive(true);
            //show login screen
        }
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
