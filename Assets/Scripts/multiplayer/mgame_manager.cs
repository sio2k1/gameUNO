using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System;
using UnityEngine;

public static class mgame_manager
{
    const string gamepath = "games";
    private static mgame create_new_game()
    {
        mgame g = new mgame();
        g.originalgame = true; // we created this game, and we will delete it after game is finished.
        levelnames.names.ForEach(lname => { // populate game with levels
            mgame_level lvl = new mgame_level();
            lvl.lvl_name = lname;
            lvl.questions = db_helper_questions.load_questions(lname, global_debug_state.questions_per_level); // get questions from db. for debug reasons we load question number from global_debug_state
            g.levels.Add(lvl);
        });

        return g;
    }

    public async static Task<bool> move_to_screen(string player_name_, string level_name, string game_key, string current_user_key) // move player to screen in particular game
    {
        bool res = false;
        try
        {
            string currgamepath = gamepath + "/" + game_key; // game path

            await firebase_comm.delete_object_byfield_from_path<mgame_player>(currgamepath, "playerkey", current_user_key); // delete all records about player movements

            await firebase_comm.put_object_into_path(new mgame_player { lvl = level_name, player_name = player_name_, playerkey=current_user_key }, currgamepath); // assign player to lvl and make a record

        }
        catch (Exception e)
        {
            Debug.Log("Unable to create game:" + e.Message);
        }

        return res;
    }

    public async static Task<mgame> get_or_start_new_multiplayer_game() // we get available game, if its outdated or not exists we create new
    {
        mgame g = new mgame();

        try
        {
            List<fbResult<mgame>> res = await firebase_comm.get_all_objects_from_path<mgame>(gamepath); // get current game (s)
            if (res.Count == 0)
            {
                g = create_new_game(); // if there is no game we  create new game
                g.key = await firebase_comm.put_object_into_path(g, gamepath); // put new game in db
            }
            else
            {
                res.ForEach(async x =>
                {
                    if (x.obj.game_start.AddMinutes(15) < DateTime.UtcNow)
                    {
                        await firebase_comm.delete_object_from_path_key(gamepath, x.key); //delete outdated game
                                                                                          //res.Remove(x);
                    }
                });

                res.RemoveAll(x => x.obj.game_start.AddMinutes(15) < DateTime.UtcNow); // clear list

                if (res.Count != 0)
                {
                    g = res.FirstOrDefault().obj;
                    g.key = res.FirstOrDefault().key;
                }
                else
                {
                    g = create_new_game(); //create new game
                    g.key = await firebase_comm.put_object_into_path(g, gamepath); //get firebase key for created game
                }



            }
        } catch (Exception e)
        {
            Debug.Log("Unable to create game:" + e.Message);
        }

        return g;
    }
}
