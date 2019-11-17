using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class mgame_level // level calss for multiplayer game
{
    [JsonProperty]
    public string lvl_name = ""; // level name
    [JsonProperty]
    public List<question> questions = new List<question>(); // list of questions for level
}

public class mgame_player // palyer class for multiplayer game 
{
    [JsonProperty]
    public string player_name { get; set; } 
    [JsonProperty]
    public string lvl { get; set; } // level name
    [JsonProperty]
    public string playerkey { get; set; } // player id FB
    [JsonProperty]
    public string gamekey { get; set; } // game id FB
    [JsonProperty]
    public int playerpic { get; set; } // pic ID
    [JsonProperty]
    public DateTime last_active_at_screen { get; set; } // date for last activity, if last activity time exeeded some treshhold value we stop display player at level (in case player abandoned game or lost internet connection)

}
public class mgame // defines multiplayer game
{
    [JsonIgnore]
    public string key = ""; // gmae key
    [JsonProperty]
    public DateTime game_start = DateTime.UtcNow;
    [JsonProperty]
    public List<mgame_level> levels = new List<mgame_level>(); 
    [JsonProperty]
    public string created_by_player_key = "";
}

public class mgame_scoreboard_entry // scoreboard entry
{
    [JsonProperty]
    public string gamekey { get; set; }
    [JsonProperty]
    public string player_key { get; set; }
    [JsonProperty]
    public string player_display_name { get; set; }
    [JsonProperty]
    public int score { get; set; }
    [JsonProperty]
    public DateTime record_time_utc { get; set; }
}

public class mgame_scoreboard_game_entry
{
    public string gamekey;
    public List<mgame_scoreboard_entry> lines = new List<mgame_scoreboard_entry>(); 
}


