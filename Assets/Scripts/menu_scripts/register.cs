using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class register : MonoBehaviour
{
    public InputField loginbox;
    public InputField pwd1box;
    public InputField pwd2box;
    public init menu_init;
    public void btn_back_click() // hide reg, show login
    {
        menu_init.login.SetActive(true);
        menu_init.register.SetActive(false);
    }

    public void btn_reg_click()
    {
        if ((pwd1box.text != "") && (loginbox.text != ""))
        {
            if (pwd1box.text == pwd2box.text) // check if passwords not equal
            {
                string login = loginbox.text;
                string pwd = pwd1box.text;
                if (!db_helper_login.check_user_by_login(login)) // check if user already exists in db
                {
                    if (db_helper_login.reg_new_user(login, pwd)) // create new user, returns true if user created
                    {
                        menu_init.show_alert("User:" + login + " created.");
                        btn_back_click(); // hide reg show login

                    }
                    else
                    {
                        menu_init.show_alert("Unable to create new user.");
                    }

                }
                else
                {
                    menu_init.show_alert("User " + login + " already exists in db");
                }

            }
            else
            {
                menu_init.show_alert("Enter the same pasword in both password boxes");
            }
        } else
        {
            menu_init.show_alert("Enter username and password.");
        }
    }
}
