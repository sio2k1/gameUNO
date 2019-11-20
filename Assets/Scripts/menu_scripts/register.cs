using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

//user registration occurs here.

public class register : MonoBehaviour
{
    public TMP_InputField loginbox_tmp;
    public TMP_InputField pwd1box_tmp;
    public TMP_InputField pwd2box_tmp;
    //public InputField loginbox;
    //public InputField pwd1box;
    //public InputField pwd2box;
    public init menu_init;
    public display_cam_on_img regcamera;
    public void btn_back_click() // hide reg, show login
    {
        regcamera.stopCamera();
        menu_init.login.SetActive(true);
        menu_init.register.SetActive(false);
    }

    public void init_camera()
    {
        regcamera.startCamera();
    }

    public void btn_shoot_click() // occurs on take shoot btn click
    {
        regcamera.takePhoto();
    }

    public async void btn_reg_click() // occurs on reg button click
    {
        string login = loginbox_tmp.text;
        string pwd = pwd1box_tmp.text;
        if ((pwd != "") && (login != ""))
        {
            if (pwd1box_tmp.text == pwd2box_tmp.text) // check if passwords not equal
            {
                //string login = loginbox.text;
                
                if (!await db_helper_login_firebase.check_user_by_login(login)) // chech if we have user with same login in db
                {

                    //byte[] pic = File.ReadAllBytes(Path.Combine(Application.streamingAssetsPath, "pic.png"));//regcamera.lastcamerashoot
                    byte[] pic = regcamera.lastcamerashoot;
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
