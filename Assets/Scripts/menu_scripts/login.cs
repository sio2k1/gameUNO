using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class login : MonoBehaviour
{
    public init menu_init;
    public void btn_login_onClick() // login
    {
        try
        {
            app_globals.loggined_user = db_helper_login.check_user_creds(menu_init.inp_login.text, menu_init.inp_pwd.text); //check credentials
            menu_init.setusername(app_globals.loggined_user); // set username in main menu
            db_helper_login.set_last_user(app_globals.loggined_user); // save last user in db
        }
        catch
        {
            Debug.Log("Cannot check username");
        }

        if (app_globals.loggined_user.id != -1)
        {
            menu_init.login.SetActive(false); // hide login screen
        }
        else
        {
            menu_init.show_alert("Incorrect creds"); // show login and alert again if user not found
            Debug.Log("Incorrect creds");
        }
    }

    public void btn_reg_click()
    {
        menu_init.register.SetActive(true); // show register user menu
    }
}
