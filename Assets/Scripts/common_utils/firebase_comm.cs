using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Database.Query;
using Firebase.Database;
using System.Threading.Tasks;
using System;
using Firebase.Auth;
using System.Linq;

/*
 * This is wrapper to firebasedatabase.net lib
 * It allows to select in path by JSON attribute value (dont forget to put index on it) :
 *       FB rules entry for selecting by particular field:
         "COLLECTIONNAME": {
            ".indexOn": ["FIELDNAME1","FIELDNAME"]
            }
         
 * For using modern auth make shure that anonymous login enabled to firebase. 
 */
public class fbResult<T> //this is query result return class, it includes firebase key and json in obj
{
    public string key { get; set; }
    public T obj { get; set; }
}
public static class firebase_comm 
{

    public static bool usemodernauth = true; //we can spesify to use legacy way of using DB secret (should be true for using modern auth)
    private static string OLDkey = "SfKwW9OaccZhRtkPs5kqFdNqT7qkfSh1pAv5si6J"; // db secret for legacy auth method
    private static string APIKEY="AIzaSyCLv3zAYAslktqJm4GyuboymcYxBVjCT3M"; // project auth key for modern auth
    private static string APIurl = "https://knights-and-users.firebaseio.com/"; // project URL

    public static async Task<FirebaseClient> fbConnection_auth() // connection, returns ready to use (for queries) firebase client
    {
        if (usemodernauth) // modern auth way
        {
            var auth = new FirebaseAuthProvider(new FirebaseConfig(APIKEY));
            var data = await auth.SignInAnonymouslyAsync();
            return new FirebaseClient(
              APIurl,
              new FirebaseOptions
              {
                  AuthTokenAsyncFactory = () => Task.FromResult(data.FirebaseToken)
              });
        } else // legacy auth way
        {
            return new FirebaseClient(
              APIurl,
              new FirebaseOptions
              {
                  AuthTokenAsyncFactory = () => Task.FromResult(OLDkey)
              });
        }
    }

    public static async Task<List<fbResult<T>>> get_objects_byfield_from_path<T>(string path, string property, string value) // query jsons in path by field value
    {
        List<fbResult<T>> reslst = new List<fbResult<T>>();
        try
        {
            FirebaseClient fbc = await fbConnection_auth();
            var res = await fbc.Child(path).OrderBy(property).EqualTo(value).OnceAsync<T>();
            res.ToList().ForEach(o =>
            {
                reslst.Add(new fbResult<T> { key = o.Key, obj = o.Object }); //put returned values into result
            });
        } catch (Exception e)
        {
            Debug.LogError(e.Message);
        }
        /*foreach (var o in res)
        {
            fbResult<T> r = new fbResult<T> { key = o.Key, obj = o.Object }; 
            reslst.Add(r);
        }*/
        return reslst;
    }

    public static async Task<List<fbResult<T>>> get_all_objects_from_path<T>(string path) // query all jsons form specified path
    {
        List<fbResult<T>> reslst = new List<fbResult<T>>();
        try { 
            FirebaseClient fbc = await fbConnection_auth();
            var res = await fbc.Child(path).OnceAsync<T>();
            res.ToList().ForEach(o =>
            {
                reslst.Add(new fbResult<T> { key = o.Key, obj = o.Object });  //put returned values into result
            });
        } catch (Exception e)
        {
            Debug.LogError(e.Message);
        }
        /*
        foreach (var o in res) 
        {
            fbResult<T> r = new fbResult<T> { key = o.Key, obj = o.Object };
            reslst.Add(r);
        }*/
        return reslst;
    }

    public static async Task<bool> update_object_into_path_key<T>(T obj, string path, string key) // update JSON at specified path and key
    {
        try
        {
            FirebaseClient fbc = await fbConnection_auth();
            await fbc.Child(path).Child(key).PatchAsync<T>(obj);
            return true;
        } catch (Exception e)
        {
            Debug.LogError(e.Message);
            return false;
        }
    }

    public static async Task<bool> delete_object_from_path_key(string path, string key) //delete JSON at specified path and key
    {
        try { 
            FirebaseClient fbc = await fbConnection_auth();
            await fbc.Child(path).Child(key).DeleteAsync();
            return true;
        } catch (Exception e)
        {
            Debug.LogError(e.Message);
            return false;
        }
    }

    public static async Task<string> put_object_into_path<T>(T obj, string path) //add JSON at specified path and return generated key
    {
        try
        {
            FirebaseClient fbc = await fbConnection_auth();
            var res = await fbc.Child(path).PostAsync(obj);
            return res.Key;
        } catch (Exception e)
        {
            Debug.LogError(e.Message);
            return "";
        }

    }
}
