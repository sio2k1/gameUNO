using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class mgame_level
{
    [JsonProperty]
    public string lvl_name = "";
    [JsonProperty]
    public List<question> questions = new List<question>();
}

public class mgame_player
{
    [JsonProperty]
    public string player_name { get; set; }
    [JsonProperty]
    public string lvl { get; set; }
    [JsonProperty]
    public string playerkey { get; set; }
    [JsonProperty]
    public string gamekey { get; set; }
}
public class mgame 
{
    [JsonIgnore]
    public string key = "";
    [JsonProperty]
    public DateTime game_start = DateTime.UtcNow;
    [JsonProperty]
    public List<mgame_level> levels = new List<mgame_level>();
    [JsonIgnore]
    public bool originalgame = false; // it is true if we originally create this game, we want to delete it after completion it doesnot go to firebase 
}


