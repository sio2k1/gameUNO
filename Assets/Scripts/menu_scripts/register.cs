using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

//user registration occurs here.

public class register : MonoBehaviour
{
    public InputField loginbox;
    public InputField pwd1box;
    public InputField pwd2box;
    public init menu_init;
    public display_cam_on_img regcamera;
    public void btn_back_click() // hide reg, show login
    {
        menu_init.login.SetActive(true);
        menu_init.register.SetActive(false);
    }

    public void btn_shoot_click() // occurs on take shoot btn click
    {
        regcamera.takePhoto();
    }

    public async void btn_reg_click() // occurs on reg button click
    {
        if ((pwd1box.text != "") && (loginbox.text != ""))
        {
            if (pwd1box.text == pwd2box.text) // check if passwords not equal
            {
                string login = loginbox.text;
                string pwd = pwd1box.text; 
                if (!await db_helper_login_firebase.check_user_by_login(login)) // chech if we have user with same login in db
                {
                    
                    byte[] pic = File.ReadAllBytes(Path.Combine(Application.streamingAssetsPath, "pic.png"));//regcamera.lastcamerashoot
                    if (await db_helper_login_firebase.reg_new_user(login, pwd, pic)) // create new user, returns true if user created
                    {
                        menu_init.show_alert("User:" + login + " created."); // alert
                        btn_back_click(); // hide reg show login

                    }
                    else
                    {
                        menu_init.show_alert("Unable to create new user.");  // alert
                    }
                }
                else
                {
                    menu_init.show_alert("User '" + login + "' already exists in db");  // alert
                }

            }
            else
            {
                menu_init.show_alert("Enter the same pasword in both password boxes");  // alert
            }
        } else
        {
            menu_init.show_alert("Enter username and password.");  // alert
        }
    }
}
