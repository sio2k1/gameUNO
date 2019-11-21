using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System;
using UnityEngine;

public static class mgame_manager
{
    const string game_path = "games"; // FB path to games
    const string score_spath = "scores";// FB path to scores
    const string players_in_game_path = game_path + "_players"; // firebase path players

    public async static Task<List<mgame_scoreboard_game_entry>> scores_get_score_from_fb(user_fb user) // write score to firebase
    {
        List<mgame_scoreboard_game_entry> res = new List<mgame_scoreboard_game_entry>();
        try
        {
            List<fbResult<mgame_scoreboard_entry>> fb_reply = await firebase_comm.get_objects_byfield_from_path<mgame_scoreboard_entry>(score_spath, "player_key", user.key);

            var lastgames = (from g in fb_reply     
                        orderby g.obj.record_time_utc descending
                        select g).Take(5);


            foreach (var game in lastgames)
            {
                List<fbResult<mgame_scoreboard_entry>> fbr = await firebase_comm.get_objects_byfield_from_path<mgame_scoreboard_entry>(score_spath, "gamekey", game.obj.gamekey);
                mgame_scoreboard_game_entry game_entry = new mgame_scoreboard_game_entry();
                game_entry.gamekey = game.key;
                fbr.ForEach(y =>
                {
                    game_entry.lines.Add(y.obj);
                });
                res.Add(game_entry);

            }




        }
        catch (Exception e)
        {
            Debug.Log("Unable get games from the scoreboard:" + e.Message);
        }
        return res;
    }
    public async static Task<bool> scores_write_score_to_fb(string game_key, user_fb user, int score_) // write score to firebase
    {
        bool res = false;
        try
        {
            await firebase_comm.put_object_into_path(
                new mgame_scoreboard_entry { gamekey = game_key, player_display_name = user.login_display, player_key = user.key, record_time_utc = DateTime.UtcNow, score = score_ } //new entry init
                , score_spath);
            res = true;
        } catch (Exception e)
        {
            Debug.Log("Unable write to scoreboard:" + e.Message);
        }
        return res;
    }
    public async static Task<List<mgame_player>> get_all_players_in_game_at_level(string game_key, string level_name) // request all players within a particular game at level
    {
        List<mgame_player> players = new List<mgame_player>();
        try
        {
            List<fbResult<mgame_player>> res = await firebase_comm.get_objects_byfield_from_path<mgame_player>(players_in_game_path, "gamekey", game_key);
            res.FindAll(x => x.obj.lvl == level_name).ForEach(x => players.Add(x.obj)); //select all players within current level and form list of players
        }
        catch (Exception e)
        {
            Debug.Log("Unable to get list of players:" + e.Message);
        }
        return players;

    }

    public async static Task<bool> move_to_screen(string player_name_, string level_name, string game_key, string current_user_key, int pic_id) // move player to screen in particular game
    {
        bool res = false;
        try
        {
            await firebase_comm.delete_object_byfield_from_path<mgame_player>(players_in_game_path, "playerkey", current_user_key); // delete all records about player movements
            await firebase_comm.put_object_into_path(new mgame_player { lvl = level_name, player_name = player_name_, playerkey = current_user_key, gamekey = game_key, playerpic=pic_id, last_active_at_screen=DateTime.UtcNow }, players_in_game_path); // assign player to lvl and make a record
        }
        catch (Exception e)
        {
            Debug.Log("Unable to change screen:" + e.Message);
        }

        return res;
    }

    private static mgame create_new_game(string current_user_key) // new game creation
    {
        mgame g = new mgame();
        g.created_by_player_key = current_user_key;
        //g.originalgame = true; // we created this game, and we will delete it after game is finished.
        levelnames.names.ForEach(lname => { // populate game with levels
            mgame_level lvl = new mgame_level();
            lvl.lvl_name = lname;
            lvl.questions = db_helper_questions.load_questions(lname, global_debug_state.questions_per_level); // get questions from db. for debug reasons we load question number from global_debug_state
            g.levels.Add(lvl);
        });

        return g;
    }



    public async static Task<mgame> get_or_start_new_multiplayer_game(string current_user_key, string previous_game_key) // we get available game, if its outdated or not exists we create new
    {
        mgame g = new mgame();

        try
        {
            List<fbResult<mgame>> res = await firebase_comm.get_all_objects_from_path<mgame>(game_path); // get current game (s)
            if (res.Count == 0)
            {
                g = create_new_game(current_user_key); // if there is no game we  create new game
                g.key = await firebase_comm.put_object_into_path(g, game_path); // put new game in db
            }
            else
            {
                res.ForEach(async x =>
                {
                    if ((x.obj.game_start.AddMinutes(15) < DateTime.UtcNow) || (x.obj.created_by_player_key == current_user_key) || (x.key == previous_game_key)) // delete all outdated and curent game, and if we had created this game in past, and if game is older then X minutes
                    {
                        await firebase_comm.delete_object_from_path_key(game_path, x.key); //delete outdated game                                                                 //res.Remove(x);
                    }
                });

                res.RemoveAll(x => (x.obj.game_start.AddMinutes(15) < DateTime.UtcNow) || (x.obj.created_by_player_key == current_user_key) || (x.key == previous_game_key)); // clear list

                res = res.OrderByDescending(x => x.obj.game_start).ToList();

                if (res.Count != 0)
                {
                    g = res.First().obj;
                    g.key = res.First().key;
                }
                else
                {
                    g = create_new_game(current_user_key); //create new game
                    g.key = await firebase_comm.put_object_into_path(g, game_path); //get firebase key for created game
                }

            }
        } catch (Exception e)
        {
            Debug.Log("Unable to create game:" + e.Message);
        }

        return g;
    }
}
