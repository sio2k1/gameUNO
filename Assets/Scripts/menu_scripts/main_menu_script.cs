using cmn_infrastructure;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;
using System.IO;


//this script defines button event handlers at main menu
public class main_menu_script : MonoBehaviour
{
    public GameObject ladderboard;
    public init menu_init;

    
    public RawImage cam_out;
    public void btn_play_onClick()
    {
        SceneManager.LoadScene(1); //Basically loads a gamelevel scene
    }

    public void btn_ladder_onClick()
    {
        ladderboard.SetActive(true); //show up scores stats
        ladderboard.GetComponent<ladder_cl>().load_scores(); //load scores form db
    }

    public class userrecord
    {
        [JsonProperty]
        public string username { get; set; }
        [JsonProperty]
        public string password_hash { get; set; }
        [JsonProperty]
        public string b64_user_pic { get; set; }
    }
    /*public void fb_get_(List<userrecord> obj)
    {
        foreach (var o in obj)
        {
            Debug.Log(
            o.h.ToString());
        }
    }*/

    public async void btn_logoff_onClick() // logoff
    {

        Texture2D t = new Texture2D(200, 200);
        byte[] b = File.ReadAllBytes(Application.streamingAssetsPath + "/pic.png");
        string b64 = System.Convert.ToBase64String(b);

        userrecord u = new userrecord { username = "test", password_hash = "ddsdsd", b64_user_pic = b64 };

        //string key = await firebase_comm.put_object_into_path(u, "qqq");

        List<fbResult<userrecord>> url = await firebase_comm.get_objects_byfield_from_path<userrecord>("qqq", "username", "test");

        //Debug.Log(key);

        byte[] b2 = System.Convert.FromBase64String(url[0].obj.b64_user_pic);

        t.LoadImage(b2);
        cam_out.texture = t;

        


        /*
        testrecord tr = new testrecord();
        tr.h = 444;

        firebase_comm.put_object_into_path(tr, "qqq");

        
        firebase_comm.get_objects_from_path<testrecord>(null, "qqq", fb_get_);
        */

        /*
        try
        {
            menu_init.inp_login.text = ""; // clear fields
            menu_init.inp_pwd.text = "";
            user u = new user();
            db_helper_login.set_last_user(u); // reset user
            menu_init.login.SetActive(true); // show login screen
        }
        catch
        {
            Debug.Log("Logoff error");
        }*/
    }



    public void btn_ladder_back_onClick() // hide scores on back click
    {
        ladderboard.SetActive(false); 
    }

    public void btn_ok_alert_click()
    {
        menu_init.alert.SetActive(false); // hide allert on "ok" click
    }


    public void btn_quit_onClick() //exit app
    {
        Application.Quit();
    }
}
