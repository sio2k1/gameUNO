using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using TMPro;

//user login procedures occurs here.

public class login : MonoBehaviour
{
    public init menu_init;
    public async void btn_login_onClick() // login
    {
        menu_init.loading.SetActive(true);
        try
        {
            menu_init.userpic_vis.SetActive(false);
            menu_init.username.text = "Loading...";
            app_globals.loggined_user_fb = await db_helper_login_firebase.check_user_creds(menu_init.inp_login.text, menu_init.inp_pwd.text); //check credentials
            menu_init.setusername(app_globals.loggined_user_fb); // set username
            
           
            db_helper_common.set_setting("lastuserid", app_globals.loggined_user_fb.combinedloginhash); // save user in local db for autologin
        }
        catch
        {
            Debug.Log("Cannot check username");
        }
        menu_init.loading.SetActive(false);

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
        //TODO: Camerainit 
        menu_init.GetComponent<register>().init_camera();
        //List<TMP_InputField> l2 = menu_init.register.GetComponentsInChildren<TMP_InputField>().ToList();
        menu_init.register.GetComponentsInChildren<TMP_InputField>().ToList().ForEach(l => {
            l.text = "";
        }); //clear input fields
    }
}
