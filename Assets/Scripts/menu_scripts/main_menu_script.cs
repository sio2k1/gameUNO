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
    public async void btn_play_onClick()
    {
        //SceneManager.LoadScene(1); //Basically loads a gamelevel scene


       // await mgame_manager.move_to_screen(app_globals.loggined_user_fb.login_display, "_2west", "gamekey", app_globals.loggined_user_fb.key);
       // List<mgame_player> plist = await mgame_manager.get_all_players_in_game_at_level("gamekey", "_2west");

       // Debug.Log(plist.Count);
        
        current_mgame.curr_mgame = await mgame_manager.get_or_start_new_multiplayer_game(current_mgame.curr_mgame.key);
        //app_globals.loggined_user_fb
        if (current_mgame.curr_mgame.key!="")
        {
            //start new game here
            SceneManager.LoadScene(1); //Basically loads a gamelevel scene                             
        }
        Debug.Log(current_mgame.curr_mgame.key);
    }

    public void btn_ladder_onClick()
    {
        ladderboard.SetActive(true); //show up scores stats
        ladderboard.GetComponent<ladder_cl>().load_scores(); //load scores form db
    }



    private void Start()
    {
        //global_debug_state.use_debug();
    }

    private static void UnpackToPersistentDataPathFromStreaming(string fileName) //unpack db to local folder at android
    {
        string toPath = Path.Combine(Application.persistentDataPath, fileName);
        string fromPath = Path.Combine(Application.streamingAssetsPath, fileName);
        WWW reader = new WWW(fromPath);
        while (!reader.isDone) { }
        File.WriteAllBytes(toPath, reader.bytes);
    }

    public async void btn_logoff_onClick() // logoff
    {
        /*
        Texture2D t = new Texture2D(200, 200);
        string flpath = "";
#if UNITY_EDITOR
        flpath = Path.Combine(Application.streamingAssetsPath, "pic.png");
#endif

#if UNITY_ANDROID
        UnpackToPersistentDataPathFromStreaming("pic.png");
        flpath = Path.Combine(Application.persistentDataPath, "pic.png");
#endif
        if (flpath=="")
        {
            throw new System.Exception("No path defined");
        }

        debub_console_log.msg = flpath;

        byte[] b = File.ReadAllBytes(Path.Combine(Application.streamingAssetsPath, "pic.png"));
        debub_console_log.msg = flpath+ "READED";
        //Path.Combine(Application.persistentDataPath, fileName)
        string b64 = System.Convert.ToBase64String(b);

        userrecord u = new userrecord { username = "testqQQQ11aaaZZ", password_hash = "ddsdsd", b64_user_pic = "a" , arr = new List<string>() };
        u.arr.Add("sss");
        u.arr.Add("sss2");
        await firebase_comm.update_object_into_path_key<userrecord>(u, "qqq", "-LtJTgcCpQ0hltFjwmpz");


        List<fbResult<userrecord>> usr = await firebase_comm.get_objects_byfield_from_path<userrecord>("qqq", "username", "testqQQQ11aaaZZ");

        */

        //await firebase_comm.delete_object_from_path_key("qqq", "-LtJTgcCpQ0hltFjwmpz");

        //debub_console_log.msg = "updated";
        //string key = await firebase_comm.put_object_into_path(u, "qqq");

        //List<fbResult<userrecord>> url = await firebase_comm.get_objects_byfield_from_path<userrecord>("qqq", "username", "test");
        //debub_console_log.msg = url.Count.ToString();
        //Debug.Log(key);

        // byte[] b2 = System.Convert.FromBase64String(url[0].obj.b64_user_pic);

        //t.LoadImage(b2);
        //cam_out.texture = t;




        /*
        testrecord tr = new testrecord();
        tr.h = 444;

        firebase_comm.put_object_into_path(tr, "qqq");

        
        firebase_comm.get_objects_from_path<testrecord>(null, "qqq", fb_get_);
        */

        
        try
        {
            menu_init.inp_login.text = ""; // clear fields
            menu_init.inp_pwd.text = "";
            app_globals.loggined_user_fb = new user_fb();
            db_helper_common.set_setting("lastuserid", "");
            //user u = new user();
            //db_helper_login.set_last_user(u); // reset user
            menu_init.login.SetActive(true); // show login screen
        }
        catch
        {
            Debug.Log("Logoff error");
        }
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
