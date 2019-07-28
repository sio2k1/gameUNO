using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class main_menu_script : MonoBehaviour
{
    public void btn_play_onClick()
    {
        //Basically  loads a next scene from scenes list
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    public void btn_quit_onClick()
    {
        //exit app
        Application.Quit();
    }
}
