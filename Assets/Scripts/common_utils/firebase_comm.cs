using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Database.Query;
using Firebase.Database;
using System.Threading.Tasks;
using System;
using Firebase.Auth;
using System.Linq;

public delegate void firebase_select_callback<T>(List<T> obj);
public static class firebase_comm 
{
   
    //private static string key = "SfKwW9OaccZhRtkPs5kqFdNqT7qkfSh1pAv5si6J";
    private static string APIKEY="AIzaSyCLv3zAYAslktqJm4GyuboymcYxBVjCT3M";
    private static string APIurl = "https://knights-and-users.firebaseio.com/";
    //public static string last_inserted = "";


    public static async void get_objects_from_path<T>(T obj, string path, firebase_select_callback<T> cb)
    {
        var auth = new FirebaseAuthProvider(new FirebaseConfig(APIKEY));
        var data = await auth.SignInAnonymouslyAsync();
        var firebaseClient = new FirebaseClient(
          APIurl,
          new FirebaseOptions
          {
              AuthTokenAsyncFactory = () => Task.FromResult(data.FirebaseToken)
          });
        var res = await firebaseClient
       .Child(path).OnceAsync<T>();

        List<T> reslst = new List<T>();
        foreach (var o in res)
        {
            reslst.Add(o.Object);
        }

        cb.Invoke(reslst);


    }
    public static async void put_object_into_path<T>(T obj, string path)
    {
        var auth = new FirebaseAuthProvider(new FirebaseConfig(APIKEY));
        var data = await auth.SignInAnonymouslyAsync();
        var firebaseClient = new FirebaseClient(
          APIurl,
          new FirebaseOptions
          {
              AuthTokenAsyncFactory = () => Task.FromResult(data.FirebaseToken)
          });
        //var firebaseClient = new FirebaseClient(APIurl);
        //Type type = obj.GetType();
        var res = await firebaseClient
        .Child(path)
        .PostAsync(obj);




        // note that there is another overload for the PostAsync method which delegates the new key generation to the firebase server
        Debug.Log($"Key for the new dinosaur: {res.Key}");
        debub_console_log.msg = res.Key;
    }
}
