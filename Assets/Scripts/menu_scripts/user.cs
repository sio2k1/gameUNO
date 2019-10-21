using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

[DataContract]
public class user 
{
    [DataMember]
    public int id=-1;
    [DataMember]
    public string login="";
}
