using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

// holdes user class for auth and userprefs
[DataContract]
public class user 
{
    [DataMember]
    public int id=-1;
    [DataMember]
    public string login="";
}

public class user_fb
{
    [JsonProperty]
    public string key = "";
    [JsonProperty]
    public string login = "";
    [JsonProperty]
    public string login_display = "";
    [JsonProperty]
    public string pwdhash = "";
    [JsonProperty]
    public string combinedloginhash = ""; // represent hash from (login+pwd+salt); salte stored in db_helper_login_firebase.salt
    [JsonProperty]
    public string userpic = "";
}
