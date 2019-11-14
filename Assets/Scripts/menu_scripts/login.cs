using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

//user login procedures occurs here.

public class login : MonoBehaviour
{
    public init menu_init;
    public async void btn_login_onClick() // login
    {
        try
        {
            app_globals.loggined_user_fb = await db_helper_login_firebase.check_user_creds(menu_init.inp_login.text, menu_init.inp_pwd.text); //check credentials
            menu_init.setusername(app_globals.loggined_user_fb); // 
            db_helper_common.set_setting("lastuserid", app_globals.loggined_user_fb.combinedloginhash); // save user in local db for autologin
        }
        catch
        {
            Debug.Log("Cannot check username");
        }

        if (app_globals.loggined_user_fb.key != "") // good creds
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

        List<InputField> l2 = menu_init.register.GetComponentsInChildren<InputField>().ToList();
        menu_init.register.GetComponentsInChildren<InputField>().ToList().ForEach(l => {
            l.text = "";
        }); //clear input fields
    }
}
