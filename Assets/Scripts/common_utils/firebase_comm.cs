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
public delegate void firebase_put_callback<T>(string last_inserted_id);

public class fbResult<T>
{
    public string key { get; set; }
    public T obj { get; set; }
}
public static class firebase_comm 
{
   
    //private static string key = "SfKwW9OaccZhRtkPs5kqFdNqT7qkfSh1pAv5si6J";
    private static string APIKEY="AIzaSyCLv3zAYAslktqJm4GyuboymcYxBVjCT3M";
    private static string APIurl = "https://knights-and-users.firebaseio.com/";
    //public static string last_inserted = "";

    public static async Task<FirebaseClient> fbConnection_auth()
    {
        var auth = new FirebaseAuthProvider(new FirebaseConfig(APIKEY));
        var data = await auth.SignInAnonymouslyAsync();
        return new FirebaseClient(
          APIurl,
          new FirebaseOptions
          {
              AuthTokenAsyncFactory = () => Task.FromResult(data.FirebaseToken)
          });
    }

    public static async Task<List<fbResult<T>>> get_objects_byfield_from_path<T>(string path, string property, string value)
    {
        /* FB rules entry for selecting by particular field
         "COLLECTIONNAME": {
            ".indexOn": ["FIELDNAME1","FIELDNAME"]
            }
         */
        FirebaseClient fbc = await fbConnection_auth();
        var res = await fbc
       .Child(path).OrderBy(property).EqualTo(value).OnceAsync<T>();
        List<fbResult<T>> reslst = new List<fbResult<T>>();
        foreach (var o in res)
        {
            fbResult<T> r = new fbResult<T> { key = o.Key, obj = o.Object }; 
            reslst.Add(r);
        }
        return reslst;
    }

    public static async Task<List<fbResult<T>>> get_all_objects_from_path<T>(string path)
    {
        FirebaseClient fbc = await fbConnection_auth();
        var res = await fbc
       .Child(path).OnceAsync<T>();
        List<fbResult<T>> reslst = new List<fbResult<T>>();
        foreach (var o in res)
        {
            fbResult<T> r = new fbResult<T> { key = o.Key, obj = o.Object };
            reslst.Add(r);
        }
        return reslst;
    }


    public static async Task<string> put_object_into_path<T>(T obj, string path)
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

        return res.Key;

        //Debug.Log($"Key for the new record: {res.Key}");
        //debub_console_log.msg = res.Key;
    }
    /*
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


    //Debug.Log($"Key for the new record: {res.Key}");
    //debub_console_log.msg = res.Key;
}*/
}
